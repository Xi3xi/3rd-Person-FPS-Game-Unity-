using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
[Serializable]
public class MyProjectile : MonoBehaviour
{
    public MyWeaponData data;
    public int currentAmmo;
    public GameObject fog;
    
    public void Attack()
    {
        if (currentAmmo-data.consumeRound <= 0)
        {
            currentAmmo = 0;
            Debug.Log("Projectile weapon needs to reload");
        }else 
        {
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
        //generate a field on the landing position
        Instantiate(fog, transform.position, transform.rotation);
        //destroy projectile when hit anything
        Destroy(this.gameObject,1f);
        
        
    }
}
