using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour {
    public static GameManager current;

    [SerializeField] private TMP_Text versionText;

    public int pricePerLiter = 20; //fuel price
    [SerializeField] private List<TMP_Text> fuelPriceTexts;

    public int playerSpawnLocation; //0 - Billys, 1 - Cathedral, 2 - Westwood, 3 - BBC

    //SETUP
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerVic;

    //SPAWNS
    [SerializeField] private List<Transform> playerSpawns;
    [SerializeField] private List<Transform> playerVicSpawns;

    private void Awake() {
        current = this;
    }

    private void Start() {
        versionText.text = Application.version;

        foreach(GameObject fuelText in GameObject.FindGameObjectsWithTag("GasText")) {
            fuelPriceTexts.Add(fuelText.GetComponent<TMP_Text>());
        }
        
        UpdateFuelPriceTexts();
    }

    private void Update() {
        
    }

    private void UpdateFuelPriceTexts() {
        foreach(TMP_Text fuelPriceText in fuelPriceTexts) {
            fuelPriceText.text = pricePerLiter + ".00/L"; //"P" + 
        }
    }

    public void SetPlayerHouse(int newHouseNum) {
        playerSpawnLocation = newHouseNum;
    }

    public void Setup() {
        player.position = playerSpawns[playerSpawnLocation].position;
        playerVic.position = playerVicSpawns[playerSpawnLocation].position;
        playerVic.rotation = playerVicSpawns[playerSpawnLocation].rotation;
    }
}
