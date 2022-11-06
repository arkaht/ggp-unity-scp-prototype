using UnityEngine;

public class GasTrigger : Trigger
{
	public int HurtDamage = 10;
	public float HurtCooldown = 0.1f;

	[TextArea( 3, 10 )]
	public string DeathMessage;

	float hurtCooldown = 0.0f;

	protected override void Awake()
	{
		base.Awake();

		color = Color.red;
	}

	void OnTriggerStay( Collider other )
	{
		Player player = Player.Instance;
		if ( other.gameObject != player.gameObject ) return;

		if ( !player.IsAlive || player.IsGasMaskEquiped ) 
		{
			player.InGas = false;
			return;
		}

		if ( ( hurtCooldown -= Time.deltaTime ) <= 0.0f )
		{
			player.TakeDamage( HurtDamage, DeathMessage );
			hurtCooldown = HurtCooldown;
		}

		player.InGas = true;
	}

	void OnTriggerExit( Collider other )
	{
		Player player = Player.Instance;
		if ( other.gameObject != player.gameObject ) return;

		player.InGas = false;
	}
}