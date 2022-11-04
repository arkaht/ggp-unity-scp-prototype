using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public bool IsSelected => Item == Player.Instance.EquipedItem;
	public int InventoryID { get; set; }

	public Color EquipedColor;
	public Image OutlineImage;
	public Image ItemImage;

	public Item Item { get; private set; }

	GraphicRaycaster raycaster;

	public void SetItem( Item item )
	{
		Item = item;

		//  update image
		if ( item == null )
		{
			ItemImage.enabled = false;
			OutlineImage.color = Color.white;
		}
		else
		{
			OutlineImage.color = Item == Player.Instance.EquipedItem ? EquipedColor : Color.white;

			ItemImage.enabled = true;
			ItemImage.sprite = item.Sprite;
		}
	}

	public void OnBeginDrag( PointerEventData eventData )
	{
		if ( Item == null ) return;

		ItemImage.transform.SetParent( ItemImage.canvas.transform );
	}

	public void OnDrag( PointerEventData data )
	{
		if ( Item == null ) return;

		ItemImage.transform.localPosition += (Vector3) data.delta;
	}

	public void OnEndDrag( PointerEventData data )
	{
		if ( Item == null ) return;

		if ( raycaster != null || ItemImage.canvas.TryGetComponent( out raycaster ) )
		{
			List<RaycastResult> results = new();
			raycaster.Raycast( data, results );

			//  re-order our slots
			bool is_success = false;
			foreach ( var hit in results )
			{
				if ( hit.gameObject.transform.parent.TryGetComponent( out InventorySlotUI slot ) )
				{
					//  move item
					if ( slot.Item == null )
						Player.Instance.MoveItemTo( InventoryID, slot.InventoryID );
					//  swap items
					else
						Player.Instance.SwapItems( slot.InventoryID, InventoryID );

					//  swap our slots
					Item slot_item = slot.Item;
					slot.SetItem( Item );
					SetItem( slot_item );

					is_success = true;
					break;
				}

			}

			//  off-inventory: drop item
			if ( !is_success )
			{
				Item.Drop();
				SetItem( null );
			}
		}

		//  reset dragged image transform
		ItemImage.transform.SetParent( transform );
		ItemImage.transform.localPosition = Vector2.zero;
	}

	public void OnPointerClick( PointerEventData data )
	{
		if ( Item == null ) return;
		if ( data.clickCount != 2 ) return;

		//  toggle equipping
		Player player = Player.Instance;
		if ( IsSelected )
		{
			player.EquipedItem = null;
		}
		else
		{
			//  update previous selected slot visual
			if ( player.EquipedItem != null )
			{
				InventorySlotUI slot = InventoryUI.Instance.GetSlotFor( player.EquipedItem );
				if ( slot != null )
				{
					slot.SetItem( slot.Item );
				}
			}

			player.EquipedItem = Item;
		}

		//  update visual
		SetItem( Item );
	}
}
