using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FogDamage : MonoBehaviour
{
    public MyWeaponData data;

    public Boolean canPoisonPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.transform.parent.gameObject,data.consumeRound);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyManager>().Hit(data.damage);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (canPoisonPlayer == true)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().Poisoned(data.damage,2,10);
                // other.GetComponent<PlayerManager>().Poisoned(data.damage,2,10);
            }
        }
    }
}
