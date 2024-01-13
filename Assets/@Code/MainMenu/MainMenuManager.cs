using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
    private void Start() {
        
    }

    private void Update() {
        
    }

    public void StartGameOfflineFreeride() {
        SceneManager.LoadScene("SCENE - Game");
    }

    public void ExitGame() {
        Application.Quit();
    }
}
