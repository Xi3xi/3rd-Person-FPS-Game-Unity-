// reference to tutorial:
// https://www.youtube.com/watch?v=SGz3sbZkfkg
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem current;
    private Dictionary<InventoryItemData, InventoryItem> m_itemDictionary;
    public List<InventoryItem> inventory { get; private set; }

    private PlayerManager playerManager;

    private int itemStudyPoints = 10;
    private int itemHealth= 20;
    // Start is called before the first frame update
    public void Start()
    {
       
    }

    public event Action onInventoryChangedEvent;
    public void InventoryChangedEvent()
    {
        if (onInventoryChangedEvent != null)
        {
            onInventoryChangedEvent.Invoke();
        }
    }
    public Boolean checkItemFromInventory(InventoryItemData item)
    {
        return m_itemDictionary.TryGetValue(item, out InventoryItem itemObject);
    }
    public void Awake()
    {
        current = this;
        inventory = new List<InventoryItem>();
        m_itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
    }

    public void Add(InventoryItemData referenceData)
    {
        
        if (referenceData.id == "InventoryItem_Exp")
        {
            AddStudyPoints();
        }
        else{
            if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
            {
                value.AddToStack();
            }
            else
            {
                InventoryItem newItem = new InventoryItem(referenceData);
                inventory.Add(newItem);
                m_itemDictionary.Add(referenceData,newItem);
            }
            InventoryChangedEvent();
        }
    }

    public void Remove(InventoryItemData referenceData)
    {
        
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            value.RemoveFromStack();
            if (value.stackSize == 0)
            {
                inventory.Remove(value);
                m_itemDictionary.Remove(referenceData);
            }
        }
        InventoryChangedEvent();
    }

    public void AddStudyPoints()
    {
        StudyPointManager.Instance.AddStudyPoint(this.itemStudyPoints);
    }    
}
