using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FreerideManager : MonoBehaviour {
    [SerializeField] private Toggle togglePassengerPickups;
    [SerializeField] private Toggle togglePayments;
    [SerializeField] private Toggle toggleEvents;
    [SerializeField] private Slider sliderPopulationCount;
    [SerializeField] private Slider sliderTrafficCount;
    [SerializeField] private Toggle toggleShifts;
    [SerializeField] private Slider sliderShiftLength;

    [Space(10)]
    [Header("SAVED DATA")]
    // [SerializeField] private TMP_Text dayText;
    // [SerializeField] private TMP_Text moneyText;
    // [SerializeField] private TMP_Text pickupsText;
    // [SerializeField] private TMP_Text eventsText;
    // [SerializeField] private TMP_Text maxPopText;
    // [SerializeField] private TMP_Text maxTrafficText;
    [SerializeField] private TMP_Text text1;
    [SerializeField] private TMP_Text text2;
    [SerializeField] private TMP_Text text3;

    private void Start() {
        LoadSavedStats();
        // LoadDefaultSettings();
    }

    private void Update() {
        
    }

    // private void LoadDefaultSettings() {
        
    // }

    private void LoadSavedStats() {
        string dayCount = PlayerPrefs.GetInt("Freeride_Day", 1) + "";
        string deposit = PlayerPrefs.GetInt("Freeride_Deposit", 0) + "";
        string isPassengerPickup = (PlayerPrefs.GetInt("Freeride_IsPassengerPickup", 1) == 1)? "ON":"OFF";
        string isPayments = (PlayerPrefs.GetInt("Freeride_IsPayments", 1) == 1)? "ON":"OFF";
        string isEvents = (PlayerPrefs.GetInt("Freeride_IsEvents", 1) == 1)? "ON":"OFF";
        string isShifts = (PlayerPrefs.GetInt("Freeride_IsShifts", 1) == 1)? "ON":"OFF";
        string popCount = PlayerPrefs.GetInt("Freeride_PopulationCount", 50) + "";
        string trafficCount = PlayerPrefs.GetInt("Freeride_TrafficCount", 25) + "";
        string shiftLength = PlayerPrefs.GetInt("Freeride_ShiftLength", 15) + "";

        text1.text = "Day: " + dayCount + "\n" +
            "Money: " + deposit + "\n" +
            "Pickups: " + isPassengerPickup + "\n";

        text2.text = "Payments: " + isPayments + "\n" +
            "Events: " + isEvents + "\n" +
            "Shifts: " + isShifts + "\n";

        text3.text = "Max Population: " + popCount + "\n" +
            "Max Traffic: " + trafficCount + "\n" +
            "Shift Length: " + shiftLength + "\n";
    }

    //Only runs when start new is pressed
    public void SaveFreerideSettings() {
        SetPassengerPickups(togglePassengerPickups.isOn);
        SetPayments(togglePayments.isOn);
        SetEvents(toggleEvents.isOn);
        SetShifts(toggleShifts.isOn);
        SetPopulationCount(Mathf.RoundToInt(sliderPopulationCount.value));
        SetTrafficCount(Mathf.RoundToInt(sliderTrafficCount.value));
        SetShiftLength(Mathf.RoundToInt(sliderShiftLength.value));

        //RESET PROGRESS
        PlayerPrefs.SetInt("Freeride_Deposit", 0);
        PlayerPrefs.SetInt("Freeride_Day", 1);
        PlayerPrefs.SetInt("Freeride_Time", 0);
    }

    // public void SetDay(int newDay) {
    //     PlayerPrefs.SetInt("Freeride_DayCount", newDay);
    // }

    // public void SetDeposit(int newDeposit) {
    //     PlayerPrefs.SetInt("Freeride_Deposit", newDeposit);
    // }

    public void SetPassengerPickups(bool newVal) {
        PlayerPrefs.SetInt("Freeride_IsPassengerPickup", newVal? 1:0);
    }

    public void SetPayments(bool newVal) {
        print("Setting payments: " + (newVal? "ON":"OFF"));
        PlayerPrefs.SetInt("Freeride_IsPayments", newVal? 1:0);
    }

    public void SetEvents(bool newVal) {
        PlayerPrefs.SetInt("Freeride_IsEvents", newVal? 1:0);
    }

    public void SetPopulationCount(float newVal) {
        PlayerPrefs.SetInt("Freeride_PopulationCount", Mathf.FloorToInt(newVal));
    }

    public void SetTrafficCount(float newVal) {
        PlayerPrefs.SetInt("Freeride_TrafficCount", Mathf.FloorToInt(newVal));
    }

    public void SetShifts(bool newVal) {
        PlayerPrefs.SetInt("Freeride_IsShifts", newVal? 1:0);
    }

    public void SetShiftLength(float newVal) {
        PlayerPrefs.SetInt("Freeride_ShiftLength", Mathf.FloorToInt(newVal));
    }
}