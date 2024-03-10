using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class BreadthManager : MonoBehaviour
{
    private Skills skills;
    private int skillCost = 50;
    // Start is called before the first frame update
    void Awake()
    {
        // Biomedicine and Chemistry
        if (GameController.faculty != 0){
            transform.Find("CPRStat").GetComponent<Button_UI>().ClickFunc = () => {
                UnlockSkill(Skills.SkillType.CPRStat);
            };
                transform.Find("IStudyMicrobiology").GetComponent<Button_UI>().ClickFunc = () => {
                UnlockSkill(Skills.SkillType.IStudyMicrobiology);
            };
        }
        
        // Engineering and IT
        if (GameController.faculty != 1){
            transform.Find("Terminator").GetComponent<Button_UI>().ClickFunc = () => {
                UnlockSkill(Skills.SkillType.Terminator);
            };
            transform.Find("CaptainAmerica").GetComponent<Button_UI>().ClickFunc = () => {
                UnlockSkill(Skills.SkillType.CaptainAmerica);
            };
        }
        
        // Arts and Music
        if (GameController.faculty != 2){
            transform.Find("ListenToMyVoice").GetComponent<Button_UI>().ClickFunc = () => {
                UnlockSkill(Skills.SkillType.ListenToMyVoice);
            };
            transform.Find("GonnaPaintMyself").GetComponent<Button_UI>().ClickFunc = () => {
                UnlockSkill(Skills.SkillType.GonnaPaintMyself);
            };
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UnlockSkill(Skills.SkillType skillType)
    {
        if (StudyPointManager.Instance.GetCurrentStudyPoints() >= skillCost)
        {
            if(skills.TryUnlockSkill(skillType))
            {
                StudyPointManager.Instance.RemoveStudyPoint(skillCost);
            }
        }
        else
        {
            Tooltip_Warning.ShowTooltip_Static("Not Enough Points!");
        }
    }

    public void SetSkills(Skills skills)
    {
        this.skills = skills;
    }
}
