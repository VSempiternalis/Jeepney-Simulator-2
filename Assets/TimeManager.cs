using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;

public class TimeManager : MonoBehaviour {
    public static TimeManager current;

    public int time;
    private float minutes;
    [SerializeField] private float secsPerRealMinute = 1f;
    public int hours;
    public int days;
    // [SerializeField] private TMP_Text daysUI;

    public bool updatingTime;

    private List<TMP_Text> clocks = new List<TMP_Text>();
    private List<TMP_Text> dayTexts = new List<TMP_Text>();

    //SHIFTS
    [Space(10)]
    [Header("SHIFT")]
    public int shiftLength;
    private bool isShifts;
    private bool isShiftOn;
    // public float floatShiftStart;
    // public float floatShiftEnd;
    // private int shiftDay; //the day the current shift started
    // private bool isShiftEnded; //should only turn to true after 
    private int shiftHoursLeft;
    private int shiftMinutesLeft;
    private int shiftTimeLeft;
    private List<TMP_Text> shiftTimeTexts = new List<TMP_Text>();
    // [SerializeField] private TMP_Text shiftTimeTextRed;
    [SerializeField] private Color white;
    [SerializeField] private Color red;

    //ENVIRONMENT
    private int minHour = 0;
    private int maxHour = 24;
    [SerializeField] private SKYPRO_Sun_Rotator sunRotator;
    
    //[EVENTS]
    public event Action<int, int> onHourUpdateEvent;

    private void Awake() {
        current = this;
        // PlayerPrefs.DeleteAll();
    }

    private void Start() {
        foreach(GameObject clockGO in GameObject.FindGameObjectsWithTag("Clock")) {
            clocks.Add(clockGO.GetComponent<TMP_Text>());
        }

        foreach(GameObject dayText in GameObject.FindGameObjectsWithTag("DayText")) {
            dayTexts.Add(dayText.GetComponent<TMP_Text>());
        }

        foreach(GameObject shiftTimeText in GameObject.FindGameObjectsWithTag("ShiftTimeText")) {
            shiftTimeTexts.Add(shiftTimeText.GetComponent<TMP_Text>());
        }

        //[Set to random time]
        StartCoroutine(UpdateTimeCoroutine());
    }

    public void Setup() {
        shiftTimeLeft = shiftLength * 60;

        //Update clocks
        foreach(TMP_Text shiftTimer in shiftTimeTexts) {
            shiftTimer.text = shiftLength + ":00";
        }
    }


    #region SHIFTS ==========================================================================================

    public void NewShift() {
        days ++;
        UpdateDayTexts();

        shiftTimeLeft = shiftLength * 60;
        UpdateShiftTimers();
    }

    public void CheckForShifts(bool newIsShifts) {
        isShifts = newIsShifts;

        if(!isShifts) {
            foreach(TMP_Text shiftTimers in shiftTimeTexts) {
                shiftTimers.gameObject.SetActive(false);
            }
        }
    }

    public void TryStartShift() {
        if(!isShifts) return;
        // if(shiftDay == days) return;
        // if(isShiftEnded) return;
        if(isShiftOn) return;

        // shiftTimeLeft = shiftLength * 60;
        isShiftOn = true;
        // shiftDay = days;
    }

    public void TryEndShift() {
        if(!isShiftOn) return;
        // if(isShiftEnded) return;
        // if(shiftDay == days) {
            isShiftOn = false;
            // isShiftEnded = true;

            //stuff
            BoundaryManager.current.UpdateTexts();
        // }
    }

    private void UpdateShiftTime() {
        //OVERTIME!
        if(shiftTimeLeft <= 0) {
            //alarm audio
            BoundaryManager.current.TryAddLateFee();
            //Turn off all normal shift timers
            foreach(TMP_Text shiftTimer in shiftTimeTexts) {
                shiftTimer.text = "-" + BoundaryManager.current.lateFee;
                shiftTimer.color = red;
            }
            // UpdateShiftTimers();

            return;
        }

        shiftTimeLeft --;

        UpdateShiftTimers();
    }

    private void UpdateShiftTimers() {
        print("updating shift timers");

        //MINUTES
        shiftMinutesLeft = shiftTimeLeft % 60;

        //HOURS
        float totalHoursFloat = shiftTimeLeft / 60.0f;
        int totalHours = Mathf.FloorToInt(totalHoursFloat);
        shiftHoursLeft = totalHours % 24;

        //Update clocks
        string tempHours = shiftHoursLeft + "";
        string tempMins = shiftMinutesLeft + "";

        if(shiftHoursLeft < 10) tempHours = "0" + tempHours;
        if(shiftMinutesLeft < 10) tempMins = "0" + tempMins;

        if(shiftHoursLeft < 3) {
            //Turn off all normal shift timers
            foreach(TMP_Text shiftTimer in shiftTimeTexts) {
                shiftTimer.text = tempHours + ":" + tempMins;
                shiftTimer.color = red;
            }
        } else {
            foreach(TMP_Text shiftTimer in shiftTimeTexts) {
                shiftTimer.text = tempHours + ":" + tempMins;
                shiftTimer.color = white;
            }
        }
    }

    #endregion


    #region TIME ==========================================================================================

    //Runs every second
    private IEnumerator UpdateTimeCoroutine() {
        while(true) {
            yield return new WaitForSeconds(secsPerRealMinute);
            if(updatingTime) UpdateTime();
            if(isShiftOn) UpdateShiftTime();
        }
    }

    public int GetDays() {
        return days;
    }

    public void AddHours(int hoursToAdd) {
        //round off
        if(minutes >= 30) hours ++;
        minutes = 0;

        //add hours
        hours += hoursToAdd;

        UpdateClocks();
    }

    private void UpdateDayTexts() {
        foreach(TMP_Text dayText in dayTexts) {
            dayText.text = days + "";
        }
    }

    private void UpdateTime() {
        //sky
        time ++;

        //Add minute
        minutes ++; //one minute per real second (1 hour = 1 min)

        // Reset time to 4 if less than zero
        if(hours < 0) hours = 4;

        // [PER HOUR UPDATE]
        if(minutes >= 60) {
            hours ++;
            minutes = 0;
            if(onHourUpdateEvent != null) onHourUpdateEvent(hours, days);
        }
        // [MIDNIGHT UPDATE]
        if(hours >= 24) {
            hours = 0;
            //day?
        }

        UpdateClocks();
        sunRotator.time = time;
    }

    private void UpdateClocks() {
        //Update clocks
        string tempHours = hours + "";
        string tempMins = Mathf.RoundToInt(minutes) + "";

        if(hours < 10) tempHours = "0" + tempHours;
        if(minutes < 10) tempMins = "0" + tempMins;

        foreach(TMP_Text clock in clocks) {
            clock.text = tempHours + ":" + tempMins;
        }
    }

    public void SetTimeTo(int newTime) {
        time = newTime;
        sunRotator.time = time;

        // hours = newTime % 60;
        // minutes = 0;

        //MINUTES
        minutes = time % 60; // Remaining minutes after counting hours

        //HOURS
        float totalHoursFloat = time / 60.0f;  // Use floating-point division
        int totalHours = Mathf.FloorToInt(totalHoursFloat);
        hours = totalHours % 24; // Remaining hours after counting days

        //DAYS
        // days = totalHours / 24;

        UpdateClocks();
        UpdateDayTexts();
    }

    #endregion
}