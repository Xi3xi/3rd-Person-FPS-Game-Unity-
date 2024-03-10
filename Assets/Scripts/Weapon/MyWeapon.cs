using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[Serializable]
public class MyWeapon : MonoBehaviour
{
    public MyWeaponData data;
    public int currentAmmo;
    public GameObject effectObject;
    private ParticleSystem shootParticles;
 
    private AudioSource audioSource;
    private void Update()
    {
        // if (data.isProjectile)
        // {
        //     //load projectile to weapon manager instantiate prefab
        //     Instantiate(data.prefab)
        // }
    }

    private void Awake()
    {
        effectObject = transform.GetChild(1).GetChild(0).gameObject;
        shootParticles = effectObject.GetComponent<ParticleSystem>();
    
        audioSource=GetComponent<AudioSource>();
        
    }

    public void Attack()
    {
        if (currentAmmo-data.consumeRound <= 0)
        {
            currentAmmo = 0;
            Debug.Log("Weapon needs to reload");
            audioSource.clip = data.clockSound;
            audioSource.Play();
            
        }else
        {
            if (data.hasBullet)
            {
                // Check if the particle system is not null before trying to stop and play it
                if (!shootParticles.IsUnityNull())
                {
                    // Stop and replay the particle system
                    shootParticles.Stop();
                    shootParticles.Play();
                    audioSource.clip = data.audio;
                    audioSource.Play();
                }
            }
            
            currentAmmo -= data.consumeRound;
        }
    }

    public int Reload(int reload)
    {
        if (currentAmmo + reload > data.maxRound)
        {
            int remains = currentAmmo + reload - data.maxRound;
            currentAmmo = data.maxRound;
            return remains;
        }
        else
        {
            currentAmmo += reload;
        }

        return 0;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (data.isProjectile&& data.generate!=null)
        {
            //generate a field on the landing position
            Instantiate(data.generate, transform.position, transform.rotation);
            //destroy projectile when hit anything
            Destroy(this.gameObject,1f);
        }

        
        
    }
}
