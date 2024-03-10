using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowableWeapon : MonoBehaviour
{
    // public GameObject parent;
    // public EnemyManager enemy;
    public int damage;
    public float attackingRadius;
    
    private bool isThrown;
    private Transform thrower;
    // private int clock;

    private Vector3 directionFrom;
    private Vector3 directionTo;
    
    // Start is called before the first frame update
    void Start()
    {
        // transform.parent = parent.transform;
        isThrown = false;
        // clock = 0;
    }

    private void Awake()
    {
        Destroy(this,5);
    }

    // Update is called once per frame
    void Update()
    {
        // if (!isThrown)
        // {
        //     rb.useGravity = false;
        //     cd.enabled = false;
        //     // transform.parent = parent.transform;
        //     // transform.position = parent.transform.position;
        // }
        // else
        // {
        //     clock--;
        // }
        // else if (!isHold)
        // {
        //     Debug.Log("pick up weapon!!!!!!!!");
        //     rb.useGravity = true;
        //     cd.enabled = true;
        //     toThrow = false;
        //     
        //     enemy.pickUpWeapon(transform);
        //     float dis = Vector3.Distance(transform.position, enemy.transform.position);
        //     if (dis <= 2)
        //     {
        //         enemy.isPickUp = false;
        //     }
        // }
    }

    public void throwIt(Transform target)
    {
        Debug.Log("Throw!!");
        // transform.parent = null;
        // rb.useGravity = true;
        // cd.enabled = true;
        // transform.rotation = parent.transform.rotation;

        isThrown = true;
        thrower = target;
        Vector3 throwDirection = target.position - transform.position + new Vector3(0,3,0);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddRelativeForce(throwDirection * 1000);
        // clock = 100;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        // Debug.Log("Colide!!!!!");
        GameObject player = GameObject.FindWithTag("Player");
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player was hit by a thrown brick!");
            player.GetComponent<PlayerManager>().Hit(damage);
        }
        else
        {
            Vector3 contactPosition = collision.contacts[0].point;
            float dis =  Vector3.Distance(contactPosition, thrower.transform.position);
            if (dis <= attackingRadius)
            {
                Debug.Log("Player was in the brick's hitting range!");
                player.GetComponent<PlayerManager>().Hit(damage);
            }
            else
            {
                Debug.Log("Missing" + dis);
            }
        }
        Destroy(gameObject);
    }
}
