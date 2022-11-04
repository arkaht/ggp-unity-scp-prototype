using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientPlayer : MonoBehaviour
{
    public AudioClip[] AmbientSounds;
    public Vector2 RangeCooldown;

    float nextSoundCooldown = 0.0f;

    new AudioSource audio;

    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if ( AmbientSounds.Length == 0 ) return;

        if ( !audio.isPlaying )
        {
            if ( ( nextSoundCooldown -= Time.deltaTime ) <= 0.0f )
            {
                NextSound();
            }
        }
    }

    public void NextSound()
    {
        audio.clip = Utils.GetRandomElement( AmbientSounds );
        audio.Play();

        nextSoundCooldown = Random.Range( RangeCooldown.x, RangeCooldown.y );
    }
}
