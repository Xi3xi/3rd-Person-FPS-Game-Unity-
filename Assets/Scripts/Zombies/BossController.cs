using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossController : EnemyManager
{
    // public GameObject player;
    protected BossStatus bossStatus;
    protected Animator bossAnimator;
    // protected NavMeshAgent agent;

    // private int avoidMode;
    private bool isWeak;
    private bool bossIsDead;
    protected float lastAvoiding;
    
    public GameObject weaponPrefab;
    public Transform throwPoint;

    private string zombieCrawler = "Crawler";
    private string zombieFeral = "Feral";
    private string zombieSpitter = "Spitter";
    private string zombieUsual = "usualZombie";
    private bool forceFieldEnabled=true;

    protected override void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        bossAnimator = GetComponent<Animator>();
        bossStatus = GetComponent<BossStatus>();

        bossIsDead = false;
        isWeak = false;
        lastAttacking = 0;
        lastAvoiding = 5;
        agent.speed = 0;

        // transform.localScale = bossStatus.originScale * 2;
        bossAnimator.SetInteger("avoid", 5);

        
    }

    protected override void Update()
    {
        if (Time.time < 3 || bossIsDead) return;
        awarePlayer();
        
        agent.SetDestination(player.transform.position);
        // agent.speed = 3;
        
        Vector3 dir = player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(new Vector3(dir.x,0,dir.z), Vector3.up);
        transform.rotation = rotation;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        // Debug.Log("distance is    " + distance);
        
        if (bossStatus.health <= bossStatus.weakBar)
        {
            isWeak = true;
            if (forceFieldEnabled==true)
                this.transform.GetChild(0).gameObject.SetActive(false);
            bossAnimator.SetBool("weak", true);
        }
        if (!isWeak)
        {
            attackPlayerStrong(distance);
        }
        else attackPlayerWeak(distance);
    }

    private void awarePlayer()
    {
        bossAnimator.SetBool("aware", true);
    }

    private void attackPlayerStrong(float distance)
    {
        // Long distance attack in mode one
        if (distance > bossStatus.strongAttackingRadius)
        {
            agent.speed = 3;
            if (Time.time >= lastAttacking + bossStatus.longAttackSpeed)
            {
                Debug.Log("Long distance attack");
                lastAttacking = Time.time;
                int r = Random.Range(0, 2);
                if(r == 1){
                    bossAnimator.SetTrigger("longAttack"); // Do attack animation;
                }
                else SummonAttack();
            }
        }
        else
        {
            // Do the short distance attack
            agent.speed = 0;
            if (Time.time >= lastAttacking + bossStatus.shortAttackSpeed)
            {
                Debug.Log("short distance attack");
                lastAttacking = Time.time;
                bossAnimator.SetTrigger("shortAttack");
                bossStatus.doShortDamage(player.GetComponent<PlayerManager>());
            }
            
        }
        // Randomly avoid player's attack.
        if(Time.time >= lastAvoiding + bossStatus.avoidSpeed)
        {
            lastAvoiding = Time.time;
            // Invoke("avoidPlayer", 5);
            avoidPlayer();
            
            // Generate a random avoid action.
            int seed = Random.Range(0, 4);
            Debug.Log("avoid " + seed);
            bossAnimator.SetInteger("avoid", seed);
            bossAnimator.SetInteger("avoid", 5);
        }
    }

    private void avoidPlayer()
    { 
        // Generate a random moving direction.
        float maxAngle = 360f;
        float randomAngle = Random.Range(0f, maxAngle);
        float radians = randomAngle * Mathf.Deg2Rad;
        Vector3 randomDirection = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f);
        agent.SetDestination(randomDirection);
        agent.speed = 4;
        Debug.Log("avoid to " + randomDirection);
        
    }
    
    private void attackPlayerWeak(float distance)
    {
        if (distance <= bossStatus.weakAttackingRadius)
        {
            if (Time.time >= lastAttacking + bossStatus.weakAttackSpeed)
            {
                Debug.Log("weak attack");
                lastAttacking = Time.time;
                bossAnimator.SetTrigger("attack"); // Do attack animation;
                bossStatus.doweakDamage(player.GetComponent<PlayerManager>());
            }
        }
    }

    private void SummonAttack()
    {
        agent.speed = 0;
        SummonRandomZombie();
    }
    private void generateRandomZombie(){
            int rand = Random.Range(1, 4);
            Vector3 summonPoint = this.transform.position + new Vector3(3, 0, 5);
            switch (rand)
            {
                case 1:
                    Instantiate(Resources.Load(zombieCrawler), summonPoint, transform.rotation);
                    break;
                case 2:
                    Instantiate(Resources.Load(zombieFeral), summonPoint, transform.rotation);
                    break;
                case 3:
                    Instantiate(Resources.Load(zombieSpitter), summonPoint, transform.rotation);
                    break;
                case 4:
                    Instantiate(Resources.Load(zombieUsual), summonPoint, transform.rotation);
                    break;
            }
        }

        private void SummonRandomZombie()
    {
        bossAnimator.SetTrigger("summon");
        Invoke("generateRandomZombie",1);
        bossStatus.summonHurt();
        // bossStatus.modifyScale();
    }

    public override void Hit(float damage)
    {
        if (bossIsDead) return;
        if (isWeak)
        {
            float health = bossStatus.getHurt(damage);
            if (health <= 0.0)
            {
                bossAnimator.SetTrigger("death");
                agent.speed = 0;
                bossIsDead = true;
            }
            else
            {
                bossAnimator.SetTrigger("damage");
            }
        }
    }
    
    void throwWeapon()
    {
        Debug.Log("enemy throw!");
        GameObject weapon = Instantiate(weaponPrefab, throwPoint.position, Quaternion.identity);
        ThrowableWeapon weaponItem = weapon.GetComponent<ThrowableWeapon>();
        weaponItem.throwIt(player.transform);
    }
}
