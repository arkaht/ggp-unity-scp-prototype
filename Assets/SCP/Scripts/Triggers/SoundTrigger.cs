using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : Trigger
{
	public AudioClip[] Sounds;
	public Vector3 PositionRange;

	new AudioSource audio;

	protected override void Awake()
	{
		base.Awake();

		audio = GetComponent<AudioSource>();
	
		color = Color.cyan;	
	}

	protected override void OnTrigger( Collider other )
	{
		//  play sound
		audio.PlayOneShot( Utils.GetRandomElement( Sounds ) );

		//  translate play position
		if ( PositionRange != Vector3.zero )
		{
			float angle = Mathf.Deg2Rad * Random.Range( 0.0f, 360.0f );
			transform.position += new Vector3( Mathf.Cos( angle ) * PositionRange.x, Mathf.Sin( angle ) * PositionRange.y, Mathf.Tan( angle ) * PositionRange.z );
		}
	}
}
