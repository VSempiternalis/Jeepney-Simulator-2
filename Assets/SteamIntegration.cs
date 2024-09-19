using UnityEngine;

public class SteamIntegration : MonoBehaviour {
    [SerializeField] private SteamAchievements steamAchievements;

    private void Start() {
        steamAchievements = SteamAchievements.current;

        try {
            Steamworks.SteamClient.Init(2819970);
            PrintSteamName();
            PrintFriends();

            steamAchievements.isSteamActive = true;
        } catch(System.Exception e) {
            print("[STEAM INTEGRATION] Went wrong:" + e);
            steamAchievements.isSteamActive = false;
        }
    }

    private void Update() {
        Steamworks.SteamClient.RunCallbacks();
    }

    private void PrintSteamName() {
        print(Steamworks.SteamClient.Name);
    }

    private void PrintFriends() {
        foreach(var friend in Steamworks.SteamFriends.GetFriends()) {
            print(friend.Name);
        }
    }

    private void OnApplicationQuit() {
        Steamworks.SteamClient.Shutdown();
    }
}
