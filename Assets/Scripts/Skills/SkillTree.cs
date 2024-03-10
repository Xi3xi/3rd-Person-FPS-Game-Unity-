using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class SkillTree : MonoBehaviour
{
    private Skills skills;

    private int skillCost = 50;
    private void Awake()
    {
        // Biomedicine and Chemistry
        if (GameController.faculty == 0)
        {
            transform.Find("IHavePPE").GetComponent<Button_UI>().ClickFunc = () => {
                UnlockSkill(Skills.SkillType.IHavePPE);
            };
            transform.Find("IHaveABandAid").GetComponent<Button_UI>().ClickFunc = () => {
                UnlockSkill(Skills.SkillType.IHaveABandAid);
            };
            transform.Find("CPRStat").GetComponent<Button_UI>().ClickFunc = () => {
                UnlockSkill(Skills.SkillType.CPRStat);
            };
            transform.Find("LargerApparatus").GetComponent<Button_UI>().ClickFunc = () => {
                UnlockSkill(Skills.SkillType.LargerApparatus);
            };
            transform.Find("ClassIsToxic").GetComponent<Button_UI>().ClickFunc = () => {
                UnlockSkill(Skills.SkillType.ClassIsToxic);
            };
            transform.Find("IStudyMicrobiology").GetComponent<Button_UI>().ClickFunc = () => {
                UnlockSkill(Skills.SkillType.IStudyMicrobiology);
            };
        }

        // Engineering and IT
        if (GameController.faculty == 1){
            Debug.Log("EngIT");
            transform.Find("ItsATrap").GetComponent<Button_UI>().ClickFunc = () => {
                UnlockSkill(Skills.SkillType.ItsATrap);
                Debug.Log("ItsATrap");
            };
            transform.Find("ILikeDIY").GetComponent<Button_UI>().ClickFunc = () => {
                UnlockSkill(Skills.SkillType.ILikeDIY);
                Debug.Log("ILikeDIY");
            };
            transform.Find("Terminator").GetComponent<Button_UI>().ClickFunc = () => {
                UnlockSkill(Skills.SkillType.Terminator);
            };
            transform.Find("LetsAskGoogleMap").GetComponent<Button_UI>().ClickFunc = () => {
                UnlockSkill(Skills.SkillType.LetsAskGoogleMap);
            };
            transform.Find("Hacker").GetComponent<Button_UI>().ClickFunc = () => {
                UnlockSkill(Skills.SkillType.Hacker);
            };
            transform.Find("CaptainAmerica").GetComponent<Button_UI>().ClickFunc = () => {
                UnlockSkill(Skills.SkillType.CaptainAmerica);
            };
        }
        

        // Arts and Music
        if (GameController.faculty == 2) {
            Debug.Log("ArtsMusic");
            transform.Find("Imagination").GetComponent<Button_UI>().ClickFunc = () => {
                UnlockSkill(Skills.SkillType.Imagination);
            };
            transform.Find("HarryPoEr").GetComponent<Button_UI>().ClickFunc = () => {
                UnlockSkill(Skills.SkillType.HarryPoEr);
            };
            transform.Find("ListenToMyVoice").GetComponent<Button_UI>().ClickFunc = () => {
                UnlockSkill(Skills.SkillType.ListenToMyVoice);
            };
            transform.Find("LetsDance").GetComponent<Button_UI>().ClickFunc = () => {
                UnlockSkill(Skills.SkillType.LetsDance);
            };
            transform.Find("Soundproof").GetComponent<Button_UI>().ClickFunc = () => {
                UnlockSkill(Skills.SkillType.Soundproof);
            };
            transform.Find("GonnaPaintMyself").GetComponent<Button_UI>().ClickFunc = () => {
                UnlockSkill(Skills.SkillType.GonnaPaintMyself);
            };
        }
        
    }

    // popup message saying skill is unlock
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
