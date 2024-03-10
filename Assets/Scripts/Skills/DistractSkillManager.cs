using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractSkillManager : MonoBehaviour
{
    public float detectionRadius = 10f;

    private GameObject player;

    private GameObject selectedObject;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void UseSkill()
    {
        // Get all objects within detection radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        // Filter out objects that are not tagged as "Distractible"
        List<GameObject> distractibleObjects = new List<GameObject>();
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Distractible"))
            {
                distractibleObjects.Add(collider.gameObject);
            }
        }

        // Randomly select an object from the list of distractible objects
        if (distractibleObjects.Count > 0)
        {
            int randomIndex = Random.Range(0, distractibleObjects.Count);
            selectedObject = distractibleObjects[randomIndex];

            // Tag the selected object as "Player"
            selectedObject.tag = "Player";
        }
        player.tag = "Untagged";

        Invoke("EndDistraction", 10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void EndDistraction(){
        player.tag = "Player";
        selectedObject.tag = "Untagged";
    }
}