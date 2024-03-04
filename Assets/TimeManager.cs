using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;

public class TimeManager : MonoBehaviour {
    public static TimeManager current;

    public float floatTime;
    private float minutes;
    [SerializeField] private float minsPerRealSecond;
    public int hours;
    public int days;
    [SerializeField] private TMP_Text daysUI;

    public bool updatingTime;

    private List<TMP_Text> clocks = new List<TMP_Text>();

    //ENVIRONMENT
    private int minHour = 0;
    private int maxHour = 24;
    private SKYPRO_Sun_Rotator sunRotator;
    
    //[EVENTS]
    public event Action<int, int> onHourUpdateEvent;

    // private float nextSecUpdate;

    private void Awake() {
        current = this;
        sunRotator = SKYPRO_Sun_Rotator.current;
        // PlayerPrefs.DeleteAll();
    }

    private void Start() {
        // nextSecUpdate = Mathf.RoundToInt(Time.time) + 1;

        foreach(GameObject clockGO in GameObject.FindGameObjectsWithTag("Clock")) {
            clocks.Add(clockGO.GetComponent<TMP_Text>());
        }

        //[Set to random time]
        // int setTime = Random.Range(0, 23);
        // SetTimeTo(setTime); //12 noon, 0 midnight
        SetTimeTo(8); //12 noon, 0 midnight, 10 standard
        StartCoroutine(UpdateTimeCoroutine());
    }

    private void FixedUpdate() {
        // if(Time.time >= (nextSecUpdate)) {
        //     nextSecUpdate += 1f;
        //     if(updatingTime) UpdateTime();
        // }
    }

    private IEnumerator UpdateTimeCoroutine() {
        while(true) {
            yield return new WaitForSeconds(1f);
            if(updatingTime) UpdateTime();
        }
    }

    public void AddHours(int hoursToAdd) {
        //round off
        if(minutes >= 30) hours ++;
        minutes = 0;

        //add hours
        hours += hoursToAdd;

        UpdateClocks();
    }

    public void SetDay(int newDay) {
        days = newDay;
        daysUI.text = "DAY " + days;
    }

    public void NextDay() {
       days ++;
       daysUI.text = "DAY " + days;
    }

    private void UpdateTime() {
        //sky
        floatTime += minsPerRealSecond;
        sunRotator.time = floatTime;

        //Add minute
        minutes ++; //one minute per real second (1 hour = 1 min)
        // minutes += 0.5f; //30 secs per real second (1 hour = 2 mins)

        // minutesThisDay ++;
        // minutes += 19;

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
        }

        UpdateClocks();
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

    //Uses hours cuz I cant be bothered using minutes
    public void SetTimeTo(int newHours) {
        // print("Setting time to " + newHours);

        hours = newHours;
        minutes = 0;

        UpdateClocks();
    }
}