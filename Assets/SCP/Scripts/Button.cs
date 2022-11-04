using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : UseableEntity
{
	public UnityEvent useCallback;

	public AudioClip UseSound;

	new AudioSource audio;

	void Awake()
	{
		audio = GetComponent<AudioSource>();
	}

	protected override void OnUse( Player player )
	{
		if ( useCallback == null ) 
		{
			Debug.LogError( "Button " + gameObject + " is not assigned to any events!" );
			return;
		}

		useCallback.Invoke();
		audio.PlayOneShot( UseSound );
	}
}
