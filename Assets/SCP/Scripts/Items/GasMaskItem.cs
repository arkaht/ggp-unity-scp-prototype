using UnityEngine;

public class GasMaskItem : Item
{
	[Header( "Properties" )]
	public float FogDensityMultiplier = 1.5f;

	float startFogDensity;

	void Start()
	{
		startFogDensity = RenderSettings.fogDensity;
	}

	public override void OnEquiped()
	{
		//  play drop sound
		AudioNotification.PlayAudioAt( transform.position, DropSound, 0.5f );

		GasMaskUI.Instance.Show();

		//  apply properties
		RenderSettings.fogDensity = startFogDensity * FogDensityMultiplier;
	}

	public override void OnUnEquiped()
	{
		//  play drop sound
		AudioNotification.PlayAudioAt( transform.position, DropSound, 0.5f );

		GasMaskUI.Instance.Hide();

		//  apply properties
		RenderSettings.fogDensity = startFogDensity;
	}
}