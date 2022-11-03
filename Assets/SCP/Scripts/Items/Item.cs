using UnityEngine;

public class Item : UseableEntity
{
	public Player Owner { get; private set; }

	public override bool CanUse( Player player )
	{
		if ( Owner != null ) return false;
		if ( player.IsInventoryFull ) return false;

		return base.CanUse( player );
	}

	protected override void OnUse( Player player )
	{
		//  register in inventory
		player.Inventory.Add( this );

		//  setup transform
		transform.SetParent( player.transform );
		transform.localPosition = Vector3.zero;

		//  set owner
		Owner = player;
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
		transform.SetParent( null );
		transform.position = pos;

		//  remove owner references
		Owner.Inventory.Remove( this );
		Owner = null;
	}
}
