using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveKeySkillsManager : MonoBehaviour
{
    public InventoryItemData inventoryAEDDataRef;
    public InventoryItemData inventoryMicroscopeDataRef;
    public InventoryItemData inventoryShieldDataRef;
    // Start is called before the first frame update
    
    public InventoryItemData GetAEDItem()
    {
        return inventoryAEDDataRef;
    }
    public InventoryItemData GetMicroscopeItem()
    {
        return inventoryMicroscopeDataRef;
    }
    public InventoryItemData GetShieldItem()
    {
        return inventoryShieldDataRef;
    }
}
