using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceSkill{
    public class VoiceSkillManager : MonoBehaviour
{
    public delegate void VoiceSkillEventHandler();
    public static event VoiceSkillEventHandler OnVoiceSkillUsed;

    public InventoryItemData inventoryMicrophoneDataRef;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public InventoryItemData GetKeyItem()
    {
        return inventoryMicrophoneDataRef;
    }

    // Call this method to trigger the event
    public void UseVoiceSkill()
    {
        // Trigger the event
        OnVoiceSkillUsed?.Invoke();
    }
}
}



