using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactUI : MonoBehaviour
{
    [SerializeField] private GameObject container;
    // [SerializeField] private NPCInteractionManager playerInteract;

    public void show()
    {
        container.SetActive(true);
    }

    public void hide()
    {
        container.SetActive(false);
    }
    
    // private void Update()
    // {
    //     if (playerInteract.GetInteractableNPC() != null)
    //     {
    //         show();
    //     }else hide();
    // }
}
