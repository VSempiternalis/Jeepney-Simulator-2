using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class BoundaryManager : MonoBehaviour {
    public static BoundaryManager current;

    private SaveLoadSystem sls;
    public bool doBoundary;
    public int deposit;
    public int boundary;
    public int lateFee;
    public int failureCharge;
    public int failureChargePerPerson;
    public int total;
    public int shiftLength;

    [SerializeField] private int baseBoundary;
    [SerializeField] private int dayMultiplier;
    [SerializeField] private int shiftMultiplier;

    [SerializeField] private Transform depositTo;
    [SerializeField] private StorageHandler storage;

    [SerializeField] private Color white;
    [SerializeField] private Color green;
    [SerializeField] private Color red;

    [Space(10)]
    [Header("MONEY")]
    [SerializeField] private List<TMP_Text> depositTexts = new List<TMP_Text>();
    [SerializeField] private List<TMP_Text> boundaryTexts = new List<TMP_Text>();
    [SerializeField] private List<TMP_Text> lateFeeTexts = new List<TMP_Text>();
    [SerializeField] private List<TMP_Text> failureChargeTexts = new List<TMP_Text>();
    [SerializeField] private List<TMP_Text> totalTexts = new List<TMP_Text>();

    [SerializeField] private Transform spawnPoint;

    [Space(10)]
    [Header("VEHICLE")]
    [SerializeField] private GameObject money1PF;
    [SerializeField] private GameObject money5PF;
    [SerializeField] private StorageHandler storage1;
    [SerializeField] private StorageHandler storage2;
    [SerializeField] private CarController carCon;

    private AudioManager am;

    private void Awake() {
        current = this;
    }

    private void Start() {
        sls = SaveLoadSystem.current;
        am = AudioManager.current;
    }

    private void Update() {
        
    }

    private void ResetVicMoney() {
        storage1.Clear();
        storage2.Clear();

        for(int i = 0; i < 10; i++){
            GameObject newMoney = Instantiate(money1PF, storage1.transform.position, Quaternion.identity);
            // float xPos = Random.Range(storage.transform.position.x - (storage.transform.localScale.x/2), storage.transform.position.x + (storage.transform.localScale.x/2));
            // float zPos = Random.Range(storage.transform.position.z - (storage.transform.localScale.z/2), storage.transform.position.z + (storage.transform.localScale.z/2));
            // Vector3 newPos = new Vector3(xPos, storage1.transform.position.y, zPos);
            storage1.AddItemRandom(newMoney);
            newMoney.name = "Money - P" + newMoney.GetComponent<Value>().value;
        }

        for(int i = 0; i < 3; i++){
            GameObject newMoney = Instantiate(money5PF, storage2.transform.position, Quaternion.identity);
            // float xPos = Random.Range(storage.transform.position.x - (storage.transform.localScale.x/2), storage.transform.position.x + (storage.transform.localScale.x/2));
            // float zPos = Random.Range(storage.transform.position.z - (storage.transform.localScale.z/2), storage.transform.position.z + (storage.transform.localScale.z/2));
            // Vector3 newPos = new Vector3(xPos, storage1.transform.position.y, zPos);
            storage2.AddItemRandom(newMoney);
            newMoney.name = "Money - P" + newMoney.GetComponent<Value>().value;
        }
    }

    public void Setup(string gameMode) {
        if(gameMode == "Career") doBoundary = true;

        CalculateNewBoundary();
        UpdateTexts();
        ResetVicMoney();
        RouteSelector.current.NewShift(3); //3 is dests to lock
    }

    public void TryAddLateFee() {
        if(!doBoundary) return;
        lateFee ++;
    }

    private void CalculateNewBoundary() {
        if(!doBoundary) {
            boundary = 0;
            return;
        }

        // boundary = (TimeManager.current.days + 1) * 10 * (shiftLength * 10) + ;
        int shiftFactor = shiftLength * shiftMultiplier; //money earned per hour/realminute = 10
        int dayFactor = TimeManager.current.days * dayMultiplier;
        boundary = shiftFactor + dayFactor + baseBoundary;
    }

    public void CalculateFailureCharge() {
        if(!PlayerDriveInput.current.carCon) return;

        int passengers = PlayerDriveInput.current.carCon.GetComponent<CarController>().passengerCount;
        failureCharge = passengers * failureChargePerPerson;

        if(passengers > 0) NotificationManager.current.NewNotif("FAILURE CHARGE", "You have " + passengers + " undelivered passengers. You have been given a P" + failureCharge + " failure charge.");
    }

    private void CalculateTotal() {
        total = deposit - (boundary + lateFee + failureCharge);
    }

    public void UpdateTexts() {
        CalculateTotal();

        foreach(TMP_Text depositText in depositTexts) {
            if(deposit < 0) depositText.text = "-P" + Mathf.Abs(deposit);
            else depositText.text = "P" + deposit;
        }

        foreach(TMP_Text boundaryText in boundaryTexts) {
            boundaryText.text = "-P" + boundary;
        }
        
        foreach(TMP_Text lateFeeText in lateFeeTexts) {
            lateFeeText.text = "-P" + lateFee;
        }
        
        foreach(TMP_Text failureChargeText in failureChargeTexts) {
            failureChargeText.text = "-P" + failureCharge;
        }
        
        foreach(TMP_Text totalText in totalTexts) {
            if(total >= 0) {
                totalText.text = "P" + total;
                totalText.color = green;
            } else {
                totalText.text = "-P" + Mathf.Abs(total);
                totalText.color = red;
            }
        }
    }

    public void AddToDeposit(int mod) {
        deposit += mod;
        UpdateTexts();
    }

    public void StorageToDeposit() {
        deposit += storage.value;

        // CalculateTotal();
        UpdateTexts();

        //animate
        storage.Clear();

        //audio
    }

    public void CompleteShift() {
        if(deposit <= 0) {
            NotificationManager.current.NewNotif("EMPTY DEPOSIT", "You need to deposit your earnings first to pay the boundary.");
            AudioManager.current.PlayUI(7);
            return;
        }

        if(total >= 0 || !doBoundary) {
            deposit = total;
            // AddToDeposit(-total);
            string text = "CONGRATULATIONS! YOU MADE THE BOUNDARY!\n\nSaving game...\n";
            if(!doBoundary) text = "Saving game...";

            Fader.current.FadeToBlack(1f, text, () => {
                //Reset
                TimeManager.current.NewShift();
                SaveLoadSystem.current.SaveGame();
                RouteSelector.current.NewShift(3); //3 is dests to lock
                am.PlayUI(3);

                CalculateNewBoundary();
                UpdateTexts();
                
                LeanTween.delayedCall(1f, () => {
                    Fader.current.SetText("DAY " + TimeManager.current.days);
                });

                LeanTween.delayedCall(2f, () => {
                    am.PlayUI(4);
                    Fader.current.FadeFromBlack(1f, "DAY " + TimeManager.current.days, null);
                });
            });
        } else {
            Fader.current.FadeToBlack(1f, "YOU'RE FIRED!\n\nLoading previous save...\n", () => {
                //Reset
                // if(TimeManager.current.days == 1) SaveLoadSystem.current.NewGame();
                // else 
                SaveLoadSystem.current.OnLose();
                UpdateTexts();
                ResetVicMoney();
                PlayerDriveInput.current.GetComponent<PlayerInteraction>().ClearItems(); //yes, this is stupid
                TimeManager.current.ResetShiftTime();

                am.PlayUI(5);
                
                LeanTween.delayedCall(1f, () => {
                    Fader.current.SetText("DAY " + TimeManager.current.days);
                });

                LeanTween.delayedCall(2f, () => {
                    Fader.current.FadeFromBlack(1f, "DAY " + TimeManager.current.days, null);
                });
            });
        }
        lateFee = 0;
        failureCharge = 0;
        // PlayerDriveInput.current.isPickups = true;
        PlayerDriveInput.current.SetPickups(SaveLoadSystem.current.isPassengerPickups);
        //notif

        //Clear jeepney seats
        carCon.NewDay();
    }
}
