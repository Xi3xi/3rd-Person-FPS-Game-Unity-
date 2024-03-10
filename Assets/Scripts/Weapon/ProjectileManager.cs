using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager current;
    public GameObject playerCam;
    private float damage = 0;
    //public float range = 100;
    public float launchVelocity = 500;
    public InventoryItemData inventoryAmmoDataRef;
    public GameObject projectilePrefab;
    public MyProjectile CurrentProjectile;
    public Transform throwPoint;

    public int coolDownTime = 2;
    private float lastAttackedAt = -9999f;
    public Action onWeaponStatusChangedEvent;

    public void WeaponStatusChangedEvent()
    {
        if (onWeaponStatusChangedEvent != null)
        {
            onWeaponStatusChangedEvent();
        }
    }
    private void Awake()
    {
        current = this;
    }

    void Start()
    {        
        playerCam = GameObject.FindGameObjectWithTag("playerCam");
        damage = CurrentProjectile.data.damage;
        WeaponStatusChangedEvent();
    }
    void Update()
    {
        if(LevelController.GameIsPaused)
        {
            return;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            if (Time.time > lastAttackedAt + coolDownTime) {
                lastAttackedAt = Time.time;
                Debug.Log("press Fire1");
                if (CurrentProjectile.currentAmmo > 0)
                {
                    Throw();
                    //consume ammo
                    CurrentProjectile.Attack();
                    WeaponStatusChangedEvent();
                }
            }

        }
        
        if (Input.GetButtonDown("Reload"))
        {
            // only allow reload when current ammo<maximum ammo
            if(CurrentProjectile.currentAmmo<CurrentProjectile.data.maxRound) 
            {
                int reloadRound = CurrentProjectile.data.reloadRound;
                //check ammo, if ammo>ammo need for 1 reload, reload maximum
                if (InventorySystem.current.checkItemFromInventory(inventoryAmmoDataRef))
                {
                    InventorySystem.current.Remove(inventoryAmmoDataRef);
                    int reloadRemain= CurrentProjectile.Reload(reloadRound);
                    //InventorySystem.current.Add(inventoryAmmoDataRef);
                }
                else
                {
                    Debug.Log("projectile run out of ammo!");
                }
            }
            else
            {
                Debug.Log("projectile is already full");
            }

            WeaponStatusChangedEvent();
        }
    }

    void Throw()
    {
        GameObject throwProjectile=Instantiate(projectilePrefab,throwPoint.position, throwPoint.rotation);
        throwProjectile.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0,0,launchVelocity));
        
    }

    public void addDamage(float damage)
    {
        this.damage += damage;
    }
    
    
}
