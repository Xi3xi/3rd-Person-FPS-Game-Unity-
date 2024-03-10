using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowStatus : EnemyStatus
{
    protected override void initialize()
    {
        // damage = 20f;
        health = 100f;
        speed = 2;
        awareRadius = 100f;
        attackingRadius = 10f;
        attackSpeed = 8;
    }
}
