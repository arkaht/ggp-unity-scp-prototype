using UnityEngine;

public class Item : UseableEntity
{
	public Sprite Sprite;
	public AudioClip[] PickSounds;

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
		AudioSource audio = AudioNotification.PlayAudioAt( player.transform.position, PickSounds );
		audio.volume = 0.5f;

		//  register in inventory
		player.AddItemToInventory( this );

		//  reset use entity
		player.UseEntity = null;
	}

	public void Drop()
	{
		if ( Owner == null ) return;

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
}
