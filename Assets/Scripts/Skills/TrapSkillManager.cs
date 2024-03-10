using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSkillManager : MonoBehaviour
{
    public GameObject trapPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // deploy a trap underneath the player
    public void UseTrapSkill(){
        var pos = transform.position;
        pos.y = 0.1f;
        Instantiate(trapPrefab, pos, Quaternion.identity);
    }
}
