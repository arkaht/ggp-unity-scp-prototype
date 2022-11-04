using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    public AudioClip[] Sounds;
    public bool SingleUse = true;
    public Vector3 PositionRange;

    private AudioSource targetAudio;
    private Collider collider;

    private void Awake()
    {
        targetAudio = GetComponent<AudioSource>();
        collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        targetAudio.PlayOneShot(Utils.GetRandomElement(Sounds));

        if (SingleUse) collider.enabled = false;

        if (PositionRange == Vector3.zero) return;

        TranslatePlayPosition();
    }

    private void TranslatePlayPosition()
    {
        float angle = Mathf.Deg2Rad * Random.Range(0.0f, 360.0f);
        transform.position += new Vector3(Mathf.Cos(angle) * PositionRange.x, Mathf.Sin(angle) * PositionRange.y, Mathf.Tan(angle) * PositionRange.z);
    }
}