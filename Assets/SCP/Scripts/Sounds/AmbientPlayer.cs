using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientPlayer : MonoBehaviour
{
    public AudioClip[] AmbientSounds;
    public Vector2 RangeCooldown;

    private float nextSoundCooldown = 0.0f;
    private AudioSource ambientPlayerAudio;

    private void Awake() => ambientPlayerAudio = GetComponent<AudioSource>();

    private void Update()
    {
        if (AmbientSounds.Length == 0) return;

        if (ambientPlayerAudio.isPlaying) return;

        if ((nextSoundCooldown -= Time.deltaTime) <= 0.0f)
            NextSound();
    }

    public void NextSound()
    {
        ambientPlayerAudio.clip = Utils.GetRandomElement(AmbientSounds);
        ambientPlayerAudio.Play();

        nextSoundCooldown = Random.Range(RangeCooldown.x, RangeCooldown.y);
    }
}