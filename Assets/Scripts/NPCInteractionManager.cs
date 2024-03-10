using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractionManager : MonoBehaviour
{
    private float interactRange = 2f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SurvivorInteractable npc = GetInteractableNPC();
            if ( npc != null)
            {
                Debug.Log("Interact Success!");
                npc.Interact();
            }
        }
    }

    public SurvivorInteractable GetInteractableNPC()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out SurvivorInteractable npc))
            {
                return npc;
            }
        }

        return null;
    }
}
