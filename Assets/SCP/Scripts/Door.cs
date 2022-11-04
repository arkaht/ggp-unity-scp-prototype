using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public AudioClip[] OpenSounds, CloseSounds;

    private bool isOpen = false;
    private AudioSource doorAudio;
    private Animator animator;

    private void Awake()
    {
        doorAudio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    public void SetToggle(bool is_open)
    {
        isOpen = is_open;

        //  play animation
        animator.SetTrigger(is_open ? "Open" : "Close");

        //  play trigger sound
        doorAudio.PlayOneShot(isOpen ? Utils.GetRandomElement(OpenSounds) : Utils.GetRandomElement(CloseSounds));
    }

    public void Toggle() => SetToggle(!isOpen);
}