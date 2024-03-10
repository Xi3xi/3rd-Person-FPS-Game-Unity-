using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeralManager : EnemyManager
{
    protected override void moveTowardsPlayer(float distance)
    {
        if (distance <= status.awareRadius)
        {
            agent.SetDestination(target.transform.position);
            
            /* Need to modify the height of player. */
            transform.LookAt(target.transform);                // Rotating to face to the player.
            
            zombieAnimator.SetFloat("speed", 0.3f, 0.3f, Time.deltaTime);
            zombieAnimator.SetBool("attack", false);
            agent.speed = status.speed;

            if (distance < status.attackingRadius)
            {
                zombieAnimator.SetFloat("speed", 0f);
                agent.speed = 0;
                
                // Attack with an attacking speed.
                if (Time.time >= lastAttacking + status.attackSpeed)
                {
                    Debug.Log(lastAttacking + status.attackSpeed);
                    lastAttacking = Time.time;
                    
                    // Do attack 
                    attackPlayer();
                }
            }

        }
    }
}
