using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSkillManager : MonoBehaviour
{
    private int nextEmptySlot = 0;

    private int currentActiveSkill = 0;

    private List<EquipSkillSlot> equipSkillSlots;

    public void AddSkill(Skills.SkillType skill) {
        
        Debug.Log(equipSkillSlots.Count);
        // add skill to the next empty slot
        equipSkillSlots[nextEmptySlot].AddSkill(skill);
    
        // update next empty slot
        nextEmptySlot++;
    }

    public void DeactivateSkill() {

        // remove skill from the list
        equipSkillSlots[currentActiveSkill].DeactivateSkill();

    }

    public void UseSkill() {

        if(nextEmptySlot == 0){
            SkillMessage.Instance.ShowMessage("No Skill Learnt!");
        }
        // check if the skill is active
        else if (equipSkillSlots[currentActiveSkill].checkIfSkillActive()) {
            // use the skill
            equipSkillSlots[currentActiveSkill].UseSkill();
            SkillMessage.Instance.ShowMessage("Skill " + (currentActiveSkill + 1) + " Used!");
        } else {
            SkillMessage.Instance.ShowMessage("Skill " + (currentActiveSkill + 1) + " Not Available!");
        }

    }

    void Awake()
    {
        equipSkillSlots = new List<EquipSkillSlot>();

        GameObject[] equipskillSlot = GameObject.FindGameObjectsWithTag("SkillSlot");

        foreach (GameObject slot in equipskillSlot) {
            equipSkillSlots.Add(slot.GetComponent<EquipSkillSlot>());
        }
    }

    public void ChangeActiveSkill(int index) {
        currentActiveSkill = index;
        SkillMessage.Instance.ShowMessage("Skill " + (currentActiveSkill + 1) + " Selected!");
    }

    public void Reset(){
        
    }
}
