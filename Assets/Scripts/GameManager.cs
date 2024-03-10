using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int enemyAlive = 0;
    public int level = 0;
    public GameObject[] spawners;
    public GameObject[] enemyPrefab;

    private int zombieMaxNum = 10;
    
    // void Update()
    // {
    //     if (enemyAlive == 0)
    //     {
    //         level++;
    //         nextEnemyWave(level);
    //     }
    //
    // }

    private void nextEnemyWave()
    {
        int num = Random.Range(5, zombieMaxNum);
        for (var x = 0; x <= num; x++)
        {
            int p = Random.Range(0, spawners.Length);
            int seed = Random.Range(0, 4);
            // if(level > 4) seed = Random.Range(0, 5);
            // else seed = Random.Range(0, level + 1);
            
            GameObject enemy = Instantiate(enemyPrefab[seed], spawners[p].transform.position, Quaternion.identity);
            enemy.GetComponent<EnemyManager>().gameManager = GetComponent<GameManager>();
            enemyAlive++;
        }
    }

    public void checkUpdate()
    {
        if (enemyAlive == 0)
        {
            // level++;
            nextEnemyWave();
        }
    }
    
}
