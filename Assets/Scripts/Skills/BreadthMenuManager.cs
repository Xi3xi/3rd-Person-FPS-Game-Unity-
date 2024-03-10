using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class BreadthMenuManager : MonoBehaviour
{
    public GameObject breadthMenu;

    public GameObject breadthButton;
    private bool isBreadthMenuOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("SkillMenu").Find("BreadthButton").GetComponent<Button_UI>().ClickFunc = () => {
            ExpandBreadthMenu();
        };
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isBreadthMenuOpen)
            {
                CloseBreadthMenu();
            }
        }
    }

    public void ExpandBreadthMenu()
    {
        breadthMenu.SetActive(true);
        isBreadthMenuOpen = true;
    }

    public void CloseBreadthMenu()
    {
        breadthMenu.SetActive(false);
        isBreadthMenuOpen = false;
    }

    public bool GetIsBreadthMenuOpen()
    {
        return isBreadthMenuOpen;
    }
}
