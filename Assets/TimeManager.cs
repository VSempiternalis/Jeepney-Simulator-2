using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

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
    private bool isShifts;
    private bool isShiftOn;
    public float floatShiftStart;
    public float floatShiftEnd;
    private int shiftHoursLeft;
    private int shiftMinutesLeft;
    private int shiftTimeLeft;
    private List<TMP_Text> shiftTimeTexts = new List<TMP_Text>();
    [SerializeField] private TMP_Text shiftTimeTextRed;

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


    #region SHIFTS ==========================================================================================

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
        if(isShiftOn) return;

        isShiftOn = true;

        shiftTimeLeft = GameManager.current.shiftLength * 60;
    }

    public void TryEndShift() {
        if(!isShiftOn) return;

        isShiftOn = false;

        //stuff
    }

    private void UpdateShiftTime() {
        if(shiftTimeLeft <= 0) {
            //alarm
            //shift over. +1 boundary per second
            return;
        }

        shiftTimeLeft --;

        //MINUTES
        shiftMinutesLeft = shiftTimeLeft % 60; // Remaining minutes after counting hours

        //HOURS
        float totalHoursFloat = shiftTimeLeft / 60.0f;  // Use floating-point division
        int totalHours = Mathf.FloorToInt(totalHoursFloat);
        shiftHoursLeft = totalHours % 24; // Remaining hours after counting days

        UpdateShiftTimers();
    }

    private void UpdateShiftTimers() {
        //Update clocks
        string tempHours = shiftHoursLeft + "";
        string tempMins = shiftMinutesLeft + "";

        if(shiftHoursLeft < 10) tempHours = "0" + tempHours;
        if(shiftMinutesLeft < 10) tempMins = "0" + tempMins;

        if(shiftHoursLeft < 3) {
            foreach(TMP_Text shiftTimer in shiftTimeTexts) {
                shiftTimer.gameObject.SetActive(false);
            }

            shiftTimeTextRed.gameObject.SetActive(true);
            shiftTimeTextRed.text = tempHours + ":" + tempMins;
        } else {
            shiftTimeTextRed.gameObject.SetActive(false);

            foreach(TMP_Text shiftTimer in shiftTimeTexts) {
                shiftTimer.text = tempHours + ":" + tempMins;
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

    // public void SetDay(int newDay) {
    //     days = newDay;
    //     UpdateDayTexts();
    // }

    // public void NextDay() {
    //     days ++;
    //     UpdateDayTexts();
    // }

    private void UpdateDayTexts() {
        foreach(TMP_Text dayText in dayTexts) {
            dayText.text = (days + 1) + "";
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
        days = totalHours / 24;

        UpdateClocks();
        UpdateDayTexts();
    }

    #endregion
}