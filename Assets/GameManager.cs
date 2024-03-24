using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour {
    [SerializeField] private TMP_Text versionText;

    public static GameManager current;
    public int pricePerLiter = 20; //fuel price
    [SerializeField] private List<TMP_Text> fuelPriceTexts;

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
}
