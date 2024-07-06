using UnityEngine;

public class DebugManager : MonoBehaviour {
    private bool isDebugOn;
    [SerializeField] private GameObject debugOnText;

    private Transform player;
    [SerializeField] private Transform poi1; //position of interest
    [SerializeField] private Transform poi2;
    [SerializeField] private Transform poi3;

    private void Start() {
        player = PlayerDriveInput.current.transform;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.BackQuote) && Input.GetKey(KeyCode.LeftControl)) {
            isDebugOn = !isDebugOn;
            debugOnText.SetActive(isDebugOn);
        }

        if(isDebugOn) {
            //[1] Teleport to POI
            if(Input.GetKeyDown(KeyCode.Alpha1)) {
                player.position = poi1.position;
            }
            //[2] Teleport to POI
            if(Input.GetKeyDown(KeyCode.Alpha2)) {
                player.position = poi2.position;
            }
            //[3] Teleport to POI
            if(Input.GetKeyDown(KeyCode.Alpha3)) {
                player.position = poi3.position;
            }
            //[4] Check achievement state
            if(Input.GetKeyDown(KeyCode.Alpha4)) {
                SteamAchievements.current.CheckAchievementState("ACH_STARTUP");
            }
            //[5] Trigger achievement
            if(Input.GetKeyDown(KeyCode.Alpha5)) {
                SteamAchievements.current.UnlockAchievement("ACH_STARTUP");
            }
            //[6] Clear achievement
            if(Input.GetKeyDown(KeyCode.Alpha6)) {
                SteamAchievements.current.ClearAchievement("ACH_STARTUP");
            }
            //[7] Subtract one hour to time
            else if(Input.GetKeyDown(KeyCode.Alpha7)) {
                // TimeManager.current.SetTimeTo(540);
                TimeManager.current.AddHours(-1);
            }
            //[8] Add one hour to time
            else if(Input.GetKeyDown(KeyCode.Alpha8)) {
                // TimeManager.current.SetTimeTo(540);
                TimeManager.current.AddHours(1);
            }
            //[9] Switch player run speed to 9 or 60
            else if(Input.GetKeyDown(KeyCode.Alpha9)) {
                FirstPersonMovement fpm = player.GetComponent<FirstPersonMovement>();

                if(fpm.runSpeed == 9) fpm.runSpeed = 60;
                else  fpm.runSpeed = 9;
            }
            //[0] Add 10k money to deposit
            else if(Input.GetKeyDown(KeyCode.Alpha0)) {
                BoundaryManager.current.AddToDeposit(10000);
            }
        }
    }
}
