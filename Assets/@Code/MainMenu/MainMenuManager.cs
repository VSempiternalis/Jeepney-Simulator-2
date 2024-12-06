using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
    [SerializeField] private uiAnimGroup loadingScreen;
    [SerializeField] private RectTransform loadingProgress;
    [SerializeField] private float loadTransitionTime = 2f;

    [Space(10)]
    [Header("TIPS")]
    //Hard coded
    private List<string> tips = new List<string>(){
        "Tip 1: For the best performance, try decreasing the RENDER DISTANCE and QUALITY in the settings.",
        "Tip 2: The game will only save when you press the 'PAY BOUNDARY' button. Try not to ALT+F4!",
        "Tip 3: You can change the KEYBINDS in the settings by pressing [ESC].",
        "Tip 4: People waiting in pedestrian lanes WILL NOT BOARD YOUR JEEPNEY!",
        "Tip 5: Want a challenge? Set the transmission to MANUAL and TURN OFF NOOB MODE UI in the settings",
        "Tip 6: You can improve your jeepney's speed and stats by BUYING UPGRADES in Billy's Office!",
        "Tip 7: You can only carry TEN ITEMS in your hand. Unless you buy the wallet upgrade in Billy's Office!",
        "Tip 8: Passengers can't pay their fare if someone else is still trying to pay.",
        "Tip 9: Passengers won't leave the jeepney if they haven't received their change yet.",
        "Tip 10: Pay attention to YOUR FUEL! Always know where the nearest gas station is.",
        "Tip 11: Once your shift has ended, YOU CAN NO LONGER PICK UP PASSENGERS.",
        "Tip 12: CRASHING will damage your jeepney and affect its performance.",
        "Tip 13: Always wear a GOOD MORNING TOWEL for luck!",
        "Tip 14: You can buy APARTMENTS in Billy's Office! They unlock new landmarks and serve as checkpoints where you can pay your boundary and save the game!",
        "Tip 15: Try not to hit anyone while driving!                   ",
        "Tip 16: You must purchase the ANTENNA upgrade in order to listen to your favorite music in-game!",
        "Tip 17: Add your favorite songs to the songs folder (found in settings) before launching the game. Adding songs during gameplay requires a restart to take effect.",
        "Tip 18: Try not to drop passengers at ILLEGAL DROP-OFFS",
        "Tip 19: Please leave a REVIEW if you enjoyed the game! It helps us a lot!",
        "Tip 20: Claim your lottery winnings at the GCSO OFFICE!",
        "Tip 21: Feeling lucky? Buy a LOTTERY TICKET and wait for the draw!",
        "Tip 22: Stuck? Out of gas? Use your tablet to find the TOW TRUCK and drop your jeepney in the nearest gas station or even in Billy's Office!",
        "Tip 23: Visit ALBERTO in the nearest EZ GAS to get change!",
        "Tip 24: Use ATMs to withdraw money from your deposit!",
        "Tip 25: Try not to commit any crimes near police cars! They are marked as red dots in the tablet map.",
        "Tip 25: Never drink and drive!                                ",
        "Tip 26: Use your arrow keys to activate your turn indicators! Vehicles will stop for you and let you turn.",
        "Tip 27: Avoid running a red light, colliding with a vehicle or person, or unloading illegaly when a cop car is nearby! You will see them as red dots on the tablet map.",
        "Tip 28: Encountering bugs? You can reach out to us on our facebook page. Please be as specific as you can about the bug and how you encountered it.",
        "Tip 29: You can discover hidden shortcuts between landmarks in the map. Explore!",
        "Tip 30: Want to unlock landmarks that you've already visited? There is a button for that in the tablet map. You can now also disable landmarks when you leave them!",
        "Tip 31: When being chased by police, drive as far away from them as you can! They will stop and let you go over time. Don't do this in real life please!",
        "Tip 32: Passengers will sometimes let you know whether they want to ride on your jeepney or not. Pay attention to their body language!",
        "Tip 33: Want to add your voice to the game? Go to the main menu and find the button on the left to upload your voice lines!",
        "Tip 666: She's behind you.",
    };
    [SerializeField] private TMP_Text tipText;

    private void Awake() {
        // print("DELETE ALL");
        // PlayerPrefs.DeleteAll();
    }

    private void Start() {
        SteamAchievements.current.UnlockAchievement("ACH_STARTUP");
    }

    private void Update() {
        
    }

    public void StartFreeride(bool isNewGame) {
        StartCoroutine(LoadLevel("Freeride", isNewGame));
    }

    public void StartCareer(bool isNewGame) {
        StartCoroutine(LoadLevel("Career", isNewGame));
    }

    public void StartTutorial() {
        StartCoroutine(LoadLevel("Tutorial", true));
    }

    IEnumerator LoadLevel(string gameMode, bool isNewGame) {
        print("Load level: " + gameMode + ", isNewGame: " + isNewGame);

        loadingScreen.In();
        loadingScreen.GetComponent<CanvasGroup>().blocksRaycasts = true;

        //Tips
        int randInt = Random.Range(0, tips.Count);
        print("tip randInt: " + randInt);
        if(tipText) tipText.text = tips[randInt];

        yield return new WaitForSeconds(loadTransitionTime);

        // SceneManager.LoadScene("SCENE - Game");

        if(gameMode == "Tutorial") {
            AsyncOperation operation = SceneManager.LoadSceneAsync("SCENE - Tutorial");
            PlayerPrefs.SetString("Game_GameMode", gameMode);
            PlayerPrefs.SetInt("Game_isNewGame", isNewGame? 1:0);

            //LOADING BAR
            print("(MAIN MENU) Game_isNewGame: " + (isNewGame? 1:0));
            while(!operation.isDone) {
                float progress = Mathf.Clamp01(operation.progress/0.9f);
                // print("Loading bar: " + progress);
                loadingProgress.LeanScaleX(progress, 1f);
                yield return null;
            }
        } else {
            AsyncOperation operation = SceneManager.LoadSceneAsync("SCENE - Game");
            PlayerPrefs.SetString("Game_GameMode", gameMode);
            PlayerPrefs.SetInt("Game_isNewGame", isNewGame? 1:0);

            //LOADING BAR
            print("(MAIN MENU) Game_isNewGame: " + (isNewGame? 1:0));
            while(!operation.isDone) {
                float progress = Mathf.Clamp01(operation.progress/0.9f);
                // print("Loading bar: " + progress);
                loadingProgress.LeanScaleX(progress, 1f);
                yield return null;
            }
        }
    }

    public void ExitGame() {
        Application.Quit();
    }
}
