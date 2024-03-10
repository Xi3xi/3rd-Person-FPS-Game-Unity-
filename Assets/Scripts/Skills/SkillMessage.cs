using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillMessage : MonoBehaviour
{
    public static SkillMessage Instance { get; private set; }

    public GameObject skillMessage;

    public TMP_Text text;

    private void Awake()
    {
        Instance = this;
        skillMessage.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        skillMessage.SetActive(true);
        text.SetText(message);
        StartCoroutine(HideMessage());
    }

    private IEnumerator HideMessage()
    {
        yield return new WaitForSeconds(2f);
        skillMessage.SetActive(false);
    }
}
