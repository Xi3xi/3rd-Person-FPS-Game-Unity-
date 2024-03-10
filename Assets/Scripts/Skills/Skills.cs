using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PaintSkill;
using VoiceSkill;
using HackSkill;

public class Skills: MonoBehaviour
{
    public event EventHandler<OnSkillUnlockedEventArgs> OnSkillUnlocked;
    public class OnSkillUnlockedEventArgs : EventArgs{
        public SkillType skillType;
    }
    public enum SkillType{
        None,
        IHavePPE,
        IHaveABandAid,
        CPRStat,
        LargerApparatus,
        ClassIsToxic,
        IStudyMicrobiology,

        ItsATrap,
        ILikeDIY,
        Terminator,
        LetsAskGoogleMap,
        Hacker,
        CaptainAmerica,
        Imagination,
        HarryPoEr,
        ListenToMyVoice,
        LetsDance,
        Soundproof,
        GonnaPaintMyself,
    }

    public enum SkillKeyItemType{
        No,
        Yes,
    }

    public enum ActiveSkill{
        None,
        ItsATrap,
        Hacker,
        Terminator,
        GonnaPaintMyself,
        ListenToMyVoice,
        Imagination,
    }

    private PassiveKeySkillsManager passiveKeySkillsManager;
    private RobotSkillManager robotSkillManager;

    private PaintSkillManager paintSkillManager;
    private VoiceSkillManager voiceSkillManager;
    private TrapSkillManager trapSkillManager;
    private HackSkillManager hackSkillManager;

    private List<SkillType> usedSkillsTypes;

    private List<SkillType> unlockedSkillTypes;

    private List<SkillType> breadthSkillTypes;

    public Skills()
    {
        unlockedSkillTypes = new List<SkillType>();
        usedSkillsTypes = new List<SkillType>();
        breadthSkillTypes = new List<SkillType>();
        passiveKeySkillsManager = GameObject.Find("SkillController").GetComponent<PassiveKeySkillsManager>();
        robotSkillManager = GameObject.Find("SkillController").GetComponent<RobotSkillManager>();
        paintSkillManager = GameObject.Find("SkillController").GetComponent<PaintSkillManager>();
        voiceSkillManager = GameObject.Find("SkillController").GetComponent<VoiceSkillManager>();
        trapSkillManager = GameObject.Find("SkillController").GetComponent<TrapSkillManager>();
        hackSkillManager = GameObject.Find("SkillController").GetComponent<HackSkillManager>();
    }

    private void UnlockSkill(SkillType skillType)
    {
        if (!IsSkillUnlocked(skillType)){
            unlockedSkillTypes.Add(skillType);
            OnSkillUnlocked?.Invoke(this, new OnSkillUnlockedEventArgs {skillType = skillType});
        }
         
    }

    public bool IsSkillUnlocked(SkillType skillType)
    {
        return unlockedSkillTypes.Contains(skillType);
    }

    public SkillType GetSkillsRequirement(SkillType skillType){

        if (breadthSkillTypes.Contains(skillType)){
            return SkillType.None;
        }

        switch (skillType) {
            // Biomed
            case SkillType.IHavePPE:
                return SkillType.IHaveABandAid;
            case SkillType.IHaveABandAid:
                return SkillType.None;
            case SkillType.CPRStat:
                return SkillType.IHavePPE;
            case SkillType.LargerApparatus:
                return SkillType.None;
            case SkillType.ClassIsToxic:
                return SkillType.LargerApparatus;
            case SkillType.IStudyMicrobiology:
                return SkillType.ClassIsToxic;

            // Engineering and IT
            case SkillType.ItsATrap:
                return SkillType.None;
            case SkillType.ILikeDIY:
                return SkillType.ItsATrap;
            case SkillType.Terminator:
                return SkillType.ILikeDIY;
            case SkillType.LetsAskGoogleMap:
                return SkillType.None;
            case SkillType.Hacker:
                return SkillType.LetsAskGoogleMap;
            case SkillType.CaptainAmerica:
                return SkillType.Hacker;
            
            // Arts and Music
            case SkillType.Imagination:
                return SkillType.None;
            case SkillType.HarryPoEr:
                return SkillType.Imagination;
            case SkillType.ListenToMyVoice:
                return SkillType.HarryPoEr;
            case SkillType.LetsDance:
                return SkillType.None;
            case SkillType.Soundproof:
                return SkillType.LetsDance;
            case SkillType.GonnaPaintMyself:
                return SkillType.Soundproof;
            default:
                return SkillType.None;
        }
    }

    public InventoryItemData GetSkillsKeyItemsRequirement(SkillType skillType){
        switch (skillType) {
            case SkillType.Terminator:
                return robotSkillManager.GetKeyItem();
            case SkillType.CPRStat:
                return passiveKeySkillsManager.GetAEDItem();
            case SkillType.IStudyMicrobiology:
                return passiveKeySkillsManager.GetMicroscopeItem();
            case SkillType.CaptainAmerica:
                return passiveKeySkillsManager.GetShieldItem();
            case SkillType.ListenToMyVoice:
                return voiceSkillManager.GetKeyItem();
            case SkillType.GonnaPaintMyself:
                return paintSkillManager.GetKeyItem();
            default:
                return null;
        }
    }

    public bool TryUnlockSkill(SkillType skillType){

        SkillType skillRequirement = GetSkillsRequirement(skillType);
        InventoryItemData skillKeyItemRequirement = GetSkillsKeyItemsRequirement(skillType);

        if(skillRequirement == SkillType.None){
            if(!IsSkillUnlocked(skillType)){
                if (skillKeyItemRequirement != null){
                    if (!InventorySystem.current.checkItemFromInventory(skillKeyItemRequirement)){
                        Tooltip_Warning.ShowTooltip_Static("Key Item Missing!");
                        return false;
                    }
                    else{
                        InventorySystem.current.Remove(skillKeyItemRequirement);
                    }
                }
                UnlockSkill(skillType);
                Tooltip_Warning.ShowTooltip_Static("Skill Unlocked!");
                return true;
            }
            else{
                Tooltip_Warning.ShowTooltip_Static("Already Unlocked!");
                return false;
            }  
        } 
        else{
            if (IsSkillUnlocked(skillRequirement)){
                if(!IsSkillUnlocked(skillType)){
                    if (skillKeyItemRequirement != null){
                        if (!InventorySystem.current.checkItemFromInventory(skillKeyItemRequirement)){
                            Tooltip_Warning.ShowTooltip_Static("Key Item Missing!");
                            return false;
                        }
                        else{
                            InventorySystem.current.Remove(skillKeyItemRequirement);
                        }
                    }
                    UnlockSkill(skillType);
                    Tooltip_Warning.ShowTooltip_Static("Skill Unlocked!");
                    return true;
                }
                else{
                    Tooltip_Warning.ShowTooltip_Static("Already Unlocked!");
                    return false;
                }  
            }
            else{
                Tooltip_Warning.ShowTooltip_Static("Cannot Unlock!");
                return false;
            }
        }
    }

    public void UseSkill(SkillType skillType){

        // if already used
        if(usedSkillsTypes.Contains(skillType)){
            return;
        }

        switch (skillType) {
            case SkillType.Terminator:
                robotSkillManager.ActivateRobot();
                break;
            case SkillType.GonnaPaintMyself:
                paintSkillManager.UsePaintSkill();
                break;
            case SkillType.ListenToMyVoice:
                voiceSkillManager.UseVoiceSkill();
                break;
            case SkillType.Imagination:
                trapSkillManager.UseTrapSkill();
                break;
            case SkillType.ItsATrap:
                trapSkillManager.UseTrapSkill();
                break;
            case SkillType.Hacker:
                hackSkillManager.UseHackSkill();
                break;
            default:
                return;
        } 
        usedSkillsTypes.Add(skillType);
    }

    public bool IsActiveSkills(SkillType skillType){
        switch (skillType) {
            case SkillType.Terminator:
                return true;
            case SkillType.ItsATrap:
                return true;
            case SkillType.Hacker:
                return true;
            case SkillType.GonnaPaintMyself:
                return true;
            case SkillType.ListenToMyVoice:
                return true;
            case SkillType.Imagination:
                return true;
            default:
                return false;
        }
    }

    public void Reset(){
        usedSkillsTypes.Clear();
    }
}
