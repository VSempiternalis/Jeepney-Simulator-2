using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour {
    public static GameManager current;
    public int pricePerLiter = 20; //fuel price
    [SerializeField] private List<TMP_Text> fuelPriceTexts;

    private void Awake() {
        current = this;
    }

    private void Start() {
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
}
