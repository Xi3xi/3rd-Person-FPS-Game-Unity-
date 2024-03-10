using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipSkillSlot : MonoBehaviour
{
    // update sprite of the slot

    private Skills.SkillType skill = Skills.SkillType.None;

    private PlayerManager playerManager;
    
    public GameObject icon;

    public GameObject background;
    
    public void AddIcon() {
        // add image to the icon
        Sprite sprite = Resources.Load<Sprite>("Skills/" + skill.ToString());

        icon.GetComponent<Image>().sprite = sprite;
    }

    public void AddSkill(Skills.SkillType skill) {
        // add skill to the list
        this.skill = skill;
        AddIcon();
        // update UI
        UpdateUI();
    }

    public void DeactivateSkill() {
        // remove skill from the list
        this.skill = Skills.SkillType.None;
    }

    public bool checkIfSkillActive() {
        // check if the skill is active
        return skill != Skills.SkillType.None;
    }

    private void UpdateUI() {
        // show sprite
        icon.SetActive(true);
        background.SetActive(false);
    }

    void Awake()
    {
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
    }

    public void UseSkill(){
        playerManager.GetSkills().UseSkill(skill);
    }

}
