using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBarController : MonoBehaviour
{
    private InventoryUI inventoryUI;
    void Start()
    {
        InventorySystem.current.onInventoryChangedEvent += OnUpdateInventory;
    }

    // Update is called once per frame
    private void OnUpdateInventory()
    {
        inventoryUI.UpdateInventory();
    }

    void Awake()
    {
        inventoryUI = GameObject.FindGameObjectWithTag("InventoryUI").GetComponent<InventoryUI>();
    }
}
