using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowerManager : EnemyManager
{
    protected override void attackPlayer()
    {
        zombieAnimator.SetTrigger("attack");
    }
}
