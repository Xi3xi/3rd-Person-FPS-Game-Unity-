using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpawnBoss : MonoBehaviour
{
    public GameObject portal;
    public Transform spawnPoint;
    public GameObject boss;
    // Start is called before the first frame update
    void Start()
    {
        boss = Resources.Load("Boss").GameObject();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        AddBoss();
        AddPortal();
    }
    public void AddBoss()
    {
        Instantiate(boss, spawnPoint.transform.position, spawnPoint.transform.rotation);
    }
    public void AddPortal()
    {
        Instantiate(portal, spawnPoint.transform.position + new Vector3(0,-2,0), spawnPoint.transform.rotation);
    }
}
