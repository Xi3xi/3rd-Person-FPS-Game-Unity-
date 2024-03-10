using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thrower : EnemyManager
{
    public GameObject weaponPrefab;
    public Transform throwPoint;
    protected override void attackPlayer()
    {
        zombieAnimator.SetTrigger("attack");
    }

    void getNewWeapon()
    {
        Instantiate(weaponPrefab);
    }

    void throwWeapon()
    {
        Debug.Log("attack by throw!");
        // Vector3 direct =  (player.transform.position - transform.position).normalized;
        GameObject weapon = Instantiate(weaponPrefab, throwPoint.position, Quaternion.identity);
        // Debug.Log(transform.position);
        ThrowableWeapon weaponItem = weapon.GetComponent<ThrowableWeapon>();
        weaponItem.throwIt(player.transform);
        
        // GameObject weapon =Instantiate(weaponPrefab, new Vector3(0,0,0), Quaternion.identity);
        // weapon.GetComponent<ThrowableWeapon>().parent = weaponPrefab.GetComponent<ThrowableWeapon>().parent;
        // weaponPrefab.GetComponent<ThrowableWeapon>().enemy = GetComponent<EnemyManager>();
    }
}
