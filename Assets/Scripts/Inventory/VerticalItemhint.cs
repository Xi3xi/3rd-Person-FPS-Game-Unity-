using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalItemhint : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target,Vector3.up);    
    }
}
