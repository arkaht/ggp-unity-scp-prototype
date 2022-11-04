using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepPlayer : MonoBehaviour
{
    public AudioClip[] Sounds;
    public float PlayCooldown = 0.6f;

    private int soundID = 0;
    private float nextPlayCooldown = 0.0f;
    private AudioSource audio;

    private void Awake() => audio = GetComponent<AudioSource>();

    private void Update()
    {
        if ((nextPlayCooldown -= Time.deltaTime) > 0.0f) return;

        Play();
    }

    public void Play()
    {
        soundID = (soundID + Random.Range(1, 3)) % Sounds.Length;
        audio.PlayOneShot(Sounds[soundID]);

        nextPlayCooldown = PlayCooldown;
    }
}
