using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager current;
    public GameObject playerCam;
    public float damage = 0;
    public float range = 100;
    public InventoryItemData inventoryAmmoDataRef;
    private MyWeapon currentWeapon;
    public Action onWeaponStatusChangedEvent;
    private GameObject projectilePrefab=null;
    public Transform throwPoint;
    //public int coolDownTime = 2;
    private float lastAttackedAt = -9999f;
    
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
        currentWeapon = GameObject.FindGameObjectWithTag("WeaponHolder").transform.GetChild(0).GetComponent<MyWeapon>();
        damage = currentWeapon.data.damage;
        WeaponStatusChangedEvent();
    }
    
    void Fire1()
    {
        if (GameController.Instance.GetGameIsPaused()){
            return;
        }
        if (currentWeapon.currentAmmo > 0 )
        {
            if (Time.time > lastAttackedAt + currentWeapon.data.coolDownTime) {
                lastAttackedAt = Time.time;

                    if(!currentWeapon.data.isProjectile)
                        Shoot();
                    else
                        Throw();
                    //consume ammo
                    currentWeapon.Attack();
                    WeaponStatusChangedEvent();
            }
        }else
        {
            currentWeapon.GetComponent<AudioSource>().clip = currentWeapon.data.clockSound;
            currentWeapon.GetComponent<AudioSource>().Play();
        }
    }

    private void FixedUpdate()
    {
        if (currentWeapon.data.causeContinuousDamage&&Input.GetButton("Fire1"))
        {
            Fire1();
        }
    }

    void Update()
    {
        if(LevelController.GameIsPaused)
        {
            return;
        }
        if (!currentWeapon.data.causeContinuousDamage&&Input.GetButtonDown("Fire1"))
        {
            Fire1();
        }
        
        if (Input.GetButtonDown("Reload"))
        {
            // only allow reload when current ammo<maximum ammo
            if(currentWeapon.currentAmmo<currentWeapon.data.maxRound) 
            {
                int reloadRound = currentWeapon.data.reloadRound;
                //check ammo, if ammo>ammo need for 1 reload, reload maximum
                if (InventorySystem.current.checkItemFromInventory(inventoryAmmoDataRef))
                {
                    InventorySystem.current.Remove(inventoryAmmoDataRef);
                    int reloadRemain= currentWeapon.Reload(reloadRound);
                    //InventorySystem.current.Add(inventoryAmmoDataRef);
                }
            }
            WeaponStatusChangedEvent();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if ( Physics.Raycast(playerCam.transform.position, transform.forward, out hit, range))
        {
            EnemyManager enemyManager = hit.transform.GetComponent<EnemyManager>();
            if (!enemyManager.IsUnityNull())
            {
                Instantiate(Resources.Load("Blood"), hit.point,hit.transform.rotation,hit.transform);
                enemyManager.Hit(damage);
            }

        }
        if (currentWeapon.data.hasBullet)
        {
            GameObject bullet=Instantiate(currentWeapon.data.generate, currentWeapon.transform.position,
                currentWeapon.transform.rotation);
            bullet.transform.Rotate(0f, 0f, -90f);
            
            bullet.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0,0,currentWeapon.data.launchVelocity));
         
            GameObject shell = Instantiate(currentWeapon.data.generate,
                currentWeapon.transform.position + new Vector3(0.1f, 0, 0.1f),
                currentWeapon.transform.rotation);
            shell.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(-10,2,0));
        }
    }
    void Throw()
    {
        projectilePrefab = currentWeapon.data.prefab;
        GameObject throwProjectile=Instantiate(projectilePrefab,throwPoint.position, throwPoint.rotation);
        throwProjectile.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0,0,currentWeapon.data.launchVelocity));
 
    }
    public void addDamage(float damage)
    {
        this.damage += damage;
    }
    
    public void addRange(float range)
    {
        this.range += range;
    }

    public void poison()
    {
        currentWeapon.data.isPoisonous= true;
    }
    
    public void addCapacity(int capacity)
    {
        currentWeapon.data.maxRound += capacity;
    }

    public void ReloadMax()
    {
        currentWeapon.currentAmmo = currentWeapon.data.maxRound;
    }
}
