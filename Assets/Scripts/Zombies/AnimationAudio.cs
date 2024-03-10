using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AnimationAudio : MonoBehaviour
{
    public AudioClip attackSound;
    // public float pitch;
    private AudioSource soundSource;
    void Start()
    {
        soundSource = GetComponent<AudioSource>();
        // soundSource.pitch = pitch;
    }

    public void attack()
    {
        // soundSource.pitch = pitch;
        soundSource.clip = attackSound;
        soundSource.PlayOneShot(soundSource.clip);
    }
    
}
