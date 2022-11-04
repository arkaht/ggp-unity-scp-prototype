using System.Collections;
using UnityEngine;


public class AudioNotification : MonoBehaviour
{
    public static AudioSource PlayAudioAt( Vector3 pos, AudioClip sound )
    {
        GameObject obj = new GameObject( "AudioNotification" );
        obj.transform.position = pos;

        //  add audio source
        AudioSource audio = obj.AddComponent<AudioSource>();

        //  add audio notification
        obj.AddComponent<AudioNotification>().Sound = sound;

        return audio;
    }
    public static AudioSource PlayAudioAt( Vector3 pos, AudioClip[] sounds )
    {
        return PlayAudioAt( pos, sounds[Random.Range( 0, sounds.Length )] );
    }

    public AudioClip Sound;

    new AudioSource audio;

    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    void Start()
    {
        audio.PlayOneShot( Sound );     
    }

    void Update()
    {
        if ( !audio.isPlaying )
        {
            Destroy( gameObject );
        }
    }
}