using UnityEngine;

public class GasTrigger : Trigger
{
	protected override void Awake()
	{
		base.Awake();

		color = Color.red;
	}

	void OnTriggerStay( Collider other )
	{
		Player player = Player.Instance;
		if ( other.gameObject != player.gameObject ) return;

		player.InGas = !player.IsGasMaskEquiped;
	}

	void OnTriggerExit( Collider other )
	{
		Player player = Player.Instance;
		if ( other.gameObject != player.gameObject ) return;

		player.InGas = false;
	}
}