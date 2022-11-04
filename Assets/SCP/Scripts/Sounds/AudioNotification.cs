using System.Collections;
using UnityEngine;


public class AudioNotification : MonoBehaviour
{
    public AudioClip Sound;
    private AudioSource audioNotification;

    private void Awake() => audioNotification = GetComponent<AudioSource>();

    private void Start() => audioNotification.PlayOneShot(Sound);

    private void Update()
    {
        if (audioNotification.isPlaying) return;

        Destroy(gameObject);
    }

    public static AudioSource PlayAudioAt(Vector3 pos, AudioClip sound, float volume = 1.0f)
    {
        var obj = new GameObject("AudioNotification");
        obj.transform.position = pos;

        //  add audioNotification source
        AudioSource audioNotification = obj.AddComponent<AudioSource>();
        audioNotification.volume = volume;

        //  add audioNotification notification
        obj.AddComponent<AudioNotification>().Sound = sound;

        return audioNotification;
    }

    public static AudioSource PlayAudioAt(Vector3 pos, AudioClip[] sounds, float volume = 1.0f) =>
         PlayAudioAt(pos, sounds[Random.Range(0, sounds.Length)], volume);
}