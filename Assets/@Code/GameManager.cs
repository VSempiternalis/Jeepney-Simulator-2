using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour {
    public static GameManager current;

    public string gameMode;

    public int dayCount;
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

    [Space(10)]
    [Header("OFFICE PANEL")]
    [SerializeField] private GameObject freeridePanel;
    [SerializeField] private TMP_Text freerideSettingsText;
    // [SerializeField] private GameObject startShiftButton;

    [Space(10)]
    [Header("LOADING SCREEN")]
    [SerializeField] private uiAnimGroup loadingScreen;
    [SerializeField] private RectTransform loadingProgress;
    [SerializeField] private float loadTransitionTime = 2f;

    private void Awake() {
        current = this;
    }

    private void Start() {
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
    }

    private void Update() {
        
    }

    //=========================================================================================

    private void LoadFreerideSettings() {
        // dayCount = PlayerPrefs.GetInt("Freeride_DayCount", 1);
        deposit = PlayerPrefs.GetInt("Freeride_Deposit", 0);
        isPassengerPickups = PlayerPrefs.GetInt("Freeride_IsPassengerPickup", 1) == 1? true:false;
        isPayments = PlayerPrefs.GetInt("Freeride_IsPayments", 1) == 1? true:false;
        isEvents = PlayerPrefs.GetInt("Freeride_IsEvents", 1) == 1? true:false;
        isShifts = PlayerPrefs.GetInt("Freeride_IsShifts", 1) == 1? true:false;
        populationCount = PlayerPrefs.GetInt("Freeride_PopulationCount", 50);
        trafficCount = PlayerPrefs.GetInt("Freeride_TrafficCount", 25);
        shiftLength = PlayerPrefs.GetInt("Freeride_ShiftLength", 10);
        time = PlayerPrefs.GetInt("Freeride_Time", 0);
        TimeManager.current.SetTimeTo(480);
        dayCount = TimeManager.current.GetDays() + 1;

        SpawnArea.current.maxVicCount = trafficCount;
        SpawnArea.current.maxPersonCount = populationCount;
        PlayerDriveInput.current.isTakingPassengers = isPassengerPickups;

        freerideSettingsText.text = dayCount + "\n" +
            "P" + deposit + "\n" +
            (isPassengerPickups? "ON":"OFF") + "\n" +
            (isPayments? "ON":"OFF") + "\n" +
            (isEvents? "ON":"OFF") + "\n" +
            (isShifts? "ON":"OFF") + "\n" +
            populationCount + "\n" +
            trafficCount + "\n"+
            shiftLength + "\n";
    }

    private void LoadCareerSettings() {
    
    }
    
    private void NewFreeride() {
        //new game setup
        print("NEW FREERIDE!");
        print("isPassengerPickups: " + isPassengerPickups);
        print("isEvents: " + isEvents);
        print("populationCount: " + populationCount);
        print("trafficCount: " + trafficCount);
        TimeManager.current.CheckForShifts(isShifts);
    }

    private void LoadFreeride() {
        //load saved progress
        // print("LOADING SAVED FREERIDE!");
        // print("isPassengerPickups: " + isPassengerPickups);
        // print("isEvents: " + isEvents);
        // print("populationCount: " + populationCount);
        // print("trafficCount: " + trafficCount);
        TimeManager.current.SetTimeTo(time);
    }

    private void NewCareer() {
        TimeManager.current.SetTimeTo(480);
    }

    private void LoadCareer() {
    
    }

    private void SaveGame() {
    
    }

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
