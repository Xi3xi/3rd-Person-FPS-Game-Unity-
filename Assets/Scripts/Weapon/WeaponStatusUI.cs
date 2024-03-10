using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class WeaponStatusUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoStatus;

    [SerializeField] private int remainingAmmo;
    [SerializeField] private int maxAmmo;
    // Start is called before the first frame update
    private void Start()
    {
        WeaponManager.current.onWeaponStatusChangedEvent += OnUpdateWeaponAmmo;
        OnUpdateWeaponAmmo();
    }

    void Awake()
    {
        ammoStatus = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    
    }

    // Update is called once per frame
    public void Set(MyWeapon weapon)
    {
        remainingAmmo = weapon.currentAmmo;
        maxAmmo = weapon.data.maxRound;
        ammoStatus.text = "Ammo\n" + remainingAmmo + "/" + maxAmmo;
    }

    private void OnUpdateWeaponAmmo()
    {
        MyWeapon myWeapon = GameObject.FindGameObjectWithTag("WeaponHolder").transform.GetChild(0).GetComponent<MyWeapon>();
        Set(myWeapon);
    }
    void Update()
    {
        
    }
}
