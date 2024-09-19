using System.Collections.Generic;
using UnityEngine;

public class SteamAchievements : MonoBehaviour {
    public bool isSteamActive;
    public static SteamAchievements current;
    // [SerializeField] private int achievementsCount;
    // [SerializeField] private int totalAchievementsCount;

    private List<int> followerInts = new List<int>{0, 0, 0, 0};
    private int totalKills;
    bool isDone;

    private void Awake() {
        current = this;
    }

    private void Start() {
        // achievementsCount = PlayerPrefs.GetInt("AchievementsCount", 0);
        totalKills = PlayerPrefs.GetInt("TotalKills", 0);
    }

    private void Update() {
        if(!isDone) {
            // ClearAchievement("ACH_PATOK_JEEPNEY");
            // ClearAchievement("ACH_SKIN_COLLECTOR");
            isDone = true;
        }
    }

    public void AddKill() {
        totalKills ++;
        PlayerPrefs.SetInt("TotalKills", totalKills);

        if(totalKills >= 1) UnlockAchievement("ACH_KILL_1"); 
        if(totalKills >= 5) UnlockAchievement("ACH_KILL_5"); 
        if(totalKills >= 10) UnlockAchievement("ACH_KILL_10"); 
        if(totalKills >= 25) UnlockAchievement("ACH_KILL_25"); 
        if(totalKills >= 50) UnlockAchievement("ACH_KILL_50"); 
        if(totalKills >= 100) UnlockAchievement("ACH_KILL_100"); 
    }

    public void ACH_FOLLOWER(int i) {
        followerInts[i] = 1;
        
        //Check
        bool unlock = true;
        foreach(int link in followerInts) {
           if(link == 0) unlock = false;
        }
        if(unlock) UnlockAchievement("ACH_FOLLOWER");
    }

    public void CheckAchievementState(string id) {
        if(!isSteamActive) return;
        
        var ach = new Steamworks.Data.Achievement(id);
        print($"Achievement {id} status: {ach.State}");
    }

    public void UnlockAchievement(string id) {
        if(!isSteamActive) return;

        var ach = new Steamworks.Data.Achievement(id);
        ach.Trigger();
        print($"Achievement {id} unlocked");

        // achievementsCount ++;
        // if(achievementsCount == totalAchievementsCount - 1) UnlockAchievement("ACH_KING_OF_THE_ROAD");
        // PlayerPrefs.SetInt("AchievementsCount", achievementsCount);
    }

    public void ClearAchievement(string id) {
        if(!isSteamActive) return;

        var ach = new Steamworks.Data.Achievement(id);
        ach.Clear();
        print($"Achievement {id} status cleared");
    }
}
