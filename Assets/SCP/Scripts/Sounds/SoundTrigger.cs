using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    public AudioClip[] Sounds;
    public bool SingleUse = true;
    public Vector3 PositionRange;

    new AudioSource audio;
    new Collider collider;

    void Awake()
    {
        audio = GetComponent<AudioSource>();
        collider = GetComponent<Collider>();
    }

    void OnTriggerEnter( Collider other )
    {
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
