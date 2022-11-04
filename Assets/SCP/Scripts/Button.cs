using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : UseableEntity
{
	public ActivableEntity ActiveEntity;

	public AudioClip UseSound;

	new AudioSource audio;

	void Awake()
	{
		audio = GetComponent<AudioSource>();
	}

	protected override void OnUse( Player player )
	{
		if ( ActiveEntity == null ) 
		{
			Debug.LogError( "Button " + gameObject + " is not assigned to an activable entity!" );
			return;
		}

		//  activate
		ActiveEntity.Activate( player, this );

		//  play use sound
		audio.PlayOneShot( UseSound );
	}
}
