using UnityEngine;

public class Item : UseableEntity
{
	public Sprite Sprite;

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
		//  register in inventory
		player.AddItemToInventory( this );
	}

	public void Drop()
	{
		if ( Owner == null ) return;

		//  prepare raycast
		float dist = Owner.DropItemDistance;
		Vector3 pos = Owner.ViewPos, dir = Owner.ViewDir;

		//  get drop pos
		if ( Physics.Raycast( pos, dir, out RaycastHit hit, dist ) )
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
}
