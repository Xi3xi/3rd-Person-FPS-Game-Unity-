using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    private Queue<string> sentences;
    
    
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("start conversation with " + dialogue.name);
        sentences.Clear();
        nameText.text = dialogue.name;
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplaySentence();
    }

    public void DisplaySentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        Debug.Log("trigger");
        string s = sentences.Dequeue();
        // dialogueText.text = s;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(s));
        Debug.Log(s);
    }

    IEnumerator TypeSentence(String sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        Debug.Log("conversation end!");
    }
}
