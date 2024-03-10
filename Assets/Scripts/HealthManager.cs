using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthManager : MonoBehaviour
{
    public GameObject healthBar;
    public Image healthImage;

    public TMP_Text healthText;
    private float maxHealth = 100f;
    private float health = 100;

    private int healthRegen = 0;

    // Start is called before the first frame update
    void Awake()
    {
        healthBar = GameObject.Find("InGameCanvas").transform.Find("HealthBar").gameObject;
        healthImage = healthBar.transform.Find("Health").GetComponent<Image>();
        healthText = healthBar.transform.Find("HealthText").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    { 
        // health regen
        if(Time.frameCount % 60 == 0){
            HealthRegen();
            UpdateHealthBar();
        }

    }

    // regen shield every second
    public void HealthRegen()
    {
        if (health < maxHealth)
        {
            health += healthRegen;
        }
    }

    public void SetHealth(int maxHealth)
    {
        if(maxHealth > this.maxHealth)
        {
            this.maxHealth = maxHealth;
        }
        this.health = maxHealth;
    }

    public void AddHealth(int health)
    {
        this.health += health;
        if (this.health > maxHealth)
        {
            this.health = maxHealth;
        }
        UpdateHealthBar();
    }

    public void SetRegen(int regen)
    {
        this.healthRegen = regen;
    }

    public void UpdateHealth(float damage)
    {
        health -= damage;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthImage.fillAmount = health / maxHealth;
        healthText.text = "Health: " +health.ToString("0") + "/" + maxHealth.ToString("0");
    }

    public float GetHealth()
    {
        return health;
    }

    public void Damage(float damage)
    {
        health -= damage;
        UpdateHealthBar();
    }
}
