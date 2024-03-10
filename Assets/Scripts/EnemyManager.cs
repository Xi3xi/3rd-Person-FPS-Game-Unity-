using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using PaintSkill;
using static PaintSkill.PaintSkillManager;
using static VoiceSkill.VoiceSkillManager;
using static HackSkill.HackSkillManager;
using Unity.VisualScripting;

public class EnemyManager : MonoBehaviour
{
    public GameObject player;

    public GameObject target;
    public GameManager gameManager;

    protected EnemyStatus status = null;
    protected Animator zombieAnimator;
    protected NavMeshAgent agent;
    
    private bool isDead;
    // public bool isPickUp;
    private int destroyClock;

    protected float lastAttacking;
    
    // private GameObject target;
    // public float damage = 20f;
    // public float attackingRadius = 5f;

    public bool isPoisoned = false;
    public float poisonDuration = 10;
    public float poisonTimeGap = 2;
    public float poisonDamage = 10;
    private float startTime = 0;
    private float lastHealthDeductionTime = 0;

    public float poisonResistance = 1;
    // final poisoned time= time*poisionRecoverRate,
    // increase poisonRecoverRate to cause less damage
    public float poisonRecoverRate = 1;

    private int studyPointsAmount = 20;
    private PlayerManager playerManager;

    public bool playerInvisible = false;
    public bool isDistracted = false;
 
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        target = player;
        zombieAnimator = GetComponent<Animator>();
        status = GetComponent<EnemyStatus>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        isDead = false;
        // transform.localScale *= 1.5f;
        // GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().sharedMesh;
        
        lastAttacking = 0;
 
        playerManager = player.GetComponent<PlayerManager>();
        OnPaintSkillUsed += new PaintSkillEventHandler(StopAttack);
        OnVoiceSkillUsed += new VoiceSkillEventHandler(Hypnotised);
        OnHackSkillUsed += new HackSkillEventHandler(Distract);
    }

    protected virtual void Update()
    {
        if (isDead)
        {
            if (destroyClock == 0)
            {
                StudyPointManager.Instance.AddStudyPoint(this.studyPointsAmount);
                Destroy(gameObject);
                gameManager.enemyAlive--;
            }
            else destroyClock--;
            return;
        }

        if (!playerInvisible)
        {
            GetComponent<NavMeshAgent>().destination = target.transform.position;
            float distance = Vector3.Distance(transform.position, target.transform.position);
            moveTowardsPlayer(distance);
        }

        if (isPoisoned)
        {
            if (startTime + poisonDuration > Time.time)
            {
                if (Time.time - lastHealthDeductionTime > poisonTimeGap * poisonRecoverRate )
                {
                    lastHealthDeductionTime = Time.time;
                    Hit(poisonDamage * poisonResistance);
                }
            }
            else {
                isPoisoned = false;
            }
        }


        //float distance = Vector3.Distance(transform.position, player.transform.position);
        //moveTowardsPlayer(distance);
        
    }


    protected virtual void moveTowardsPlayer(float distance)
    {
        // if (isPickUp) return;
        
        if (distance <= status.awareRadius)
        {
            agent.SetDestination(target.transform.position);
            
            /* Need to modify the height of player. */
            // transform.LookAt(player.transform);                // Rotating to face to the player.
            Vector3 dir = target.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(new Vector3(dir.x,0,dir.z));
            transform.rotation = rotation;

            if (distance >= status.attackingRadius)
            {
                // Debug.Log("Aware of player");
                zombieAnimator.SetFloat("speed", 0.3f, 0.3f, Time.deltaTime);
                agent.speed = status.speed;
                // zombieAnimator.SetBool("Aware", true);
            }
            else
            {
                zombieAnimator.SetFloat("speed", 0f);
                agent.speed = 0;
                // Debug.Log("stop at" + distance);
                // Attack with an attacking speed.
                if (Time.time >= lastAttacking + status.attackSpeed)
                {
                    lastAttacking = Time.time;
                    attackPlayer();
                }
                
            }

        }
    }

    protected virtual void attackPlayer()
    {
        zombieAnimator.SetTrigger("attack");

        if (target == player){
            status.doDamage(target.GetComponent<PlayerManager>());

            // reflect damage
            if (playerManager.withIStudyMicrobiology()){
                Hit(status.damage / 2);
            }
        }
        else {
            EnemyManager enemy = target.GetComponent<EnemyManager>();
            if (enemy != null){
                status.friendlyFire(enemy);
            }
        }
    }
    
    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (collision != null && collision.gameObject == player)
    //     {
    //         Debug.Log("Player was hit!");
    //         status.doDamage(player.GetComponent<PlayerManager>());
    //     }
    //
    // }
    public void damageEnemy()
    {
        agent.speed = 0;
        zombieAnimator.SetFloat("speed", 0f);
        isDead = true;
        destroyClock = 300;
    }
    
    public virtual void Hit(float damage)
    {
        //GameObject blood = Instantiate(Resources.Load("Blood", typeof(GameObject)), gameObject.transform) as GameObject;

        if(isDead) return;
    
        float health = status.getHurt(damage);
        if (health <= 0.0)
        {
            zombieAnimator.SetTrigger("death");
            Invoke("damageEnemy",1);
        }
        else
        {
            zombieAnimator.SetTrigger("damage");
        }
    }

    public void pickUpWeapon(Transform weapon)
    {
        // isPickUp = true;
        agent.SetDestination(weapon.position);
        transform.LookAt(player.transform);
        // zombieAnimator.SetFloat("speed", 0.3f, 0.3f, Time.deltaTime);
    }

    public void Poisoned(float damage, float timeGap, float poisonDuration)
    {
        this.poisonDamage = damage;
        this.poisonTimeGap = timeGap;
        this.poisonDuration = poisonDuration;
        this.isPoisoned = true;
        float startTime = Time.time;
        float lastHealthDeductionTime = startTime;
    }

    // make enemy attack each other for 10 seconds, find nearest enemy to attack
    public void Hypnotised(){
        int searchRadius = 50;
        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius);
        float closestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Enemy") && collider.gameObject != gameObject)
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = collider.gameObject;
                }
            }
        }

        if (closestEnemy != null){
            target = closestEnemy;
            status.damage /= 2;
            Invoke("StopHypnotised", 10f);
        }
        else {
            StopAttack();
        }
        OnVoiceSkillUsed -= new VoiceSkillEventHandler(Hypnotised);
    }

    // stop attacking player for 15 seconds
    public void StopAttack(){
        playerInvisible = true;
        Invoke("ResumeAttack", 15f);
        OnPaintSkillUsed -= new PaintSkillManager.PaintSkillEventHandler(StopAttack);
    }

    public void ResumeAttack(){
        playerInvisible = false;
    }

    public void StopHypnotised(){
        target = player;
        status.damage *= 2;
    }

    void OnDestroy()
    {
        OnPaintSkillUsed -= new PaintSkillManager.PaintSkillEventHandler(StopAttack);
        OnVoiceSkillUsed -= new VoiceSkillEventHandler(Hypnotised);
        OnHackSkillUsed -= new HackSkillEventHandler(Distract);
    }

    public void Distract(){
        isDistracted = true;
        target = GameObject.FindGameObjectWithTag("Distraction");
        Invoke("StopDistract", 10f);
    }

    public void StopDistract(){
        isDistracted = false;
        target = player;
    }
}
