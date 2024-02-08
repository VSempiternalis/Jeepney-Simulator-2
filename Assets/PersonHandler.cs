using UnityEngine;
using System.Collections.Generic;

public class PersonHandler : MonoBehaviour {

    [Space(10)]
    [Header("STATS")]
    public string state;
    private float dropStartTime;
    private float reachDist = 0.35f;
    public string personType;

    [Space(10)]
    [Header("VARIABLES")]
    [SerializeField] private Vector2 moveSpeedRange;
    private float moveSpeed;
    [SerializeField] private Vector2 waitTimeRange;
    private float waitTime;
    [SerializeField] private float callDist;
    [SerializeField] private float magnitudeThresh;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Vector2 payTimeRange;
    private float payTime;
    [SerializeField] private bool isPassenger;

    [Space(10)]
    [Header("PAYMENT")]
    [SerializeField] private int fare = 12;
    [SerializeField] private GameObject coin1PF;
    [SerializeField] private GameObject coin5PF;
    [SerializeField] private GameObject coin10PF;
    [SerializeField] private GameObject cash20PF;
    [SerializeField] private GameObject cash50PF;
    private List<GameObject> money = new List<GameObject>();
    [SerializeField] private List<int> moneySpawnProbabilities = new List<int>{100, 75, 50, 25};
    private bool waitingForChange;
    public int change;

    [Space(10)]
    [Header("ANIMATION")]
    private Animator ani;

    private float nextSecUpdate;
    private Transform player;

    private void Start() {
        //[+] sayHandler = GetComponent<SayHandler>();
        player = GameObject.Find("PLAYER").transform;
        ani = GetComponent<Animator>();

        //Populate money
        money.Add(coin1PF);
        money.Add(coin5PF);
        money.Add(coin10PF);
        money.Add(cash20PF);
        money.Add(cash50PF);

        MakeWait();

        //SET MOVESPEED
        moveSpeed = Random.Range(moveSpeedRange.x, moveSpeedRange.y);

        //[+] GET VOICES
        // if(personType == "Male") voices = voicesMale;
        // else if(personType == "Female") voices = voicesFemale;
        // else if(personType == "Child") {
        //     voices = voicesChild;
        //     //Make children immortal beings
        //     Destroy(GetComponent<Ragdoll>());
        // }
        // else print("VOICE ERROR GODDAMMIT");
        // voiceHandler = GetComponent<VoiceHandler>();
        // int voiceIndex = Random.Range(0, voices.childCount);
        // VoiceType voiceType = voices.GetChild(voiceIndex).GetComponent<VoiceType>();
        // voiceHandler.SetAudioClips(voiceType.payAudios, voiceType.stopAudios, voiceType.deathAudios);
        // patience = maxPatience;

        nextSecUpdate = Time.time + 1;
    }

    private void FixedUpdate() {
        if(nextSecUpdate > 30) SetState("Walking");
        print("nextsecupdate: " + nextSecUpdate);

        if(state == "Idle") return;

        // else if(state == "Paying") PayFare();
        // else if(state == "Wandering") Wander();
        // else if(state == "Moving to vehicle") MoveToVehicle();
        // else if(state == "Waiting to drop") WaitToDrop();
        // else if(state == "Dropping") Dropping();

        if(Time.time >= (nextSecUpdate)) {
            nextSecUpdate ++;
        //     PatienceCheck();
            
        //     if(state == "Waiting") {
        //         Wait();
        //     } else if(state == "Waiting to pay") {
        //         if(Time.time >= payTime) {
        //             if(payPoint.GetComponent<StorageHandler>().value > 0) StartPayTimer();
        //             else PayFare();
        //         }
        //     } else if(state == "Waiting for change") {
        //         // if(changePoint.GetComponent<StorageHandler>().value > 0 && changePoint.GetComponent<StorageHandler>().value <= change) {
        //         if(changePoint.GetComponent<StorageHandler>().value > 0 && changePoint.GetComponent<ChangeHandler>().currentChangee == this) {
        //             // GetChange(changePoint.GetComponent<StorageHandler>().value);
        //             // changePoint.GetComponent<StorageHandler>().Clear();
        //             List<GameObject> removeList = new List<GameObject>();
        //             foreach(GameObject item in changePoint.GetComponent<StorageHandler>().items) {
        //                 if(item.GetComponent<Value>() && item.GetComponent<Value>().value <= change) {
        //                     GetChange(item.GetComponent<Value>().value);
        //                     removeList.Add(item);
        //                 }
        //             }

        //             //[] Remove items taken
        //             foreach(GameObject item in removeList) {
        //                 changePoint.GetComponent<StorageHandler>().RemoveItem(item);
        //                 Destroy(item);
        //             }
        //             changePoint.GetComponent<ChangeHandler>().UpdateText();
        //         }
        //     }
        }
    }

    //==========================================================================================

    private void MakeWait() {
        waitTime = Time.time + Random.Range(waitTimeRange.x, waitTimeRange.y + 1);

        state = "Waiting";
    }

    private void SetState(string newState) {
        //Animation
        if(newState == "Idle") ani.SetInteger("State", 0);
        else if(newState == "Walking") ani.SetInteger("State", 1);
    }
}