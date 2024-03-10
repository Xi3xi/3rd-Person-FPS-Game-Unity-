using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameController : MonoBehaviour
{
    private static GameController instance;

    private GameManager gameManager;
    private bool isGameLost = false;
    private bool isGameWon = false;
    public static bool GameIsPaused = true;
    private const int BIOMED_CHEM = 0;
    private const int ENG_IT = 1;
    private const int ARTS_MUSIC = 2;

    public static int faculty = -1;

    public GameObject levelControllerPrefab;
    private GameObject mainMenuCanvas;
    private GameObject helpMenuCanvas;
    private GameObject facultyMenuCanvas;
    private GameObject inGameCanvas;
    private GameObject inventoryCanvas;
    private GameObject pauseMenuCanvas;
    private GameObject endMenuCanvas;
    private GameObject cutsceneCanvas;

    public GameObject cutSceneInstruction;
    private GameObject winCanvas;
    private GameObject mapCanvas;

    private GameObject inGameHelpCanvas;

    private GameObject BiomedSkillTreeCanvas;
    private GameObject EngITSkillTreeCanvas;
    private GameObject ArtSkillTreeCanvas;

    private SkillsController skillsController;

    private GameObject levelController;

    private GameObject player;

    public static GameController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameController>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(GameController).Name;
                    instance = obj.AddComponent<GameController>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    private void Start()
    {
        unlockCursor();
        Time.timeScale = 0f;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        player = GameObject.FindGameObjectWithTag("Player");

        InitMainMenu();
        InitFacultyMenu();
        InitHelpMenu();

        inGameCanvas = GameObject.Find("InGameCanvas");
        inGameCanvas.SetActive(false);

        inGameHelpCanvas = GameObject.Find("InGameHelpCanvas");
        inGameHelpCanvas.SetActive(false);

        inventoryCanvas = GameObject.Find("InventoryCanvas");
        inventoryCanvas.SetActive(false);

        pauseMenuCanvas = GameObject.Find("PauseMenuCanvas");
        pauseMenuCanvas.SetActive(false);

        InitEndCanvas();
        InitWinCanvas();

        mapCanvas = GameObject.Find("MapCanvas");
        mapCanvas.SetActive(false);

        BiomedSkillTreeCanvas = GameObject.Find("BiomedSkillTreeCanvas");
        BiomedSkillTreeCanvas.SetActive(false);
        EngITSkillTreeCanvas = GameObject.Find("EngITSkillTreeCanvas");
        EngITSkillTreeCanvas.SetActive(false);
        ArtSkillTreeCanvas = GameObject.Find("ArtSkillTreeCanvas");
        ArtSkillTreeCanvas.SetActive(false);
        
        skillsController = GameObject.Find("Player").transform.Find("SkillController").GetComponent<SkillsController>();

        MyWeapon currentWeapon = GameObject.FindGameObjectWithTag("WeaponHolder").transform.GetChild(0).GetComponent<MyWeapon>();
        currentWeapon.data.maxRound = 20;

        mainMenuCanvas.SetActive(true);
    }

    private IEnumerator runCutscene()
    {
        cutsceneCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);

        VideoPlayer videoPlayer = cutsceneCanvas.transform.Find("Cutscene").GetComponent<VideoPlayer>();
        VideoClip videoClip = videoPlayer.clip;

        // Wait for the video to start playing
        yield return new WaitForEndOfFrame();

        // Skip the video to the last frame if any key is pressed
        while (videoPlayer.frame < (long)videoClip.frameCount - 1) {
            if (Input.anyKeyDown) {
                videoPlayer.frame = (long)videoClip.frameCount - 1;
                break;
            }
            yield return null;
        }

        // Wait for the video to finish playing
        videoPlayer.loopPointReached += OnVideoFinished;
        yield return new WaitForEndOfFrame();
    }

    private void OnVideoFinished(UnityEngine.Video.VideoPlayer vp)
    {
        unlockCursor();
        cutSceneInstruction.SetActive(false);
        mainMenuCanvas.SetActive(true);
        vp.loopPointReached -= OnVideoFinished;
    }
    private void InitSkillTree(){
        switch(faculty){
            case BIOMED_CHEM:
                BiomedSkillTreeCanvas.SetActive(true);
                BiomedSkillTreeCanvas.transform.Find("SkillMenu").gameObject.SetActive(true);
                skillsController.SetSkillTree(BiomedSkillTreeCanvas.transform.Find("SkillMenu").transform.Find("SkillTree").GetComponent<SkillTree>(),
                                              BiomedSkillTreeCanvas.transform.Find("SkillMenu").transform.Find("BreadthSkillCanvas").transform.Find("BreadthSkill").GetComponent<BreadthManager>());
                BiomedSkillTreeCanvas.transform.Find("SkillMenu").gameObject.SetActive(false);
                break;
            case ENG_IT:
                EngITSkillTreeCanvas.SetActive(true);
                EngITSkillTreeCanvas.transform.Find("SkillMenu").gameObject.SetActive(true);
                skillsController.SetSkillTree(EngITSkillTreeCanvas.transform.Find("SkillMenu").transform.Find("SkillTree").GetComponent<SkillTree>(), 
                                              EngITSkillTreeCanvas.transform.Find("SkillMenu").transform.Find("BreadthSkillCanvas").transform.Find("BreadthSkill").GetComponent<BreadthManager>());
                EngITSkillTreeCanvas.transform.Find("SkillMenu").gameObject.SetActive(false);
                break;
            case ARTS_MUSIC:
                ArtSkillTreeCanvas.SetActive(true);
                ArtSkillTreeCanvas.transform.Find("SkillMenu").gameObject.SetActive(true);
                skillsController.SetSkillTree(ArtSkillTreeCanvas.transform.Find("SkillMenu").transform.Find("SkillTree").GetComponent<SkillTree>(), 
                                              ArtSkillTreeCanvas.transform.Find("SkillMenu").transform.Find("BreadthSkillCanvas").transform.Find("BreadthSkill").GetComponent<BreadthManager>());
                ArtSkillTreeCanvas.transform.Find("SkillMenu").gameObject.SetActive(false);
                break;
        }
    }

    private void InitMainMenu(){

        
        mainMenuCanvas = GameObject.Find("MainMenuCanvas");
        GameObject startButton = mainMenuCanvas.transform.Find("MainMenu").transform.Find("StartButton").gameObject;
        Button startButtonComponent = startButton.GetComponent<Button>();
        startButtonComponent.onClick.AddListener(PickFaculty);

        GameObject helpButton = mainMenuCanvas.transform.Find("MainMenu").transform.Find("HelpButton").gameObject;
        Button helpButtonComponent = helpButton.GetComponent<Button>();
        helpButtonComponent.onClick.AddListener(DisplayHelp);

        GameObject exitButton = mainMenuCanvas.transform.Find("MainMenu").transform.Find("ExitButton").gameObject;
        Button exitButtonComponent = exitButton.GetComponent<Button>();
        exitButtonComponent.onClick.AddListener(ExitGame);
        
        mainMenuCanvas.SetActive(false);
    }

    private void InitFacultyMenu(){
        facultyMenuCanvas = GameObject.Find("FacultyChoiceCanvas");

        GameObject biomedButton = facultyMenuCanvas.transform.Find("FacultyChoice").transform.Find("Biomed").gameObject;
        Button biomedButtonComponent = biomedButton.GetComponent<Button>();
        biomedButtonComponent.onClick.AddListener(ChooseBiomed);

        GameObject engITButton = facultyMenuCanvas.transform.Find("FacultyChoice").transform.Find("EngIT").gameObject;
        Button engITButtonComponent = engITButton.GetComponent<Button>();
        engITButtonComponent.onClick.AddListener(ChooseEngIT);

        GameObject artButton = facultyMenuCanvas.transform.Find("FacultyChoice").transform.Find("Art").gameObject;
        Button artButtonComponent = artButton.GetComponent<Button>();
        artButtonComponent.onClick.AddListener(ChooseArt);

        facultyMenuCanvas.SetActive(false);
    }

    private void ChooseBiomed(){
        ChooseFaculty(BIOMED_CHEM);
    }

    private void ChooseEngIT(){
        ChooseFaculty(ENG_IT);
    }

    private void ChooseArt(){
        ChooseFaculty(ARTS_MUSIC);
    }

    private void ChooseFaculty(int f){
        faculty = f;
        facultyMenuCanvas.SetActive(false);
        helpMenuCanvas.SetActive(true);
    }

    private void InitHelpMenu(){
        helpMenuCanvas = GameObject.Find("HelpCanvas");
        GameObject startButton = helpMenuCanvas.transform.Find("Help").transform.Find("ContinueButton").gameObject;
        Button startButtonComponent = startButton.GetComponent<Button>();
        startButtonComponent.onClick.AddListener(StartGame);

        helpMenuCanvas.SetActive(false);
    }

    private void PickFaculty(){
        unlockCursor();
        mainMenuCanvas.SetActive(false);
        helpMenuCanvas.SetActive(false);
        facultyMenuCanvas.SetActive(true);
    }

    private void StartGame(){

        if(faculty == -1){
            PickFaculty();
            return;
        }

        ResetPlayer();
        Time.timeScale = 1f;
        GameIsPaused = false;
        mainMenuCanvas.SetActive(false);
        helpMenuCanvas.SetActive(false);
        facultyMenuCanvas.SetActive(false);
        inGameCanvas.SetActive(true);

        pauseMenuCanvas.SetActive(false);
        endMenuCanvas.SetActive(false);
        inventoryCanvas.SetActive(false);

        InitSkillTree();
        InitLevelController();

        lockCursor();
    }

    private void InitLevelController(){
        // inititalise a level controller
        levelController = Instantiate(levelControllerPrefab);
        levelController.name = "LevelController";

        gameManager.checkUpdate();
    }

    private void DisplayHelp(){
        mainMenuCanvas.SetActive(false);
        helpMenuCanvas.SetActive(true);
    }

    private void ExitGame(){
        Application.Quit();
    }

    public void unlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void lockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public GameObject GetInGameCanvas(){
        return inGameCanvas;
    }

    public GameObject GetInGameHelpCanvas(){
        return inGameHelpCanvas;
    }

    public GameObject GetInventoryCanvas(){
        return inventoryCanvas;
    }

    public GameObject GetPauseMenuCanvas(){
        return pauseMenuCanvas;
    }

    public GameObject GetEndMenuCanvas(){
        return endMenuCanvas;
    }

    public GameObject GetBiomedSkillTreeCanvas(){
        return BiomedSkillTreeCanvas;
    }   

    public GameObject GetEngITSkillTreeCanvas(){
        return EngITSkillTreeCanvas;
    }

    public GameObject GetArtSkillTreeCanvas(){
        return ArtSkillTreeCanvas;
    }

    public GameObject GetMapCanvas(){
        return mapCanvas;
    }

    public void EnableMapInfos(){
        levelController.GetComponent<LevelController>().EnableMapInfos();
    }

    public void InitEndCanvas(){
        endMenuCanvas = GameObject.Find("EndMenuCanvas");

        GameObject newButton = endMenuCanvas.transform.Find("EndMenu").transform.Find("RestartButton").gameObject;
        Button newButtonComponent = newButton.GetComponent<Button>();
        newButtonComponent.onClick.AddListener(NewRound);

        GameObject exitButton = endMenuCanvas.transform.Find("EndMenu").transform.Find("ExitButton").gameObject;
        Button exitButtonComponent = exitButton.GetComponent<Button>();
        exitButtonComponent.onClick.AddListener(ExitGame);

        endMenuCanvas.SetActive(false);
    }

    public void InitWinCanvas(){
        winCanvas = GameObject.Find("WinCanvas");

        GameObject newButton = winCanvas.transform.Find("WinMenu").transform.Find("NewButton").gameObject;
        Button newButtonComponent = newButton.GetComponent<Button>();
        newButtonComponent.onClick.AddListener(NewGame);

        GameObject exitButton = winCanvas.transform.Find("WinMenu").transform.Find("ExitButton").gameObject;
        Button exitButtonComponent = exitButton.GetComponent<Button>();
        exitButtonComponent.onClick.AddListener(ExitGame);

        winCanvas.SetActive(false);
    }

    public void LevelEnd(bool isWon){
        unlockCursor();
        Time.timeScale = 0f;
        GameIsPaused = true;
        Destroy(levelController);
        if(!isWon){
            endMenuCanvas.SetActive(true);
        }
        else if(isWon){
            winCanvas.SetActive(true);
        }
    }

    private void NewRound(){
        GameObject [] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies){
            Destroy(enemy);
        }
        endMenuCanvas.SetActive(false);
        StartGame();
    }
    private void NewGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ResetPlayer(){
        player.transform.position = new Vector3(107, 2, 66);
        player.GetComponent<PlayerManager>().Heal(100);
    }

    
    public bool GetGameIsPaused()
    {
        return GameIsPaused;
    }
}
