using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class RobotManager : MonoBehaviour
{
    [SerializeField] private float robotSpeed;
    [SerializeField] private int robotDamage;
    [SerializeField] private int robotRange;

    public MyWeapon currentWeapon;

    private GameObject projectilePrefab=null;
    public Transform throwPoint;

    private GameObject player;

    private GameObject enemy;

    
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        // auto attack enemy
        if (enemy == null)
        {
            return;
        }
        if (Vector3.Distance(transform.position, enemy.transform.position) < robotRange)
        {
            transform.LookAt(enemy.transform);

            // shoot once every 2 second
            if (Time.frameCount % 60 == 0){
                Debug.Log("shoot");
                Shoot();
            }
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, robotRange))
        {
            EnemyManager enemyManager = hit.transform.GetComponent<EnemyManager>();
            if (!enemyManager.IsUnityNull())
            {
                Instantiate(Resources.Load("Blood"), hit.point,hit.transform.rotation,hit.transform);
                enemyManager.Hit(robotDamage);
 
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


}
