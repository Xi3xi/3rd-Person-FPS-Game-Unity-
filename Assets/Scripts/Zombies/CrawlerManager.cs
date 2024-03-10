using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerManager : EnemyManager
{
    private bool aware = false;
    protected override void moveTowardsPlayer(float distance)
    {
        if (!aware && distance >= status.attackingRadius)
        {
            agent.speed = 0;
            return;
        }
        
        if (distance < status.awareRadius || aware)
        {
            aware = true;
            agent.speed = 2;
            agent.SetDestination(target.transform.position);
            transform.LookAt(target.transform);
            zombieAnimator.SetFloat("speed", 0.3f, 0.3f, Time.deltaTime);
        }

        if (distance < status.attackingRadius)
        {
            // zombieAnimator.SetFloat("speed", 0.3f, 0.3f, Time.deltaTime);
            if (Time.time >= lastAttacking + status.attackSpeed)
            {
                lastAttacking = Time.time;
                Debug.Log("Player was hit by crawl!");
                attackPlayer();
            }
        }

    }
    
}
