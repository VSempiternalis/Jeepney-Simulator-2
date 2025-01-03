using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;

public class MissionManager : MonoBehaviour {
    public static MissionManager current;
    private GameManager gm;
    private NotificationManager nm;
    private BoundaryManager bm;
    private CrimeManager cm;
    private AudioManager am;
    private TimeManager tm;
    [SerializeField] private CarController carcon;

    [SerializeField] private TMP_Text missionText;

    [SerializeField] private PlayerInteraction player;

    private List<string> missionNames = new List<string>() {
        "FARE GAME",
        "PRICE HIKE",
        "SIDE HUSTLE",
        "CATCH ME IF YOU CAN",
        "STREET MARKETING",
        "INSURANCE FRAUD",
        "CROWD CONTROL",
        "LATE FOR WORK", //7
        "STAY LOW, NOT SLOW"
    };
    private List<string> missionDescs = new List<string>() {
        "Billy has decided to increase the fare for today!",
        "Geopolitical tensions are causing the fuel prices to increase!",
        "Billy wants you to deliver a package to any drop-off area in ",
        "Billy is wanted for tax evasion! He needs you to distract the police by committing a crime and successfully escaping from them!",
        "Billy wants you to visit a landmark to promote BILLY'S BOUNDARIES. Go to ",
        "Billy needs you to hit three pedestrians for their insurance fraud scheme. He forgot who they were so just hit any three random pedestrians!",
        "Your performance has been dropping lately. We need you to deliver a total of ",
        "A VIP (Very Important Passenger) is late for work! You need to deliver them to ", //7
        "Drive carefully! If you don't alert the police for this shift, you'll get a reward of P"
    };
    private List<int> missionRewards = new List<int>() {
        0,
        0,
        50,
        100,
        50,
        100,
        50,
        50, //7
        50
    };

    private List<string> landmarks = new List<string>() {
        "Tondoo",
        "Factory",
        "Harbor",
        "Fort Saan Tago",
        "Rizz El Park",
        "BBC",
        "Pan De Sal University",
        "Manella Palace",
        "MegaMall",
        "City Hall",
        "Cathedral",
        "Bahay Ni Billy",
        "ISCM",
        "Central Business Town",
        "Keso Memorial Circle",
        "CCM",
        "MAIA",
        "Walana Square Gardens",
        "University of Manella",
        "Aranegus Coliseum",
        "Westwood City",
    };
    [SerializeField] private GameObject package;
    [SerializeField] private StorageHandler packageStorage;
    private string targetLandmark;

    private int passengersDelivered;
    private int targetPassengersDelivered;
    private int passengersHit;

    [SerializeField] private PersonHandler vip;
    private float timeLeft = 300f;
    private bool timerRunning;

    public int missionIndex;
    public string missionName;
    public string missionDesc;
    private bool missionFinished;

    /*
    MISSIONS
    "Fare Game"                     fare Price change
    "Price Hike"                    fuel price change 
    "Side Hustle"                   deliver parcel for extra money
    "Catch Me If You Can"           successfully escape from police for reward 
    "Sightseeing"                   visit an area for reward 
    "insurance fraud"               hit x pedestrians for reward
    "Crowd Control"                 deliver x num of passengers 
    "Late Passenger"                deliver vip to x within y minutes 
    */

    //EVENTS

    private void Awake() {
        current = this;
    }

    private void Start() {
        gm = GameManager.current;
        nm = NotificationManager.current;
        bm = BoundaryManager.current;
        cm = CrimeManager.current;
        tm = TimeManager.current;
        am = AudioManager.current;
        // StartMission(0);
        // NewDay();

        player.OnArrivedAtDropoff += ArrivedAtDropoff;
        CrimeManager.OnPlayerEscape += PlayerEscaped;
        player.OnArrivedAtLandmark += ArrivedAtLandmark;
        CarController.OnPlayerHit += PassengerHit;
        CarController.OnPassengerExit += PassengerExit;
        tm.onHourUpdateEvent += HourUpdate;
        vip.vipExit += VipExit;
        CrimeManager.OnPlayerIsWanted += PlayerWanted;
    }

    private void Update() {
        // if(Input.GetKeyDown(KeyCode.F9)) NewDay();

        if(missionIndex == 7 && timerRunning) {
            if(vip.state == "Waiting") vip.SetState("Waiting to arrive");
            if(timeLeft > 0) {
                timeLeft -= Time.deltaTime;
                missionText.text = missionName + "\n" + missionDesc + " | " + Mathf.Round(timeLeft) + " seconds left";
            } else {
                timeLeft = 0;
                timerRunning = false;
                missionText.text = missionName + "\n" + "Mission failed! Better luck next time.";
                nm.NewNotifColor("MISSION FAILED!", "You have failed to deliver the VIP in time!", 3);

                //sfx
                am.PlayUI(9); //or 21
            }

            //alerts
            if(timeLeft < 60 && timeLeft > 50) {
                nm.NewNotif("LATE FOR WORK", "1 minute left to deliver the VIP. Hurry!");
            } 
            else if(timeLeft < 180 && timeLeft > 170) {
                nm.NewNotif("LATE FOR WORK", "3 minutes left to deliver the VIP!");
            } 
        } else if(missionIndex == 8 && tm.shiftTimeLeft <= 60 && !missionFinished) {
            CompleteMission(8);
        }
    }

    public void NewDay() {
        ResetSettings();
        TryGetNewMission();
    }

    private void ResetSettings() {
        //if on mission 8
        if(missionIndex == 8 && !missionFinished) {
            CompleteMission(8);
        }

        //reset
        missionFinished = false;
        gm.UpdateFare(13);
        gm.UpdateFuelPrice(1);
        package.SetActive(false);
        passengersDelivered = 0;
        vip.gameObject.SetActive(false);
        timeLeft = 300;
        timerRunning = false;
    }

    private void TryGetNewMission() {
        //Roll to see if you get a mission
        int randInt = UnityEngine.Random.Range(0, 2);
        if(randInt == 0) {
            print("NO MISSION");
            nm.NewNotif("REGULAR SHIFT", "You don't have a mission for today!");
            return;
        }

        //get new mission
        missionIndex = UnityEngine.Random.Range(0, missionNames.Count);
        // missionIndex = 8;

        missionName = missionNames[missionIndex];
        missionDesc = missionDescs[missionIndex];

        StartMission(missionIndex);

        //notification and text
        nm.NewNotifColor(missionName, missionDesc + "\n\nTo learn more, go to the [SERVICES] page in your TABLET...", 1);
        missionText.text = missionName + "\n" + missionDesc;
    }

    private void StartMission(int i) {
        print("START MISSON: " + i);
        if(i == 0) {
            print("FARE GAME");
            int newFare = UnityEngine.Random.Range(14, 20);
            gm.UpdateFare(newFare);
            missionDesc += "\nNew Fare: P" + newFare;
        } 
        else if(i == 1) {
            int newPrice = UnityEngine.Random.Range(2, 5);
            print("PRICE HIKE: " + newPrice);
            gm.UpdateFuelPrice(newPrice);
            missionDesc += "\nNew Fuel Price: P" + newPrice + " per liter";
        } 
        else if(i == 2) {
            // spawn package, add to storage
            package.SetActive(true);
            packageStorage.AddItemRandom(package);
            //get destination
            int index = UnityEngine.Random.Range(0, landmarks.Count);
            targetLandmark = landmarks[index];

            missionDesc += targetLandmark + "\nREWARD: P" + missionRewards[i];
        } 
        else if(i == 3) {
            missionDesc += "\nREWARD: P" + missionRewards[i];
        } 
        else if(i == 4) {
            //get destination
            int index = UnityEngine.Random.Range(0, landmarks.Count);
            targetLandmark = landmarks[index];

            missionDesc += targetLandmark + " for a reward of P" + missionRewards[i];
        } 
        else if(i == 5) {
            missionDesc += "\nREWARD: P" + missionRewards[i];
        } 
        else if(i == 6) { //CROWD CONTROL
            //get target passenger num
            targetPassengersDelivered = UnityEngine.Random.Range(1, 4);
            //multiply based on shift time
            print("SHIFT LENGTH: " + tm.shiftLength);
            int shiftTime = tm.shiftLength;
            targetPassengersDelivered *= shiftTime;
            targetPassengersDelivered = Mathf.FloorToInt(targetPassengersDelivered);

            missionDesc += targetPassengersDelivered + " passengers for this shift.\nREWARD: P" + (missionRewards[i]*(targetPassengersDelivered/shiftTime));
        } 
        else if(i == 7) {
            // spawn person
            vip.carCon = carcon;
            vip.gameObject.SetActive(true);
            vip.Start();

            // get destination
            int index = UnityEngine.Random.Range(0, landmarks.Count);
            // print("VIP DEST INDEX: " + index + "/" + landmarks.Count);
            targetLandmark = landmarks[index];
            vip.landmarkDest = targetLandmark;
            // FOR TESTING:
            // targetLandmark = "Terminal";
            // vip.landmarkDest = "Terminal";

            // add to vic
            vip.EnterVehicle();
            vip.SetState("Waiting to arrive");

            timerRunning = true;

            missionDesc += targetLandmark + " in five minutes!\nREWARD: P" + missionRewards[i];
        } 
        else if(i == 8) {
            missionDesc += missionRewards[i];
        } 
    }

    public void CompleteMission(int index) {
        if(missionFinished) {
            print("MISSION ALREADY FINISHED");
            return;
        }

        print("MISSION COMPLETED: " + index);
        if(missionRewards[index] == 0) {
            //no reward
        } else if(missionIndex == 6) {
            nm.NewNotifColor("MISSION COMPLETED!", "Good work! A reward of P" + (missionRewards[index]*targetPassengersDelivered) + " has been added to your deposit!", 1);
            bm.AddToDeposit(missionRewards[index]*targetPassengersDelivered);
        } else {
            nm.NewNotifColor("MISSION COMPLETED!", "Good work! A reward of P" + missionRewards[index] + " has been added to your deposit!", 1);
            bm.AddToDeposit(missionRewards[index]);
        }
        
        if(index == 2) {
            package.SetActive(false);
        }

        missionFinished = true;
        missionText.text = missionName + "\nMission Accomplished!";

        //sfx
        am.PlayUI(24);
    }

    private void HourUpdate(int hours, int days) {
    
    }

    private void ArrivedAtDropoff() {
        print("ARRIVED AT DROPOFF: " + player.currentLandmark + "/" + targetLandmark);

        if(missionIndex == 2 && player.currentLandmark.Contains(targetLandmark)) {
            print("gaming");
            CompleteMission(2);
        }
    }

    private void PlayerEscaped() {
        print("PLAYER ESCAPED");
        if(missionIndex == 3) CompleteMission(3);
    }

    private void ArrivedAtLandmark() {
        print("ARRIVED AT LANDMARK: " + player.currentLandmark + "/" + targetLandmark);

        if(missionIndex == 4 && player.currentLandmark.Contains(targetLandmark)) {
            CompleteMission(4);
        }
    }

    private void PassengerHit() {
        passengersHit ++;
        print("ON PASSENGER HIT: " + passengersHit + "/3");

        if(missionIndex == 5 && !missionFinished) {
            nm.NewNotif("INSURANCE FRAUD", "Pedestrians hit: " + passengersHit + "/3");
            if(passengersHit >= 3) CompleteMission(5);
        }
    }

    private void PassengerExit() {
        if(missionIndex == 6 && !missionFinished) {
            passengersDelivered ++;
            print("ON PASSENGER EXIT: " + passengersDelivered + "/" + targetPassengersDelivered);

            nm.NewNotif("CROWD CONTROL", "Passengers Delivered: " + passengersDelivered + "/" + targetPassengersDelivered);
            if(passengersDelivered >= targetPassengersDelivered) CompleteMission(6);
            // else nm.NewNotif("CROWD CONTROL", "Passengers Delivered: " + passengersDelivered + "/" + targetPassengersDelivered);
        }
    }

    private void VipExit() {
        if(missionIndex == 7 && timeLeft > 0) {
            timerRunning = false;
            CompleteMission(7);
        }
    }

    private void PlayerWanted() {
        if(missionIndex == 8) {
            missionFinished = true;
            nm.NewNotifColor("MISSION FAILED!", "You have alerted the police! You will not get the P50 bonus", 3);
            am.PlayUI(9);
        }
    }
}