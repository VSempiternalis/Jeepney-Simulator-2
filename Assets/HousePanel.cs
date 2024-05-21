using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;
using UnityEngine.XR;

public class HousePanel : MonoBehaviour {
    public static HousePanel current;

    [SerializeField] private List<int> housesOwned = new List<int>{0, 0, 0};
    [SerializeField] private List<int> housePrices;
    [SerializeField] private List<GameObject> houseButtons;
    [SerializeField] private List<DoorHandler> houseDoors;

    [SerializeField] private List<string> house1Landmarks;
    [SerializeField] private List<string> house2Landmarks;
    [SerializeField] private List<string> house3Landmarks;

    private BoundaryManager bm;
    [SerializeField] private RouteSelector rs;

    private void Awake() {
        current = this;
    }

    private void Start() {
        bm = BoundaryManager.current;
        // rs = RouteSelector.current;
    }

    private void Update() {
        
    }

    public void Load() {
        string gameMode = SaveLoadSystem.current.gameMode;

        if(PlayerPrefs.GetInt(gameMode + "_House1Owned", 0) == 1) UnlockHouse(0);
        if(PlayerPrefs.GetInt(gameMode + "_House2Owned", 0) == 1) UnlockHouse(1);
        if(PlayerPrefs.GetInt(gameMode + "_House3Owned", 0) == 1) UnlockHouse(2);
    }

    public void Save() {
        string gameMode = SaveLoadSystem.current.gameMode;

        PlayerPrefs.SetInt(gameMode + "_House1Owned", housesOwned[0]); 
        PlayerPrefs.SetInt(gameMode + "_House2Owned", housesOwned[1]); 
        PlayerPrefs.SetInt(gameMode + "_House3Owned", housesOwned[2]); 
    }

    public void TryBuyHouse(int houseNum) {
        if(bm.CanPay(housePrices[houseNum])) {
            UnlockHouse(houseNum);
        } else {
            NotificationManager.current.NewNotifColor("INSUFFICIENT FUNDS!", "You do not have enough money in your deposit to afford this.", 2);
        }
    }

    private void UnlockHouse(int houseNum) {
        //own house
        housesOwned[houseNum] = 1;

        //unlock door
        houseDoors[houseNum].isLocked = false;

        //remove button
        houseButtons[houseNum].SetActive(false);

        //add landmarks
        List<string> landmarks = new List<string>();

        if(houseNum == 0) landmarks = house1Landmarks;
        else if(houseNum == 1) landmarks = house2Landmarks;
        else landmarks = house3Landmarks;

        foreach(string landmark in landmarks) {
            print("unlocking landmark: " + landmark);
            rs.allDestinations.Add(landmark);
        }

    }
}
