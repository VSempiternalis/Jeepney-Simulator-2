using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FreerideManager : MonoBehaviour {
    [SerializeField] private Toggle togglePassengerPickups;
    [SerializeField] private Toggle toggleEvents;
    [SerializeField] private Slider sliderPopulationCount;
    [SerializeField] private Slider sliderTrafficCount;

    [SerializeField] private TMP_Text dayText;

    private void Start() {
        
    }

    private void Update() {
        
    }

    public void SaveFreerideSettings() {
        SetPassengerPickups(togglePassengerPickups.isOn);
        SetEvents(toggleEvents.isOn);
        SetPopulationCount(Mathf.RoundToInt(sliderPopulationCount.value));
        SetTrafficCount(Mathf.RoundToInt(sliderTrafficCount.value));
    }

    public void SetDay(int newDay) {
        PlayerPrefs.SetInt("Freeride_DayCount", newDay);
    }

    public void SetDeposit(int newDeposit) {
        PlayerPrefs.SetInt("Freeride_Deposit", newDeposit);
    }

    public void SetPassengerPickups(bool newVal) {
        PlayerPrefs.SetInt("Freeride_IsPassengerPickup", newVal? 1:0);
    }

    public void SetEvents(bool newVal) {
        PlayerPrefs.SetInt("Freeride_IsEvents", newVal? 1:0);
    }

    public void SetPopulationCount(int newVal) {
        PlayerPrefs.SetInt("Freeride_PopulationCount", newVal);
    }

    public void SetTrafficCount(int newVal) {
        PlayerPrefs.SetInt("Freeride_TrafficCount", newVal);
    }
}