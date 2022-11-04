using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepPlayer : MonoBehaviour
{
    public AudioClip[] Sounds;
    public float PlayCooldown = 0.6f;

    int soundID = 0;
    float nextPlayCooldown = 0.0f;

    new AudioSource audio;

    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if ( ( nextPlayCooldown -= Time.deltaTime ) <= 0.0f )
        {
            Play();
        }
    }

    public void Play()
    {
        soundID = ( soundID + Random.Range( 1, 3 ) ) % Sounds.Length;
        audio.PlayOneShot( Sounds[soundID] );

        nextPlayCooldown = PlayCooldown;
    }
}
