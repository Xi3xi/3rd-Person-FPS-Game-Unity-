using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSkillManager : MonoBehaviour
{
    public GameObject robotPrefabs;

    public GameObject robot;

    public InventoryItemData inventoryRobotDataRef;

    public void ActivateRobot()
    {
        robot.SetActive(true);
    }

    public InventoryItemData GetKeyItem()
    {
        return inventoryRobotDataRef;
    }
}