using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class SaveLoadSystem : MonoBehaviour {
    public static SaveLoadSystem current;

    public string gameMode;
    public bool isNewGame;

    // public int days;
    // public int deposit;
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
    [Header("TIPS")]
    //Hard coded
    [SerializeField] private List<string> tips;
    [SerializeField] private TMP_Text tipText;
    [SerializeField] private MusicPlayer mp;

    [Space(10)]
    [Header("SETUP")]
    // [SerializeField] private Transform spawnPoint;
    // [SerializeField] private Transform mainJeepSpawnPoint;
    // [SerializeField] private Transform player;
    // [SerializeField] private Transform mainJeep;

    [Space(10)]
    [Header("TUTORIAL UI")]
    [SerializeField] private Settings settings;
    [SerializeField] private uiAnimGroup tutorialUI;

    [Space(10)]
    [Header("CUSTOMIZATION")]
    [SerializeField] private List<GameObject> customizationFreeride;
    [SerializeField] private List<GameObject> customizationCareer;

    private void Awake() {
        current = this;
    }

    private void Start() {
        //populate tips
        tips = new List<string>(){
            "Tip 1: For the best performance, try decreasing the RENDER DISTANCE and GRAPHICS PRESET in the settings.",
            "Tip 2: The game will only save when you press the 'PAY BOUNDARY' button. Try not to ALT+F4!",
            "Tip 3: You can change the KEYBINDS anytime in the settings menu.",
            "Tip 4: People waiting in pedestrian lanes WILL NOT BOARD YOUR JEEPNEY!",
            "Tip 5: Want a challenge? Set the transmission to MANUAL and TURN OFF NOOB MODE UI in the settings",
            "Tip 6: You can improve your jeepney's speed and stats by BUYING UPGRADES in billy's office!",
            "Tip 7: You can only carry TEN ITEMS in your hand.",
            "Tip 8: Passengers can't pay their fare if someone else is still trying to pay.",
            "Tip 9: Passengers won't leave the jeepney if they haven't received their change yet.",
            "Tip 10: Pay attention to YOUR FUEL!",
            "Tip 11: Once your shift has ended, YOU CAN NO LONGER PICK UP PASSENGERS.",
            "Tip 12: CRASHING will damage your jeepney and affect its performance.",
            "Tip 13: Always wear a GOOD MORNING TOWEL for luck!",
            "Tip 14: You can buy APARTMENTS in Billy's Office! They unlock new landmarks and serve as checkpoints where you can pay your boundary and save the game!",
            "Tip 15: Try not to kill anyone!",
            "Tip 16: You must purchase the ANTENNA upgrade in order to listen to your favorite music in-game!",
            "Tip 17: Add your favorite songs to the songs folder before launching the game. Adding songs during gameplay requires a restart to take effect.",
            "Tip 18: Try not to drop passengers at ILLEGAL DROP-OFFS",
            "Tip 19: Please leave a REVIEW if you enjoyed the game! It helps us a lot!",
            "Tip 20: Claim your lottery winnings at the GCSO OFFICE!",
            "Tip 21: Feeling lucky? Buy a LOTTERY TICKET and wait for the draw!",
            "Tip 22: Stuck? Out of gas? You can call the TOW TRUCK and drop your jeepney in the nearest gas station!",
            "Tip 23: Visit ALBERTO in the nearest EZ GAS to get change!",
            "Tip 24: Use ATMs to withdraw money from your bank account!",
            // "Tip 665: ",
            "Tip 666: She's behind you."
            // "Tip 667: "
        };

        freeridePanel.SetActive(true);
        careerPanel.SetActive(true);

        gameMode = PlayerPrefs.GetString("Game_GameMode");
        isNewGame = PlayerPrefs.GetInt("Game_isNewGame") == 1? true:false;

        // print("(GAME) Game_isNewGame: " + (isNewGame? 1:0));

        GameStart(isNewGame);
            
        TimeManager.current.Setup();
        BoundaryManager.current.Setup(gameMode);
        JeepneyPanel.current.Setup();
        SpawnArea.current.isRoadEvents = isEvents;

        // //load houses
        // HousePanel.current.Load();

        // //load gamemanager player house num
        // int playerHouse = 0;
        // if(!isNewGame) playerHouse = PlayerPrefs.GetInt(gameMode + "_PlayerHouse", 0);
        // GameManager.current.SetPlayerHouse(playerHouse);

        Setup();
    }

    // private void Update() {
        
    // }

    #region SETUP AND SAVES =========================================================================================

    private void GameStart(bool isNewGame) {
        if(gameMode == "Freeride") {
            LoadFreerideSettings(); //Game settings
            if(isNewGame) NewFreeride(); //Player data
            else LoadFreeride(); //Player data
        } else if(gameMode == "Career") {
            LoadCareerSettings();
            if(isNewGame) NewCareer();
            else LoadCareer();
        }

        if(TimeManager.current.days == 1) {
            tutorialUI.In();
            tutorialUI.GetComponent<CanvasGroup>().blocksRaycasts = true;
            settings.ToggleCursor();
            settings.UpdateCursor();
        }
        
        //CUSTOMIZATION
        if(gameMode == "Freeride") {
            foreach(GameObject go in customizationCareer) {
                go.SetActive(false);
            }
            foreach(GameObject go in customizationFreeride) {
                go.SetActive(true);
            }

            //gear
            // JeepneyPanel.current.SetMaxGear(9);
        } else {
            foreach(GameObject go in customizationFreeride) {
                go.SetActive(false);
            }
            foreach(GameObject go in customizationCareer) {
                go.SetActive(true);
            }
        }
    }

    private void Setup() {
        GameManager.current.Setup();
    }

    private void LoadFreerideSettings() {
        print("Loading freeride settings");

        //GAME SETTINGS ==============================
        isPassengerPickups = PlayerPrefs.GetInt("Freeride_IsPassengerPickup", 1) == 1? true:false;
        isPayments = PlayerPrefs.GetInt("Freeride_IsPayments", 1) == 1? true:false;
        isEvents = PlayerPrefs.GetInt("Freeride_IsEvents", 1) == 1? true:false;
        isShifts = PlayerPrefs.GetInt("Freeride_IsShifts", 1) == 1? true:false;
        // print("isEvents: " + (isEvents? 1:0));
        //MAX POP
        populationCount = PlayerPrefs.GetInt("Freeride_PopulationCount", 50);
        SpawnArea.current.maxPersonCount = populationCount;
        
        //MAX TRAFFIC
        trafficCount = PlayerPrefs.GetInt("Freeride_TrafficCount", 25);
        SpawnArea.current.maxVicCount = trafficCount;
        
        //SHIFT LENGTH
        shiftLength = PlayerPrefs.GetInt("Freeride_ShiftLength", 20);
        TimeManager.current.shiftLength = shiftLength;
        BoundaryManager.current.shiftLength = shiftLength;

        PlayerDriveInput.current.isPickups = isPassengerPickups;

        // freeridePanel.SetActive(true);
        careerPanel.SetActive(false);
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
        // print("Loading career settings");

        //GAME SETTINGS ==============================

        //DEPOSIT
        // deposit = PlayerPrefs.GetInt("Career_Deposit", 0);
        // BoundaryManager.current.deposit = deposit;

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
        
        shiftLength = PlayerPrefs.GetInt("Career_ShiftLength", 20);
        TimeManager.current.shiftLength = shiftLength;
        BoundaryManager.current.shiftLength = shiftLength;
        
        //TIME AND DAY
        // time = PlayerPrefs.GetInt("Career_Time", 0);
        // TimeManager.current.SetTimeTo(480);
        // TimeManager.current.days = PlayerPrefs.GetInt("Career_Day", 1);

        PlayerDriveInput.current.isPickups = isPassengerPickups;

        //SETUPS
        // TimeManager.current.Setup();
        // BoundaryManager.current.Setup(gameMode);

        // careerPanel.SetActive(true);
        freeridePanel.SetActive(false);
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
        print("NEW FREERIDE!");

        //DEPOSIT
        // deposit = PlayerPrefs.GetInt("Freeride_Deposit", 0);
        BoundaryManager.current.deposit = 0;

        //SHIFTS
        TimeManager.current.CheckForShifts(isShifts);

        //TIME AND DAY
        TimeManager.current.days = 1;
        TimeManager.current.SetTimeTo(480);
    }

    private void LoadFreeride() {
        // print("OLD FREERIDE!");

        //DEPOSIT
        BoundaryManager.current.deposit = PlayerPrefs.GetInt("Freeride_Deposit", 0);

        //SHIFTS
        TimeManager.current.CheckForShifts(isShifts);

        //TIME AND DAY
        time = PlayerPrefs.GetInt("Freeride_Time", 0);
        TimeManager.current.days = PlayerPrefs.GetInt("Freeride_Day", 1);
        TimeManager.current.SetTimeTo(time); //THIS GOES LAST! Days is required to be set for SetTimeTo to work!
    }

    private void NewCareer() {
        // print("NEW CAREER!");

        //DEPOSIT
        // deposit = PlayerPrefs.GetInt("Freeride_Deposit", 0);
        BoundaryManager.current.deposit = 0;

        //SHIFTS
        TimeManager.current.CheckForShifts(isShifts);

        //TIME AND DAY
        TimeManager.current.days = 1;
        TimeManager.current.SetTimeTo(480);

        //load houses
        HousePanel.current.Reset();

        PlayerPrefs.SetInt(gameMode + "_PlayerHouse", 0);
        GameManager.current.SetPlayerHouse(0);

        //load gamemanager player house num
        // int playerHouse = 0;
        // if(!isNewGame) playerHouse = PlayerPrefs.GetInt(gameMode + "_PlayerHouse", 0);
        // GameManager.current.SetPlayerHouse(playerHouse);
        // Setup();
    }

    private void LoadCareer() {
        print("OLD CAREER!");

        //DEPOSIT
        BoundaryManager.current.deposit = PlayerPrefs.GetInt("Career_Deposit", 0);

        //SHIFTS
        TimeManager.current.CheckForShifts(isShifts);

        //TIME AND DAY
        time = PlayerPrefs.GetInt("Career_Time", 0);
        TimeManager.current.days = PlayerPrefs.GetInt("Career_Day", 1);
        TimeManager.current.SetTimeTo(time); //THIS GOES LAST! Days is required to be set for SetTimeTo to work!

        //load houses
        HousePanel.current.Load();

        //load gamemanager player house num
        int playerHouse = 0;
        playerHouse = PlayerPrefs.GetInt(gameMode + "_PlayerHouse", 0);
        GameManager.current.SetPlayerHouse(playerHouse);
        // Setup();
    }

    public void SaveGame() {
        print("Saving game");
        //Save vars into sls
        int deposit = BoundaryManager.current.deposit;
        int days = TimeManager.current.days;
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
        
        //SPAWN LOCATION
        PlayerPrefs.SetInt(gameMode + "_PlayerHouse", GameManager.current.playerSpawnLocation);

        Setup();

        //STEAM ACHIEVEMENTS
        if(gameMode == "Freeride") {
            if(days == 2) SteamAchievements.current.UnlockAchievement("ACH_FREERIDE_DAY_2");
            else if(days == 5) SteamAchievements.current.UnlockAchievement("ACH_FREERIDE_DAY_5");
            if(days == 10) SteamAchievements.current.UnlockAchievement("ACH_FREERIDE_DAY_10");
            else if(days == 25) SteamAchievements.current.UnlockAchievement("ACH_FREERIDE_DAY_25");
            if(days == 50) SteamAchievements.current.UnlockAchievement("ACH_FREERIDE_DAY_50");
            else if(days == 100) SteamAchievements.current.UnlockAchievement("ACH_FREERIDE_DAY_100");
        } else { //CAREER
            if(days == 2) SteamAchievements.current.UnlockAchievement("ACH_CAREER_DAY_2");
            else if(days == 5) SteamAchievements.current.UnlockAchievement("ACH_CAREER_DAY_5");
            if(days == 10) SteamAchievements.current.UnlockAchievement("ACH_CAREER_DAY_10");
            else if(days == 25) SteamAchievements.current.UnlockAchievement("ACH_CAREER_DAY_25");
            if(days == 50) SteamAchievements.current.UnlockAchievement("ACH_CAREER_DAY_50");
            else if(days == 100) SteamAchievements.current.UnlockAchievement("ACH_CAREER_DAY_100");
        }

        if(shiftLength == 30) SteamAchievements.current.UnlockAchievement("ACH_OVERTIME_30");
        else if(shiftLength == 45) SteamAchievements.current.UnlockAchievement("ACH_OVERTIME_45");
        else if(shiftLength == 60) SteamAchievements.current.UnlockAchievement("ACH_OVERTIME_60");
    }

    public void OnLose() {
        bool isNewGame = TimeManager.current.days == 1? true:false;

        GameStart(isNewGame);
        
        Setup();

        //STEAM ACH
        SteamAchievements.current.UnlockAchievement("ACH_BREADLOSER");
    }

    #endregion

    #region EXITS

    public void ExitToMain() {
        StartCoroutine(ExitToMainMenu());
    }

    IEnumerator ExitToMainMenu() {
        loadingScreen.In();
        loadingScreen.GetComponent<CanvasGroup>().blocksRaycasts = true;

        //Turn off music
        if(mp.isPlaying) mp.TogglePlay();

        //Tip
        int randInt = Random.Range(0, tips.Count);
        if(tipText) tipText.text = tips[randInt];

        yield return new WaitForSeconds(loadTransitionTime);

        // SceneManager.LoadScene("SCENE - Game");

        AsyncOperation operation = SceneManager.LoadSceneAsync("SCENE - MainMenu");

        //LOADING BAR
        while(!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress/0.9f);
            // print(progress);
            loadingProgress.LeanScaleX(progress, 1f);
            yield return null;
        }
    }

    public void ExitToDesktop() {
        Application.Quit();
    }

    #endregion
}
