using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class BossStatus : MonoBehaviour
{
    [SerializeField] public float shortDamage;
    [SerializeField] public float summonDamage;
    [SerializeField] public float weakDamage;
    [SerializeField] public float health;
    [SerializeField] public float weakBar;
    [SerializeField] public float speed;
    [SerializeField] public float strongAttackingRadius;
    [SerializeField] public float weakAttackingRadius;
    [SerializeField] public float longAttackSpeed;
    [SerializeField] public float shortAttackSpeed;
    [SerializeField] public float weakAttackSpeed;
    [SerializeField] public float avoidSpeed;

    public UnityEngine.Vector3 originScale;
    private float MAX_HEALTH = 1000f;
    
    void Start()
    {
        initialize();
    }

    protected virtual void initialize()
    {
        shortDamage = 10f;
        weakDamage = 20f;
        summonDamage = 100f;
        health = 1000f;
        weakBar = 400f;
        speed = 2;
        strongAttackingRadius = 2f;
        weakAttackingRadius = 4f;
        longAttackSpeed = 6;
        shortAttackSpeed = 2;
        weakAttackSpeed = 1;
        avoidSpeed = 10;
        
        originScale = transform.localScale;
    }

    public void modifyScale()
    {
        float scaleRatio = (float) health / MAX_HEALTH;
        transform.localScale = originScale * ( 1 + scaleRatio);
    }
    
    public float getHurt(float damageFromPlayer)
    {
        health -= damageFromPlayer;
        return health;
    }

    public void summonHurt()
    {
        health -= summonDamage;
    }

    public void doShortDamage(PlayerManager player)
    {
        player.Hit(shortDamage);
    }
    
    public void doweakDamage(PlayerManager player)
    {
        player.Hit(weakDamage);
    }
}
