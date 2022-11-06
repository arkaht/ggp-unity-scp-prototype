using System.Collections;
using UnityEngine;

public class ScriptedEvent_SCP173Intro : ActivableEntity
{
	public Transform SCP173Spawn;
	public LightFlickerEffect[] FlickedLights;
	public float BlackoutTime = 1.0f;
	public Vector2 LightOffTimeRange = new( 0.1f, 0.2f ); 

	[Header( "Sounds" )]
	public AudioClip SCP173SpawnSound;
	public AudioClip[] LightOffSounds;
	public AudioClip LightOnSound;

	new AudioSource audio;

	void Awake()
	{
		audio = GetComponent<AudioSource>();
	}

	public override bool Activate( Player player, MonoBehaviour caller )
	{
		StartCoroutine( Coroutine_Blackout() );
		return true;
	}

	IEnumerator Coroutine_Blackout()
	{
		audio.Stop();

		foreach ( LightFlickerEffect flicker in FlickedLights )
		{
			//  turn lights off
			flicker.enabled = false;
			flicker.Light.intensity = 0.0f;

			//  play light off sound
			AudioNotification.PlayAudioAt( flicker.transform.position, LightOffSounds );

			yield return new WaitForSeconds( Random.Range( LightOffTimeRange.x, LightOffTimeRange.y ) );
		}

		//  disable 173
		SCP173 scp173 = SCP173.Instance;
		scp173.enabled = false;

		//  wait some time
		yield return new WaitForSeconds( BlackoutTime );

		AudioNotification.PlayAudioAt( Vector3.zero, LightOnSound, 0.75f );

		//  enable & warp 173
		scp173.enabled = true;
		scp173.transform.position = SCP173Spawn.transform.position;
		scp173.transform.rotation = SCP173Spawn.transform.rotation;

		//  play spawn sound
		var notif = AudioNotification.PlayAudioAt( scp173.transform.position, SCP173SpawnSound );
		notif.Audio.minDistance = 3.0f;

		//  re-enable lights
		foreach ( LightFlickerEffect flicker in FlickedLights )
		{
			flicker.Light.intensity = flicker.IntensityRange.x;
		}
	}
}