using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	public AudioClip[] OpenSound, CloseSound;

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
		
		animator.SetTrigger( is_open ? "Open" : "Close" );

		audio.PlayOneShot( isOpen ? OpenSound[Random.Range( 0, OpenSound.Length )] : CloseSound[Random.Range( 0, CloseSound.Length )] );
	}

	public void Toggle()
	{
		SetToggle( !isOpen );
	}
}
