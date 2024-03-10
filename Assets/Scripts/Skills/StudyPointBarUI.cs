using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StudyPointBarUI : MonoBehaviour
{
    
    [SerializeField] private TMP_Text text; // Text mesh pro component.
    [SerializeField] private string prefix;

    [SerializeField] private string postfix;

    [SerializeField] private int defaultValue;


    public int studyPoint;

    private void Awake()
    {
        this.studyPoint = defaultValue;
        UpdateText(this.studyPoint);
        StudyPointManager.Instance.OnStudyPointChange += SetValue;
    }

    public void SetValue(int value)
    {
        this.studyPoint += value;
        UpdateText(this.studyPoint);
    }

    private void UpdateText(int displayValue)
    {
        this.text.SetText(this.prefix + displayValue + postfix);
    }

}
