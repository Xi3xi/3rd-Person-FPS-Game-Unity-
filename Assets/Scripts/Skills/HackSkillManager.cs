using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackSkill{
     public class HackSkillManager : MonoBehaviour
{
    public delegate void HackSkillEventHandler();
    public static event HackSkillEventHandler OnHackSkillUsed;

    public InventoryItemData inventoryMicrophoneDataRef;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Call this method to trigger the event
    public void UseHackSkill()
    {
        // Trigger the event
        OnHackSkillUsed?.Invoke();
    }
}
}

