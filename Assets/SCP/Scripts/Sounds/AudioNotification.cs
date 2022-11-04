using System.Collections;
using UnityEngine;


public class AudioNotification : MonoBehaviour
{
	public static AudioSource PlayAudioAt( Vector3 pos, AudioClip sound, float volume = 1.0f )
	{
		GameObject obj = new GameObject( "AudioNotification" );
		obj.transform.position = pos;

		//  add audio source
		AudioSource audio = obj.AddComponent<AudioSource>();
		audio.volume = volume;

		//  add audio notification
		obj.AddComponent<AudioNotification>().Sound = sound;

		return audio;
	}
	public static AudioSource PlayAudioAt( Vector3 pos, AudioClip[] sounds, float volume = 1.0f )
	{
		return PlayAudioAt( pos, sounds[Random.Range( 0, sounds.Length )], volume );
	}

	public AudioClip Sound;

	new AudioSource audio;

	void Awake()
	{
		audio = GetComponent<AudioSource>();
	}

	void Start()
	{
		audio.PlayOneShot( Sound );     
	}

	void Update()
	{
		if ( !audio.isPlaying )
		{
			Destroy( gameObject );
		}
	}
}