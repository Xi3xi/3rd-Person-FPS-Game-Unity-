using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Weapon Data")]
public class MyWeaponData : ScriptableObject
{
    public string id;
    public string displayName;
    public Sprite icon;
    public GameObject prefab;
    public float damage;
    public int maxRound;
    public int consumeRound;
    public int reloadRound=6;
    public bool causeContinuousDamage = false;
    public bool isProjectile = false;
    public bool hasBullet = false;
    public float coolDownTime = 0.5f;
    public float launchVelocity = 500;
    public GameObject generate;
    public bool isPoisonous = false;
    public AudioClip audio;
    public AudioClip clockSound;

}
