using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDamageOverlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            other.gameObject.GetComponent<PlayerManager>().Hit(10);
        }
    }
}
