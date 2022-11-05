using UnityEngine;

public enum ItemSlotType
{
	Hand,
	Helmet,
	// BODY,
}

public class Item : UseableEntity
{
	public ItemSlotType Type = ItemSlotType.Hand;

	public Sprite Sprite;
	public AudioClip PickSound;
	public AudioClip DropSound;

	public Player Owner { get; set; }
	public int InventoryID { get; set; }

	public override bool CanUse( Player player )
	{
		if ( Owner != null ) return false;
		if ( player.IsInventoryFull ) return false;

		return base.CanUse( player );
	}

	protected override void OnUse( Player player )
	{
		//  play pick up sound
		AudioNotification.PlayAudioAt( transform.position, PickSound, 0.5f );

		//  register in inventory
		player.AddItemToInventory( this );

		//  remove from useables
		player.RemoveUseable( this );
	}

	public void Drop()
	{
		if ( Owner == null ) return;

		//  play drop sound
		AudioNotification.PlayAudioAt( transform.position, DropSound, 0.5f );

		//  prepare raycast
		float dist = Owner.DropItemDistance;
		Vector3 pos = Owner.ViewPos, dir = Owner.ViewDir;

		//  get drop pos
		if ( Physics.Raycast( pos, dir, out RaycastHit hit, dist  ) )
		{
			pos = hit.point;
		}
		else
		{
			pos += dir * dist;
		}

		//  setup transform
		transform.position = pos;

		//  remove owner references
		Owner.RemoveItemFromInventory( this );
	}

	public virtual void OnEquiped() {}
	public virtual void OnUnEquiped() {}
}
