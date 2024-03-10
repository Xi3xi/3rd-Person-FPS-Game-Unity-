using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSurvivor : MonoBehaviour
{
    private float interactRange = 2f;

    // Make sure that the NPC has Capsule Collider
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliders)
            {
                Debug.Log(collider);
                if (collider.TryGetComponent(out SurvivorInteractable npc))
                {
                    npc.Interact();
                }
            }
        }
    }

}
