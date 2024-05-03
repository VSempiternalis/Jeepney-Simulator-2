using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
    [SerializeField] private uiAnimGroup loadingScreen;
    [SerializeField] private RectTransform loadingProgress;
    [SerializeField] private float loadTransitionTime = 2f;

    private void Start() {
        
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
        loadingScreen.In();
        loadingScreen.GetComponent<CanvasGroup>().blocksRaycasts = true;

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
