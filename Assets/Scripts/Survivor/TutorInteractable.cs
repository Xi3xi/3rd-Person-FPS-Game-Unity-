using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorInteractable : MonoBehaviour
{
    [SerializeField]private GameObject chatWindow;
    private GameObject player;
    public Dialogue dialogue;
    private Boolean started;
    private DialogManager dialogManager;
    
    private Animator _animator;

    private void Awake()
    {
        player = GameObject.Find("Player");
        dialogManager = FindObjectOfType<DialogManager>();
        // chatWindow = GameObject.Find("dialogue");
        // ui =GameObject.Find("InGameCanvas").transform.Find("playerInteract").GetComponent<interactUI>();
    }
    
    private void Start()
    {
        hideChatWindow();
        player = GameObject.Find("Player");
        dialogManager = FindObjectOfType<DialogManager>();
        started = false;
        // chatWindow = GameObject.Find("dialogue");
        // ui.enabled = false;
        // ui.hide();
        // playerInteract = GameObject.FindGameObjectWithTag("Player").GetComponent<NPCInteractionManager>();
    }

    private void AwakeNPC()
    {
        _animator = GetComponent<Animator>();
        // transform.LookAt(playerInteract.transform);
        _animator.SetTrigger("Talk");
    }
    
    public void TriggerDialogue()
    {
        if (!started)
        {
            dialogManager.StartDialogue(dialogue);
            started = true;
        }
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
        // ui.hide();
        AwakeNPC();
        showChatWindow();
        Debug.Log("Interact!");
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > 3)
        {
            hideChatWindow();
            // ui.hide();
        }
        else
        {
            showChatWindow();
            TriggerDialogue();
            if (Input.GetKeyDown(KeyCode.E))
            {
                dialogManager.DisplaySentence();
            }
            // ui.enabled = true;
            // ui.show();
        }
        
        
    }
}
