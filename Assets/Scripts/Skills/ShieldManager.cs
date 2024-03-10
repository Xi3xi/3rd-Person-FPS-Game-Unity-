using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShieldManager : MonoBehaviour
{
    public GameObject shieldBar;
    public Image shieldImage;

    private PlayerManager playerManager;

    public TMP_Text shieldText;
    private float maxShield = 0;
    private float shield = 0;
    private int shieldRegen = 1;
    // Start is called before the first frame update

    void Awake()
    {
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        shieldImage = shieldBar.transform.Find("Shield").GetComponent<Image>();
    }


    // Update is called once per frame
    void Update()
    { 
        if (!playerManager.isShielded)
        {
            return;
        }
        if(Time.frameCount % 240 == 0){
            ShieldRegen();
            UpdateShieldBar();
        }
    }

    // regen shield every second
    public void ShieldRegen()
    {
        if (shield < maxShield)
        {
            shield += shieldRegen;
        }
    }

    public void SetShield(int maxShield)
    {
        if(maxShield > this.maxShield)
        {
            this.maxShield = maxShield;
        }
        this.shield = maxShield;
        shieldBar.SetActive(true);
        UpdateShieldBar();
    }

    public void SetRegen(int regen)
    {
        this.shieldRegen = regen;
    }

    public float UpdateShield(float damage)
    {
        if (damage > shield)
            {
                damage -= shield;
                shield = 0;
            }
            else
            {
                shield -= damage;
                damage = 0;
            }
            UpdateShieldBar();
            return damage;
    }

    private void UpdateShieldBar()
    {
        shieldImage.fillAmount = shield / maxShield;
        shieldText.text = "Shield: " + shield.ToString("0") + "/" + maxShield.ToString("0");
    }
}
