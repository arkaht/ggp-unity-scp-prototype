using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : Trigger
{
	public AudioClip[] Sounds;
	public bool SingleUse = true;
	public Vector3 PositionRange;

	new AudioSource audio;

	protected override void Awake()
	{
		base.Awake();

		audio = GetComponent<AudioSource>();
	
		color = Color.cyan;	
	}

	void OnTriggerEnter( Collider other )
	{
		if ( other.gameObject != Player.Instance.gameObject ) return;

		//  play sound
		audio.PlayOneShot( Utils.GetRandomElement( Sounds ) );

		//  disable on single use
		if ( SingleUse )
		{
			collider.enabled = false;
		}

		//  translate play position
		if ( PositionRange != Vector3.zero )
		{
			float angle = Mathf.Deg2Rad * Random.Range( 0.0f, 360.0f );
			transform.position += new Vector3( Mathf.Cos( angle ) * PositionRange.x, Mathf.Sin( angle ) * PositionRange.y, Mathf.Tan( angle ) * PositionRange.z );
		}
	}
}
