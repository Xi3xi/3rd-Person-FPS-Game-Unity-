using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogBlockDamage : MonoBehaviour
{
    public Boolean canPoisonPlayer = false;
    public float damage = 10;
    private void OnCollisionEnter(Collision other)
    {
        if (canPoisonPlayer == true)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().Poisoned(damage,2,10);
                // other.GetComponent<PlayerManager>().Poisoned(data.damage,2,10);
            }
        }
    }
    
}
