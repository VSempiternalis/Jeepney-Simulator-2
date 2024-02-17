using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {
    public bool isPassengerPickups;
    public bool isEvents;
    public int populationCount;
    public int trafficCount;

    //CAREER
    public bool isHardMode;

    [Space(10)]
    [Header("LOADING SCREEN")]
    [SerializeField] private uiAnimGroup loadingScreen;
    [SerializeField] private RectTransform loadingProgress;
    [SerializeField] private float loadTransitionTime = 2f;

    private void Start() {
        string gameMode = PlayerPrefs.GetString("Game_GameMode");
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
        isPassengerPickups = PlayerPrefs.GetInt("Freeride_IsPassengerPickup", 1) == 1? true:false;
        isEvents = PlayerPrefs.GetInt("Freeride_IsEvents", 1) == 1? true:false;
        populationCount = PlayerPrefs.GetInt("Freeride_PopulationCount", 50);
        trafficCount = PlayerPrefs.GetInt("Freeride_TrafficCount", 25);
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
    }

    private void LoadFreeride() {
        //load saved progress
        // print("LOADING SAVED FREERIDE!");
        // print("isPassengerPickups: " + isPassengerPickups);
        // print("isEvents: " + isEvents);
        // print("populationCount: " + populationCount);
        // print("trafficCount: " + trafficCount);
    }

    private void NewCareer() {
    
    }

    private void LoadCareer() {
    
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
