using System.Collections.Generic;
using UnityEngine;

public class SteamAchievements : MonoBehaviour {
    public static SteamAchievements current;
    [SerializeField] private int achievementsCount;
    [SerializeField] private int totalAchievementsCount;

    private List<int> followerInts = new List<int>{0, 0, 0, 0};

    private void Awake() {
        current = this;
    }

    private void Start() {
        achievementsCount = PlayerPrefs.GetInt("AchievementsCount", 0);
    }

    private void Update() {
        
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
        var ach = new Steamworks.Data.Achievement(id);
        print($"Achievement {id} status: {ach.State}");
    }

    public void UnlockAchievement(string id) {
        var ach = new Steamworks.Data.Achievement(id);
        ach.Trigger();
        print($"Achievement {id} unlocked");

        achievementsCount ++;
        if(achievementsCount == totalAchievementsCount - 1) UnlockAchievement("ACH_KING_OF_THE_ROAD");
        PlayerPrefs.SetInt("AchievementsCount", achievementsCount);
    }

    public void ClearAchievement(string id) {
        var ach = new Steamworks.Data.Achievement(id);
        ach.Clear();
        print($"Achievement {id} status cleared");
    }
}
