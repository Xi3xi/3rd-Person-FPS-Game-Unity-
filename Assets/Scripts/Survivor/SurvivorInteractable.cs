using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorInteractable : MonoBehaviour
{
    private GameObject chatWindow;
    private NPCInteractionManager playerInteract;
    private interactUI ui;
    
    private Animator _animator;

    private void Awake()
    {
        playerInteract = GameObject.Find("Player").GetComponent<NPCInteractionManager>();
        chatWindow = GameObject.Find("InGameCanvas").transform.Find("ChatPanel").gameObject;
        ui =GameObject.Find("InGameCanvas").transform.Find("playerInteract").GetComponent<interactUI>();
    }
    
    private void Start()
    {
        hideChatWindow();
        ui.enabled = false;
        ui.hide();
        playerInteract = GameObject.FindGameObjectWithTag("Player").GetComponent<NPCInteractionManager>();
    }

    private void AwakeNPC()
    {
        _animator = GetComponent<Animator>();
        // transform.LookAt(playerInteract.transform);
        _animator.SetTrigger("Talk");
    }

    private void showChatWindow()
    {
        chatWindow.SetActive(true);
    }
    private void hideChatWindow()
    {
        chatWindow.SetActive(false);
    }
    public void Interact()
    {
        ui.hide();
        AwakeNPC();
        showChatWindow();
        Debug.Log("Interact!");
    }

    private void Update()
    {
        if (playerInteract.GetInteractableNPC() == null)
        {
            hideChatWindow();
            ui.hide();
        }
        else
        {
            ui.enabled = true;
            ui.show();
        }
    }
}
