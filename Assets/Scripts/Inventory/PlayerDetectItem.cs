using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDetectItem : MonoBehaviour
{

    public GameObject playerCam;
    public float range = 100;
    public bool useLookAt;
    public GameObject _lookAtTarget;
    public Transform _targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        playerCam = GameObject.FindGameObjectWithTag("playerCam");
    }

    void OutlineItem(GameObject objectItem)
    {
        if (!OutlineManager.current.outlinedObject.IsUnityNull())
        {
            OutlineManager.current.notOutlineAnyObject();
            OutlineManager.current.removeAllOutlinedObjects();
        }
        OutlineManager.current.addOutlinedObject(objectItem);
        OutlineManager.current.onOutlinedObjectsChangedEvent
            +=OutlineManager.current.OutlineAllObjects;
        OutlineManager.current.OutlinedObjectsChangedEvent();
    }
    GameObject lastHit=null;
    void Update()
    {
        
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, 
                out var objectItem, range))
        {
            if (lastHit != objectItem.transform.gameObject)
            {
                lastHit = objectItem.transform.gameObject;
                if (objectItem.transform.gameObject.GetComponent<ItemObject>() != null)
                    OutlineItem(objectItem.transform.gameObject);
            }

            // Debug.Log("interaction hit something");
                if (Input.GetButtonDown("Fire2"))
                {
                    Debug.Log("press Fire2");
                    Interact(objectItem);
                }
                
            
            
        }
        else
        {
            // Debug.Log("hit failed");
        }
    }

    void Interact(RaycastHit objectItem)
    {
        // if a item is detected
        if (objectItem.transform.TryGetComponent<ItemObject>(out ItemObject item))
        {
            Debug.Log("pick up item");
            item.OnHandlePickupItem();
        }
    }

    private void OnDrawGizmos()
    {
        if (playerCam == null) return;
        RaycastHit hitInfo;
        Gizmos.color=Color.magenta;
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hitInfo, range))
        {
            Gizmos.DrawSphere(hitInfo.point,0.1f);
        }
    }
}
