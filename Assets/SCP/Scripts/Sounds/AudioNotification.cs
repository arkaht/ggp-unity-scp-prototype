using System.Collections;
using UnityEngine;


public class AudioNotification : MonoBehaviour
{
	public static AudioNotification PlayAudioAt( Vector3 pos, AudioClip sound, float volume = 1.0f )
	{
		GameObject obj = new GameObject( "AudioNotification" );
		obj.transform.position = pos;

		//  add audio source
		AudioSource audio = obj.AddComponent<AudioSource>();
		audio.clip = sound;
		audio.volume = volume;
		
		//  spatialize
		if ( pos != Vector3.zero )
		{
			audio.spatialBlend = 1.0f;
		}

		//  add audio notification
		return obj.AddComponent<AudioNotification>();
	}
	public static AudioNotification PlayAudioAt( Vector3 pos, AudioClip[] sounds, float volume = 1.0f )
	{
		return PlayAudioAt( pos, sounds[Random.Range( 0, sounds.Length )], volume );
	}



	public AudioSource Audio { get; private set; }

	bool isFadingOut = false;
	float fadeDuration = 0.0f;
	float startFadeDuration = 0.0f;

	void Awake()
	{
		Audio = GetComponent<AudioSource>();
	}

	void Start()
	{
		Audio.Play();
	}

	void Update()
	{
		if ( isFadingOut )
		{
			if ( ( fadeDuration -= Time.deltaTime ) <= 0.0f )
			{
				Destroy( gameObject );
				isFadingOut = false;
			}

			//  fade volume
			Audio.volume = Mathf.Lerp( 0.0f, Audio.volume, fadeDuration / startFadeDuration );
		}

		if ( !Audio.isPlaying )
		{
			Destroy( gameObject );
		}
	}

	public void FadeOut( float duration )
	{
		isFadingOut = true;
		fadeDuration = duration;
		startFadeDuration = fadeDuration;
	}
}