using UnityEngine;

public class GasMaskItem : Item
{
	public AudioClip BreathSound;

	[Header( "Properties" )]
	public float FogDensityMultiplier = 1.5f;

	float startFogDensity;

	AudioNotification breathAudioNotif;

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

		//  play breath sound
		breathAudioNotif = AudioNotification.PlayAudioAt( Vector3.zero, BreathSound, .75f );
		breathAudioNotif.Audio.loop = true;
	}

	public override void OnUnEquiped()
	{
		//  play drop sound
		AudioNotification.PlayAudioAt( transform.position, DropSound, 0.5f );

		GasMaskUI.Instance.Hide();

		//  apply properties
		RenderSettings.fogDensity = startFogDensity;

		//  play breath sound
		if ( breathAudioNotif != null )
		{
			breathAudioNotif.FadeOut( 1.0f );
		}
	}
}