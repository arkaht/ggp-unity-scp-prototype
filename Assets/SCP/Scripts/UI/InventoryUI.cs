using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    public bool IsVisible => gameObject.activeSelf;

    public GameObject SlotPrefab;

    public Vector2 SlotPadding = new( 300.0f, 500.0f );

    public readonly List<InventorySlotUI> Slots = new();

    void Awake()
	{
        Instance = this;
	}

    void Start()
    {
        Player player = Player.Instance;

        //  generate slots
        int half_slots = player.MaxInventorySlots / 2;

        Vector3 offset = Vector3.zero;

        int x = 0, y = 0;
        for ( int i = 0; i < player.MaxInventorySlots; i++ )
		{
            //  create slot
            GameObject obj = Instantiate( SlotPrefab, transform );
            obj.transform.localPosition = new( x * SlotPadding.x, y * SlotPadding.y, 0.0f );

            //  setup & register slot
            InventorySlotUI slot = obj.GetComponent<InventorySlotUI>();
            slot.InventoryID = i;
            Slots.Add( slot );

            //  update offset
            offset.x = Mathf.Max( offset.x, obj.transform.localPosition.x );
            offset.y = Mathf.Max( offset.y, obj.transform.localPosition.y );
           
            //  manage position
            if ( ++x >= half_slots )
			{
                x = 0;
                y--;
			}
		}

        //  apply offset to center
        transform.position -= offset / 2;

        Hide();
    }

    public void Show()
	{
        gameObject.SetActive( true );

        //  update items in slots
        Player player = Player.Instance;

        int i = 0;
		foreach ( InventorySlotUI slot in Slots )
		{
            player.Inventory.TryGetValue( i++, out Item item );

            slot.SetItem( item );
		}

        Player.SetCursorLocked( false );
	}

    public void Hide()
	{
        gameObject.SetActive( false );

        Player.SetCursorLocked( true );
	}

    public void Toggle()
	{
        if ( IsVisible )
            Hide();
        else
            Show();
	}

    public InventorySlotUI GetSlotFor( Item item )
    {
        foreach ( var slot in Slots )
        {
            if ( slot.Item == item )
            {
                return slot;
            }
        }

        return null;
    }
}
