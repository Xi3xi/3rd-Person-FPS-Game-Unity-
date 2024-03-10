using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelController : MonoBehaviour
{
    private float timeRemaining = 300.0f;
    public GameObject worldPrefab;
    private TMP_Text countdownText;

    private SkillMenuManager skillMenuManager;

    private bool isLevelLost = false;

    public static bool GameIsPaused = false;

    private GameObject pauseMenuCanvas;
    private GameObject helpMenuCanvas;
    private GameObject inventoryMenuCanvas;

    private GameObject inGameCanvas;
    private GameObject endMenuCanvas;
    private GameObject mapCanvas;

    private GameObject playerCamera;
    private GameManager enemyManager;

    private SpawnBoss spawnBoss;
    private bool bossSpawned = false;

    void Start()
    {
        countdownText = GameObject.FindGameObjectWithTag("TimerText").GetComponent<TMP_Text>();
        UpdateCountdownText();
        InitPauseMenu();

        InitHelpMenu();

        skillMenuManager = GameObject.FindGameObjectWithTag("SkillCanvas").GetComponent<SkillMenuManager>();

        inGameCanvas = GameController.Instance.GetInGameCanvas();
        
        endMenuCanvas = GameController.Instance.GetEndMenuCanvas();
        endMenuCanvas.SetActive(false);
    
        inventoryMenuCanvas = GameController.Instance.GetInventoryCanvas();
        inventoryMenuCanvas.SetActive(false);

        mapCanvas = GameController.Instance.GetMapCanvas();
        mapCanvas.SetActive(false);

        playerCamera = GameObject.FindGameObjectWithTag("Player").transform.Find("CameraHolder").transform.Find("PlayerCam").gameObject;
        
        spawnBoss = GameObject.FindGameObjectWithTag("SpawnBoss").GetComponent<SpawnBoss>();

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().Reset();
        
        LockCursor();
    }

    private void Awake()
    {
        //enemyManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        // instantiate the world
        // Instantiate(worldPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

    void Update()
    {
        if (timeRemaining <= 120 && bossSpawned== false)
        {
            spawnBoss.Spawn();
            bossSpawned = true;
            GameObject.FindWithTag("SpawnBoss").transform.GetChild(0).gameObject.SetActive(true);
            
        }
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateCountdownText();
            //enemyManager.checkUpdate();
        }
        else
        {
            // Time's up!
            GameController.Instance.LevelEnd(false);
            // Add any additional logic here for when the timer runs out
        }

        if(isLevelLost)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                inGameCanvas.SetActive(true);
                CloseAll();
                Resume();
            }
            else
            {
                CloseAll();
                pauseMenuCanvas.SetActive(true);
                Pause();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (skillMenuManager.IsActive())
            {
                skillMenuManager.Resume();
                Resume();
            }
            else
            {
                CloseAll();
                skillMenuManager.Pause();
                Pause();
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryMenuCanvas.activeSelf)
            {
                inventoryMenuCanvas.SetActive(false);
                Resume();
            }
            else
            {
                CloseAll();
                inventoryMenuCanvas.SetActive(true);
                Pause();
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (mapCanvas.activeSelf)
            {
                DisableMap();
                inGameCanvas.SetActive(true);
                Resume();
            }
            else
            {
                CloseAll();
                EnableMap();
                inGameCanvas.SetActive(false);
                Pause();
            }
        }
    }

    private void InitPauseMenu()
    {
        pauseMenuCanvas = GameController.Instance.GetPauseMenuCanvas();

        GameObject startButton = pauseMenuCanvas.transform.Find("PauseMenu").transform.Find("ResumeButton").gameObject;
        Button startButtonComponent = startButton.GetComponent<Button>();
        startButtonComponent.onClick.AddListener(ResumeGame);

        GameObject helpButton = pauseMenuCanvas.transform.Find("PauseMenu").transform.Find("HelpButton").gameObject;
        Button helpButtonComponent = helpButton.GetComponent<Button>();
        helpButtonComponent.onClick.AddListener(DisplayHelp);

        GameObject exitButton = pauseMenuCanvas.transform.Find("PauseMenu").transform.Find("ExitButton").gameObject;
        Button exitButtonComponent = exitButton.GetComponent<Button>();
        exitButtonComponent.onClick.AddListener(ExitGame);

        pauseMenuCanvas.SetActive(false);
    }

    private void InitHelpMenu(){

        helpMenuCanvas = GameController.Instance.GetInGameHelpCanvas();
        GameObject startButton = helpMenuCanvas.transform.Find("Help").transform.Find("ContinueButton").gameObject;
        Button startButtonComponent = startButton.GetComponent<Button>();
        startButtonComponent.onClick.AddListener(ResumeGame);

        helpMenuCanvas.SetActive(false);
    }

    void UpdateCountdownText()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        countdownText.text = string.Format("Time to Oxygen Depletion: {0:00}:{1:00}", minutes, seconds);
    }

    public void Reset(){
        timeRemaining = 300.0f;
        UpdateCountdownText();
    }

    public void Pause()
    {
        UnlockCursor();
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Resume()
    {
        LockCursor();
        inGameCanvas.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    private void ResumeGame()
    {
        pauseMenuCanvas.SetActive(false);
        helpMenuCanvas.SetActive(false);
        Resume();
    }

    private void DisplayHelp(){
        CloseAll();
        pauseMenuCanvas.SetActive(false);
        helpMenuCanvas.SetActive(true);
    }

    private void ExitGame(){
        Application.Quit();
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LevelLost()
    {
        if (!isLevelLost)
        {
            isLevelLost = true;
            endMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
            GameIsPaused = true;
            UnlockCursor();
        }
    }

    private void EnableMap()
    {
        CloseAll();

        playerCamera.SetActive(false);
        mapCanvas.SetActive(true);
    }

    private void DisableMap()
    {
        inGameCanvas.SetActive(true);
        mapCanvas.SetActive(false);
        playerCamera.SetActive(true);
    }

    public void EnableMapInfos()
    {
        GameObject [] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.transform.Find("MapIcon").gameObject.SetActive(true);
        }

        GameObject [] survivors = GameObject.FindGameObjectsWithTag("Survivor");
        foreach (GameObject survivor in survivors)
        {
            // survivor.transform.Find("MapIcon").gameObject.SetActive(true);
        }
    }

    private void CloseAll(){
        skillMenuManager.Resume();
        inventoryMenuCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(false);
        helpMenuCanvas.SetActive(false);
        DisableMap();
    }
}