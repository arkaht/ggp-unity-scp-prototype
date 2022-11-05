using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : ActivableEntity
{
	[Header( "Access Level" )]
	public int AccessLevel = 0;
	public AudioClip AuthorizedSound, RefusedSound;

	[Header( "Trigger Sounds" )]
	public AudioClip[] OpenSounds;
	public AudioClip[] CloseSounds;

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
		audio.PlayOneShot( isOpen ? Utils.GetRandomElement( OpenSounds ) : Utils.GetRandomElement( CloseSounds ) );
	}

	public void Toggle()
	{
		SetToggle( !isOpen );
	}

	public override bool Activate( Player player, UseableEntity caller )
	{
		//  check keycard required
		if ( AccessLevel > 0 )
		{
			var keycard = player.GetEquipedItem( ItemSlotType.Hand ) as KeycardItem;
			if ( keycard == null || keycard.AccessLevel < AccessLevel )
			{
				AudioNotification.PlayAudioAt( transform.position, RefusedSound, .5f );
				return false;
			}

			AudioNotification.PlayAudioAt( transform.position, AuthorizedSound, .5f );
		}

		Toggle();
		return true;
	}
}
