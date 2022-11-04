using UnityEngine;

public class UseableEntity : MonoBehaviour
{
	public float UseCooldown = 1.0f;
	float useCooldown = 0.0f;

	void Update()
	{
		//  decrease cooldown
		if ( useCooldown > 0.0f )
		{
			useCooldown -= Time.deltaTime;
		}
	}

	void OnTriggerEnter( Collider other )
	{
		var player = Player.Instance;
		if ( other.gameObject != player.gameObject ) return;
		if ( !CanUse( player ) ) return;

		player.AddUseable( this );
	}

	void OnTriggerExit( Collider other )
	{
		var player = Player.Instance;
		if ( other.gameObject != player.gameObject ) return;

		player.RemoveUseable( this );
	}

	public virtual bool CanUse( Player player )
	{
		return useCooldown <= 0.0f;
	}

	public void Use( Player player )
	{
		if ( !CanUse( player ) ) return;

		//  use
		OnUse( player );

		//  apply cooldown
		useCooldown = UseCooldown;
	}

	protected virtual void OnUse( Player player ) {}
}
