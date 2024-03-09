using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CareerManager : MonoBehaviour {
    [SerializeField] private Slider sliderPopulationCount;
    [SerializeField] private Slider sliderTrafficCount;
    [SerializeField] private Slider sliderShiftLength;

    [Space(10)]
    [Header("SAVED DATA")]
    [SerializeField] private TMP_Text text1;
    [SerializeField] private TMP_Text text2;
    // [SerializeField] private TMP_Text text3;
    [SerializeField] private TMP_Text hsDayText;
    [SerializeField] private TMP_Text hsMoneyText;
    [SerializeField] private TMP_Text hsSettingsText;

    private void Start() {
        LoadHighScore();
        LoadSavedStats();
        // LoadDefaultSettings();
    }

    private void Update() {
        
    }

    // private void LoadDefaultSettings() {
        
    // }

    private void LoadHighScore() {
        string dayCount = PlayerPrefs.GetInt("Career_HS_Day", 1) + "";
        string deposit = PlayerPrefs.GetInt("Career_HS_Deposit", 0) + "";
        string maxPop = PlayerPrefs.GetInt("Career_HS_MaxPop", 50) + "";
        string maxTraffic = PlayerPrefs.GetInt("Career_HS_MaxTraffic", 25) + "";
        string shiftLength = PlayerPrefs.GetInt("Career_HS_ShiftLength", 15) + "";
    
        hsDayText.text = "DAY " + dayCount;
        hsMoneyText.text = "P" + deposit;
        hsSettingsText.text = maxPop + "\n" + maxTraffic + "\n" + shiftLength;
    }

    private void LoadSavedStats() {
        string dayCount = PlayerPrefs.GetInt("Career_Day", 1) + "";
        string deposit = PlayerPrefs.GetInt("Career_Deposit", 0) + "";
        // string isPassengerPickup = (PlayerPrefs.GetInt("Career_IsPassengerPickup", 1) == 1)? "ON":"OFF";
        // string isPayments = (PlayerPrefs.GetInt("Career_IsPayments", 1) == 1)? "ON":"OFF";
        // string isEvents = (PlayerPrefs.GetInt("Career_IsEvents", 1) == 1)? "ON":"OFF";
        // string isShifts = (PlayerPrefs.GetInt("Career_IsShifts", 1) == 1)? "ON":"OFF";
        string maxPop = PlayerPrefs.GetInt("Career_MaxPop", 50) + "";
        string maxTraffic = PlayerPrefs.GetInt("Career_MaxTraffic", 25) + "";
        string shiftLength = PlayerPrefs.GetInt("Career_ShiftLength", 15) + "";

        text1.text = "Day: " + dayCount + "\n" +
            "Money: " + deposit;

        // text2.text = "Payments: " + isPayments + "\n" +
        //     "Events: " + isEvents + "\n" +
        //     "Shifts: " + isShifts + "\n";

        text2.text = "Max Population: " + maxPop + "\n" +
            "Max Traffic: " + maxTraffic + "\n" +
            "Shift Length: " + shiftLength + "\n";
    }

    //Only runs when start new is pressed
    public void SaveCareerSettings() {
        SetPopulationCount(Mathf.RoundToInt(sliderPopulationCount.value));
        SetTrafficCount(Mathf.RoundToInt(sliderTrafficCount.value));
        SetShiftLength(Mathf.RoundToInt(sliderShiftLength.value));

        //RESET PROGRESS
        PlayerPrefs.SetInt("Career_Deposit", 0);
        PlayerPrefs.SetInt("Career_Day", 1);
        PlayerPrefs.SetInt("Career_Time", 0);
    }

    public void SetPopulationCount(float newVal) {
        PlayerPrefs.SetInt("Career_MaxPop", Mathf.FloorToInt(newVal));
    }

    public void SetTrafficCount(float newVal) {
        PlayerPrefs.SetInt("Career_MaxTraffic", Mathf.FloorToInt(newVal));
    }

    public void SetShiftLength(float newVal) {
        PlayerPrefs.SetInt("Career_ShiftLength", Mathf.FloorToInt(newVal));
    }
}
