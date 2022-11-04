using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	public AudioClip[] OpenSounds, CloseSounds;

	bool isOpen = false;

	new AudioSource audio;
	Animator animator;

	void Awake()
	{
		audio = GetComponent<AudioSource>();
		animator = GetComponent<Animator>(); 
	}

	public void SetToggle( bool is_open )
	{
		isOpen = is_open;
		
		//  play animation
		animator.SetTrigger( is_open ? "Open" : "Close" );

		//  play trigger sound
		audio.PlayOneShot( isOpen ? OpenSounds[Random.Range( 0, OpenSounds.Length )] : CloseSounds[Random.Range( 0, CloseSounds.Length )] );
	}

	public void Toggle()
	{
		SetToggle( !isOpen );
	}
}
