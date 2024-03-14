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
        UpdateFuelPriceTexts();
    }

    private void Update() {
        
    }

    private void UpdateFuelPriceTexts() {
        foreach(TMP_Text fuelPriceText in fuelPriceTexts) {
            fuelPriceText.text = "P" + pricePerLiter + ".00";
        }
    }
}
