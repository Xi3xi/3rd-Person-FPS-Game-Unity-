using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private MyWeapon currentWeapon;

    private float initTime;
    public AudioSource audioSource;
    private void Awake()
    {
        currentWeapon = GameObject.FindGameObjectWithTag("WeaponHolder").
            transform.GetChild(0).GetComponent<MyWeapon>();
        //initTime = Time.time;
        Destroy(this.gameObject,4);
        audioSource=GetComponent<AudioSource>();
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // other.gameObject.GetComponent<EnemyManager>()
                // .Hit(currentWeapon.data.damage);
        }
        
        audioSource.Play();
        // Destroy(this.gameObject);
    }
}
