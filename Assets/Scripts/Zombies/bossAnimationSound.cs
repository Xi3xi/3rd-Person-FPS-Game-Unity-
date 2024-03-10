using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class bossAnimationSound : MonoBehaviour
{
    public AudioClip attackSound;
    public AudioClip buffSound;
    public AudioClip weakAttackSound;
    
    private AudioSource soundSource;
    void Start()
    {
        soundSource = GetComponent<AudioSource>();
    }

    public void strongAttack()
    {
        soundSource.clip = attackSound;
        soundSource.PlayOneShot(soundSource.clip);
    }
    
    public void buff()
    {
        soundSource.clip = buffSound;
        soundSource.PlayOneShot(soundSource.clip);
    }
    
    public void weakAttack()
    {
        soundSource.clip = weakAttackSound;
        soundSource.PlayOneShot(soundSource.clip);
    }
}
