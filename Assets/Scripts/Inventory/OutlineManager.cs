using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OutlineManager : MonoBehaviour
{
    public static OutlineManager current;
    public Color outlineColor = Color.cyan;

    public float outlineWidth = 10f;

    public bool isOutlined = false;

    public Action onOutlinedObjectsChangedEvent;

    public GameObject outlinedObject;

    private GameObject itemTooltip;

    private GameObject currentToolTip;

    private void Awake()
    {
        current = this;
        itemTooltip = Resources.Load("ItemHint").GameObject();
    }
    
    public void OutlinedObjectsChangedEvent()
    {
        if (onOutlinedObjectsChangedEvent != null)
        {
            onOutlinedObjectsChangedEvent();
        }
        if(!currentToolTip.IsUnityNull())
            Destroy(currentToolTip);
        
        currentToolTip = Instantiate(itemTooltip, 
            outlinedObject.transform, false);
            
    }

    public void addOutlinedObject(GameObject gameObject)
    {
        current.outlinedObject=gameObject;
    }

    public void removeAllOutlinedObjects()
    {
        current.outlinedObject=null;
    }

    public void OutlineAllObjects()
    {
        if(!outlinedObject.GetComponent<Outline>())
            outlinedObject.AddComponent<Outline>();
        var outline = outlinedObject.GetComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = outlineColor;
        outline.OutlineWidth = outlineWidth;

    }

    public void OutlineObject(GameObject gameObject){
        if(!gameObject.GetComponent<Outline>())
            gameObject.AddComponent<Outline>();
        var outline = gameObject.GetComponent<Outline>();
        outline.enabled=true;
        outline.OutlineMode = Outline.Mode.OutlineAll;
      
    }

    public void notOutlineObject(GameObject gameObject){
        var outline = gameObject.GetComponent<Outline>();
        if(!outline.IsUnityNull())
            outline.OutlineMode = Outline.Mode.OutlineHidden;
    }

    public void notOutlineAnyObject()
    {
        var outline = outlinedObject.GetComponent<Outline>();
        if(!outline.IsUnityNull())
            outline.OutlineMode = Outline.Mode.OutlineHidden;

    }

    public void OnCollisionEnter(Collision collision){
        if(collision.gameObject.transform.GetComponent<ItemObject>()){
            Debug.Log("outline item");
            OutlineObject(collision.gameObject);
        }
    }

    public void OnCollisionExit(Collision collision){
        if(collision.gameObject.transform.GetComponent<ItemObject>()){
            notOutlineObject(collision.gameObject);
        }
    }
}
