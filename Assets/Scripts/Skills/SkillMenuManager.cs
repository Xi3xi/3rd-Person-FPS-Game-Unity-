using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkillMenuManager : MonoBehaviour
{

    private GameObject skillMenuUI;
    private BreadthMenuManager breadthMenuManager;

    void Awake()
    {
        skillMenuUI = transform.Find("SkillMenu").gameObject;
        skillMenuUI.SetActive(false);
        breadthMenuManager = GetComponent<BreadthMenuManager>();
        breadthMenuManager.CloseBreadthMenu();
    }
    void Start()
    {
        
    }
    public void Pause()
    {
        skillMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        skillMenuUI.SetActive(false);
        breadthMenuManager.CloseBreadthMenu();
    }

    void Update()
    {
       
    }
    
    public bool IsActive()
    {
        return skillMenuUI.activeSelf;
    }
}
