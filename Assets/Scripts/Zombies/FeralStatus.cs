using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeralStatus : EnemyStatus
{
    protected override void initialize()
    {
        damage = 5f;
        health = 100f;
        speed = 3;
        awareRadius = 40f;
        attackingRadius = 1.5f;
        attackSpeed = 2;
    }
}
