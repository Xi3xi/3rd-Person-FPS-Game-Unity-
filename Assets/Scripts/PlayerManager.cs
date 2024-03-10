using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class PlayerManager : MonoBehaviour
{
    [Header("Damage Overlay")]
    public Image damageOverlay;

    public float duration;

    public float fadeSpeed;

    public float durationTime;
    
    private float health = 100;
    private float shield = 0;
    private float maxShield = 100;
    public bool isShielded = false;
    private bool betterMap = false;
    public bool isPoisoned = false;
    public float poisonDuration = 10;
    public float poisonTimeGap = 2;
    public float poisonDamage = 10;
    private float startTime = 0;
    private float lastHealthDeductionTime = 0;

    public InventoryItemData inventoryHealthDataRef;
    
    private int resistance = 1;

    private int life = 1;
    // final poison damage = poisionResistance*poision damage
    // reduce poisonResistance to cause less damage
    public float poisonResistance = 1;
    // final poisoned time= time*poisionRecoverRate,
    // increase poisonRecoverRate to cause less damage
    public float poisonRecoverRate = 1;
    private SkillMenuManager skillMenuManager;

    private GameObject skillMenu;

    private WeaponManager weaponManager;
    private Skills skills;
    private HealthManager healthManager;
    private ShieldManager shieldManager;

    private EquipSkillManager equipSkillManager;

    private bool canUseIHavePPE = false;
    private bool canUseIHaveABandAid = false;
    private bool canUseIStudyMicrobiology = false;

    private GameController gameController;
    
   
    // Start is called before the first frame update
    private void Start()
    {
        damageOverlay=GameObject.FindWithTag("DamageOverlayer").GetComponent<Image>();

        damageOverlay.color = new Color(damageOverlay.color.r,
            damageOverlay.color.g, damageOverlay.color.b, 0);
    }

    private void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        healthManager = GetComponent<HealthManager>();
        shieldManager = GetComponent<ShieldManager>();
        skillMenuManager = GetComponent<SkillMenuManager>();
        equipSkillManager = GameObject.FindGameObjectWithTag("SkillCanvas").transform.Find("SkillMenu").GetComponent<EquipSkillManager>();
        weaponManager = GameObject.Find("CameraHolder").transform.Find("PlayerCam").GetComponent<WeaponManager>();
        skills = new Skills();
        skills.OnSkillUnlocked += Skills_OnSkillUnlocked;

        healthManager.SetHealth(100);
    }

    private void FixedUpdate()
    {
        if (isPoisoned)
        {
            if (startTime + poisonDuration > Time.time)
            {
                if (Time.time - lastHealthDeductionTime > poisonTimeGap * poisonRecoverRate )
                {
                    lastHealthDeductionTime = Time.time;
                    health -= poisonDamage * poisonResistance;
                }
            }
            else {
                isPoisoned = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(LevelController.GameIsPaused)
        {
            return;
        }
        // activate skill
        if (Input.GetKeyDown(KeyCode.Q))
        {
            equipSkillManager.UseSkill();
        }

        // use health pack to heal
        if (Input.GetKeyDown(KeyCode.H))
        {
            if(healthManager.GetHealth()<100)
            {
                if (InventorySystem.current.checkItemFromInventory(inventoryHealthDataRef))
                {
                    InventorySystem.current.Remove(inventoryHealthDataRef);
                    healthManager.AddHealth(20);
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            equipSkillManager.ChangeActiveSkill(0);
        } 
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            equipSkillManager.ChangeActiveSkill(1);
        } 
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            equipSkillManager.ChangeActiveSkill(2);
        } 
        else if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            equipSkillManager.ChangeActiveSkill(3);
        }

        if (isShielded && Time.frameCount % 240 == 0)
        {
            shieldManager.ShieldRegen();
        }
        damageEffect();
    }

    private void damageEffect()
    {
        //for damage effect
        if(damageOverlay.color.a > 0)
        {
            durationTime += Time.deltaTime;
            if (durationTime > duration)
            {
                //fade the Image
                float tempAlpha = damageOverlay.color.a;
                tempAlpha-=Time.deltaTime*fadeSpeed;
                damageOverlay.color = new Color(damageOverlay.color.r,
                    damageOverlay.color.g,damageOverlay.color.b,tempAlpha);
            }
        }
    }
    
    public void Hit(float damage)
    {
        if (isShielded)
        {
            damage = shieldManager.UpdateShield(damage);
        }
        healthManager.Damage(damage/resistance);
        
        damageOverlay.color = new Color(damageOverlay.color.r,
            damageOverlay.color.g, damageOverlay.color.b, 1);
        
        if (healthManager.GetHealth() <= 0)
        {
            gameController.LevelEnd(false);
        }
    }

    public void Poisoned(float damage, float timeGap, float poisonDuration)
    {
        this.poisonDamage = damage;
        this.poisonTimeGap = timeGap;
        this.poisonDuration = poisonDuration;
        if (canUseIHavePPE){
            this.poisonDuration *= 0.5f;
        }
        this.isPoisoned = true;
        float startTime = Time.time;
        float lastHealthDeductionTime = startTime;
    }
    
    public bool withIStudyMicrobiology(){
        return canUseIStudyMicrobiology;
    }

    public bool canUseLetsAskGoogleMap(){
        return skills.IsSkillUnlocked(Skills.SkillType.LetsAskGoogleMap);
    }

    public bool canUseCaptainAmerica(){
        return skills.IsSkillUnlocked(Skills.SkillType.CaptainAmerica);
    }

    public bool canUseSoundproof(){ 
         return skills.IsSkillUnlocked(Skills.SkillType.Soundproof);
    }

    private void Skills_OnSkillUnlocked(object sender, Skills.OnSkillUnlockedEventArgs e)
    {
        equipSkillManager = GameObject.FindGameObjectWithTag("SkillCanvas").transform.Find("SkillMenu").GetComponent<EquipSkillManager>();
        switch (e.skillType) {
            case Skills.SkillType.IHavePPE:
                increaseResistance();
                canUseIHavePPE = true;
                break;
            case Skills.SkillType.CPRStat:
                life = 2;
                break;
            case Skills.SkillType.LargerApparatus:
                weaponManager.addCapacity(20);
                weaponManager.WeaponStatusChangedEvent();
                break;
            case Skills.SkillType.IHaveABandAid:
                healthManager.SetRegen(2);
                break;
            case Skills.SkillType.ClassIsToxic:
                weaponManager.addDamage(25);
                weaponManager.addRange(25);
                break;
            case Skills.SkillType.IStudyMicrobiology:
                canUseIStudyMicrobiology = true;
                break;
            case Skills.SkillType.ItsATrap:
                equipSkillManager.AddSkill(Skills.SkillType.ItsATrap);
                break;
            case Skills.SkillType.ILikeDIY:
                weaponManager.addDamage(25);
                weaponManager.addRange(25);
                break;
            case Skills.SkillType.Terminator:
                equipSkillManager.AddSkill(Skills.SkillType.Terminator);
                break;
            case Skills.SkillType.LetsAskGoogleMap:
                betterMap = true;
                gameController.EnableMapInfos();
                break;
            case Skills.SkillType.Hacker:
                equipSkillManager.AddSkill(Skills.SkillType.Hacker);
                break;
            case Skills.SkillType.CaptainAmerica:
                isShielded = true;
                shieldManager.SetShield(150);
                shieldManager.SetRegen(5);
                break;
            case Skills.SkillType.Imagination:
                equipSkillManager.AddSkill(Skills.SkillType.Imagination);
                break;
            case Skills.SkillType.HarryPoEr:
                weaponManager.addDamage(25);
                weaponManager.addRange(25);
                break;
            case Skills.SkillType.ListenToMyVoice:
                equipSkillManager.AddSkill(Skills.SkillType.ListenToMyVoice);
                break;
            case Skills.SkillType.LetsDance:
                var playerMovementController = GetComponent<PlayerMovementController>();
                playerMovementController.changeMoveSpeed(1.5f);
                break;
            case Skills.SkillType.Soundproof:
                isShielded = true;
                shieldManager.SetShield(50);
                shieldManager.SetRegen(1);
                break;
            case Skills.SkillType.GonnaPaintMyself:
                equipSkillManager.AddSkill(Skills.SkillType.GonnaPaintMyself);
                break;
            default:
                break;
        }
    }

    public Skills GetSkills()
    {
        return skills;
    }

    private void increaseResistance(){
        this.resistance = 2;
    }

    public void Heal(int health)
    {
        healthManager.AddHealth(health);
    }

    public void Reset(){
        Heal(100);

        if(isShielded){
            shieldManager.SetShield(200);
        }

        if(betterMap){
            gameController.EnableMapInfos();
        }

        skills.Reset();
        StudyPointManager.Instance.RemoveStudyPoint(1000);
        weaponManager.ReloadMax();
    }
}
