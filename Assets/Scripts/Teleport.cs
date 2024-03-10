using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject teleportObject;
    public Transform destination;
    // Start is called before the first frame update
    void Start()
    {
        teleportObject = GameObject.FindGameObjectWithTag("Player");
    }
    void TeleportObject(){
        if (GameObject.Find("Boss(Clone)").GetComponent<BossStatus>().health > 0)
        {
            GameObject.FindGameObjectWithTag("SpawnBoss").transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (teleportObject != null)
        {
            Debug.Log("Player has win the game");
            GameController.Instance.LevelEnd(true);
            //teleportObject.transform.position = destination.position;
        }
        else
        {
            //Debug.Log("Error: teleportObject or destination is null");
        }
    }
    // Update is called once per frame
    void Update()
    {

    
        
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject==teleportObject)
            TeleportObject();
    }
    
}
