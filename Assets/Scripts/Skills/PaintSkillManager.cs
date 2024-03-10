using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaintSkill{
    public class PaintSkillManager : MonoBehaviour
{
    public delegate void PaintSkillEventHandler();
    public static event PaintSkillEventHandler OnPaintSkillUsed;

    public InventoryItemData inventoryPaletteDataRef;
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
        return inventoryPaletteDataRef;
    }

    // Call this method to trigger the event
    public void UsePaintSkill()
    {
        // Trigger the event
        OnPaintSkillUsed?.Invoke();
    }
}
}


