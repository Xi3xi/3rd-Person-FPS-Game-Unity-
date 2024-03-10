using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hidetext : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("HideText",5);
    }

    private void HideText()
    {
        this.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
