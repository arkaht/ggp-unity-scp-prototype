using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( AudioSource ) )]
public class FootstepPlayer : MonoBehaviour
{
	public AudioClip[] Sounds;
	public float PlayCooldown = 0.6f;

	public bool WaitAudioStop = false;
	public int MaxIncrease = 3;

	int soundID = 0;
	float nextPlayCooldown = 0.0f;

	new AudioSource audio;

	void Awake()
	{
		audio = GetComponent<AudioSource>();
	}

	void Update()
	{
		if ( WaitAudioStop && audio.isPlaying ) return;

		if ( ( nextPlayCooldown -= Time.deltaTime ) <= 0.0f )
		{
			Play();
		}
	}

	public void Play()
	{
		soundID = ( soundID + Random.Range( 1, MaxIncrease ) ) % Sounds.Length;
		audio.PlayOneShot( Sounds[soundID] );

		nextPlayCooldown = PlayCooldown;
	}
}
