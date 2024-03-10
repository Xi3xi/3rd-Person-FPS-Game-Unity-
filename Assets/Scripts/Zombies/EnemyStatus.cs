using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    [SerializeField] public float damage;
    [SerializeField] public float speed;
    [SerializeField] public float health;
    [SerializeField] public float awareRadius;
    [SerializeField] public float attackingRadius;
    [SerializeField] public float attackSpeed;
    
    void Start()
    {
        initialize();
    }

    protected virtual void initialize()
    {
        damage = 10f;
        health = 100f;
        awareRadius = 60f;
        attackingRadius = 2.5f;
        attackSpeed = 3;
        speed = 2;

    }
    
    public float getHurt(float damageFromPlayer)
    {
        health -= damageFromPlayer;
        return health;
    }

    public void doDamage(PlayerManager player)
    {
        player.Hit(damage);
    }

    public void friendlyFire(EnemyManager enemy)
    {
        enemy.Hit(damage);
    }
}
