using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class SaveLoadSystem : MonoBehaviour {
    public static SaveLoadSystem current;

    public string gameMode;

    public int days;
    public int deposit;
    public bool isPassengerPickups;
    public bool isPayments;
    public bool isEvents;
    public bool isShifts;
    public int populationCount;
    public int trafficCount;
    public int shiftLength;
    public int time;

    //CAREER
    public bool isHardMode;
    // public int boundary = 0;
    // public int boundaryPaid = 0;

    [Space(10)]
    [Header("OFFICE PANEL")]
    [SerializeField] private GameObject freeridePanel;
    [SerializeField] private GameObject careerPanel;
    [SerializeField] private TMP_Text freerideSettingsText;
    [SerializeField] private TMP_Text careerSettingsText;
    // [SerializeField] private GameObject startShiftButton;

    [Space(10)]
    [Header("LOADING SCREEN")]
    [SerializeField] private uiAnimGroup loadingScreen;
    [SerializeField] private RectTransform loadingProgress;
    [SerializeField] private float loadTransitionTime = 2f;

    [Space(10)]
    [Header("SETUP")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform mainJeepSpawnPoint;
    [SerializeField] private Transform player;
    [SerializeField] private Transform mainJeep;

    private void Awake() {
        current = this;
    }

    private void Start() {
        freeridePanel.SetActive(false);
        careerPanel.SetActive(false);

        gameMode = PlayerPrefs.GetString("Game_GameMode");
        bool isNewGame = PlayerPrefs.GetInt("Game_isNewGame") == 1? true:false;

        if(gameMode == "Freeride") {
            LoadFreerideSettings();
            if(isNewGame) NewFreeride();
            else LoadFreeride();
        } else if(gameMode == "Career") {
            LoadCareerSettings();
            if(isNewGame) NewCareer();
            else LoadCareer();
        }

        Setup();
    }

    private void Update() {
        
    }

    #region SETUP AND SAVES =========================================================================================

    private void Setup() {
        player.position = spawnPoint.position;
        // player.rotation = spawnPoint.rotation;
        mainJeep.position = mainJeepSpawnPoint.position;
        mainJeep.rotation = mainJeepSpawnPoint.rotation;
    }

    private void LoadFreerideSettings() {
        print("Loading freeride settings");

        //DEPOSIT
        deposit = PlayerPrefs.GetInt("Freeride_Deposit", 0);
        // boundary = PlayerPrefs.GetInt("Freeride_Boundary", 0);
        // int lateFee = PlayerPrefs.GetInt("Freeride_LateFee", 0);
        BoundaryManager.current.deposit = deposit;
        // BoundaryManager.current.boundary = boundary;
        // BoundaryManager.current.lateFee = lateFee;

        isPassengerPickups = PlayerPrefs.GetInt("Freeride_IsPassengerPickup", 1) == 1? true:false;
        
        isPayments = PlayerPrefs.GetInt("Freeride_IsPayments", 1) == 1? true:false;
        
        isEvents = PlayerPrefs.GetInt("Freeride_IsEvents", 1) == 1? true:false;
        
        isShifts = PlayerPrefs.GetInt("Freeride_IsShifts", 1) == 1? true:false;
        
        //MAX POP
        populationCount = PlayerPrefs.GetInt("Freeride_PopulationCount", 50);
        SpawnArea.current.maxPersonCount = populationCount;
        
        //MAX TRAFFIC
        trafficCount = PlayerPrefs.GetInt("Freeride_TrafficCount", 25);
        SpawnArea.current.maxVicCount = trafficCount;
        
        shiftLength = PlayerPrefs.GetInt("Freeride_ShiftLength", 15);
        TimeManager.current.shiftLength = shiftLength;
        BoundaryManager.current.shiftLength = shiftLength;
        
        //TIME AND DAY
        time = PlayerPrefs.GetInt("Freeride_Time", 0);
        TimeManager.current.SetTimeTo(480);
        TimeManager.current.days = PlayerPrefs.GetInt("Freeride_Day", 1);

        PlayerDriveInput.current.isTakingPassengers = isPassengerPickups;

        //SETUPS
        TimeManager.current.Setup();
        BoundaryManager.current.Setup(gameMode);

        freeridePanel.SetActive(true);
        freerideSettingsText.gameObject.SetActive(true);
        freerideSettingsText.text = 
            (isPassengerPickups? "ON":"OFF") + "\n" +
            (isPayments? "ON":"OFF") + "\n" +
            (isEvents? "ON":"OFF") + "\n" +
            (isShifts? "ON":"OFF") + "\n" +
            populationCount + "\n" +
            trafficCount + "\n"+
            shiftLength + "\n";
    }

    private void LoadCareerSettings() {
        print("Loading career settings");

        //DEPOSIT
        deposit = PlayerPrefs.GetInt("Career_Deposit", 0);
        BoundaryManager.current.deposit = deposit;

        isPassengerPickups = true;
        
        isPayments = true;
        
        isEvents = true;
        
        isShifts = true;
        
        //MAX POP
        populationCount = PlayerPrefs.GetInt("Career_MaxPop", 50);
        SpawnArea.current.maxPersonCount = populationCount;
        
        //MAX TRAFFIC
        trafficCount = PlayerPrefs.GetInt("Career_MaxTraffic", 25);
        SpawnArea.current.maxVicCount = trafficCount;
        
        shiftLength = PlayerPrefs.GetInt("Career_ShiftLength", 15);
        TimeManager.current.shiftLength = shiftLength;
        BoundaryManager.current.shiftLength = shiftLength;
        
        //TIME AND DAY
        time = PlayerPrefs.GetInt("Career_Time", 0);
        TimeManager.current.SetTimeTo(480);
        TimeManager.current.days = PlayerPrefs.GetInt("Career_Day", 1);

        PlayerDriveInput.current.isTakingPassengers = isPassengerPickups;

        //SETUPS
        TimeManager.current.Setup();
        BoundaryManager.current.Setup(gameMode);

        careerPanel.SetActive(true);
        careerSettingsText.gameObject.SetActive(true);
        careerSettingsText.text = 
            // (isPassengerPickups? "ON":"OFF") + "\n" +
            // (isPayments? "ON":"OFF") + "\n" +
            // (isEvents? "ON":"OFF") + "\n" +
            // (isShifts? "ON":"OFF") + "\n" +
            populationCount + "\n" +
            trafficCount + "\n"+
            shiftLength + "\n";
    }
    
    private void NewFreeride() {
        //new game setup
        print("NEW FREERIDE!");
        print("isPassengerPickups: " + isPassengerPickups);
        print("isEvents: " + isEvents);
        print("populationCount: " + populationCount);
        print("trafficCount: " + trafficCount);
        TimeManager.current.CheckForShifts(isShifts);
        TimeManager.current.SetTimeTo(480);
    }

    private void LoadFreeride() {
        //load saved progress
        // print("LOADING SAVED FREERIDE!");
        // print("isPassengerPickups: " + isPassengerPickups);
        // print("isEvents: " + isEvents);
        // print("populationCount: " + populationCount);
        // print("trafficCount: " + trafficCount);
        TimeManager.current.CheckForShifts(isShifts);
        TimeManager.current.SetTimeTo(time);
    }

    private void NewCareer() {
        //new game setup
        print("NEW CAREER!");
        print("isPassengerPickups: " + isPassengerPickups);
        print("isEvents: " + isEvents);
        print("populationCount: " + populationCount);
        print("trafficCount: " + trafficCount);
        TimeManager.current.CheckForShifts(true);
        TimeManager.current.SetTimeTo(480);
    }

    private void LoadCareer() {
        TimeManager.current.CheckForShifts(true);
        TimeManager.current.SetTimeTo(time);
    }

    public void SaveGame() {
        print("Saving game");
        //Save vars into sls
        deposit = BoundaryManager.current.deposit;
        days = TimeManager.current.days;
        time = TimeManager.current.time;

        //DEPOSIT
        PlayerPrefs.SetInt(gameMode + "_Deposit", deposit);
        //DAY
        PlayerPrefs.SetInt(gameMode + "_Day", days);
        //TIME
        PlayerPrefs.SetInt(gameMode + "_Time", time);


        //CAREER HIGH SCORE
        if(gameMode == "Career") {
            int hsMoney = PlayerPrefs.GetInt("Career_HS_Deposit", 0);
            int currentMoney = deposit;

            if(currentMoney > hsMoney) {
                print("NEW HIGHSCORE!");
                PlayerPrefs.SetInt("Career_HS_Day", days);
                PlayerPrefs.SetInt("Career_HS_Deposit", deposit);
                PlayerPrefs.SetInt("Career_HS_MaxPop", populationCount);
                PlayerPrefs.SetInt("Career_HS_MaxTraffic", trafficCount);
                PlayerPrefs.SetInt("Career_HS_ShiftLength", shiftLength);
            }
        }

        Setup();
    }

    public void LoadGame() {
        if(gameMode == "Freeride") {
            LoadFreerideSettings();
            LoadFreeride();
        } else {
            LoadCareerSettings();
            LoadCareer();
        }
        Setup();
    }

    #endregion

    #region EXITS

    public void ExitToMain() {
        StartCoroutine(ExitToMainMenu());
    }

    IEnumerator ExitToMainMenu() {
        loadingScreen.In();
        loadingScreen.GetComponent<CanvasGroup>().blocksRaycasts = true;

        yield return new WaitForSeconds(loadTransitionTime);

        // SceneManager.LoadScene("SCENE - Game");

        AsyncOperation operation = SceneManager.LoadSceneAsync("SCENE - MainMenu");

        //LOADING BAR
        while(!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress/0.9f);
            print(progress);
            loadingProgress.LeanScaleX(progress, 1f);
            yield return null;
        }
    }

    public void ExitToDesktop() {
        Application.Quit();
    }

    #endregion
}
