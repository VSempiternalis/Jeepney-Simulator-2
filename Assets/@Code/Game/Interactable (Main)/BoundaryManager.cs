using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class BoundaryManager : MonoBehaviour {
    public static BoundaryManager current;

    private SaveLoadSystem sls;
    private bool doBoundary;
    public int deposit;
    public int boundary;
    public int lateFee;
    public int total;
    public int shiftLength;

    [SerializeField] private int baseBoundary;
    [SerializeField] private int dayMultiplier;

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
    [SerializeField] private List<TMP_Text> totalTexts = new List<TMP_Text>();

    [SerializeField] private Transform spawnPoint;

    private void Awake() {
        current = this;
    }

    private void Start() {
        sls = SaveLoadSystem.current;
    }

    private void Update() {
        // if(Input.GetKeyDown(KeyCode.Alpha0)) Fader.current.FadeToBlack(1f, "CONGRATULATIONS! YOU MADE THE BOUNDARY!\n\nSaving game...", () => {
        //     LeanTween.delayedCall(1f, () => {
        //         Fader.current.SetText("DAY " + TimeManager.current.days);
        //     });
        //     LeanTween.delayedCall(2f, () => {
        //         Fader.current.FadeFromBlack(1f, "DAY " + TimeManager.current.days, null);
        //     });
        // });
    }

    public void Setup(string gameMode) {
        if(gameMode == "Career") doBoundary = true;

        CalculateNewBoundary();
        UpdateTexts();
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
        int shiftFactor = shiftLength * 10; //money earned per hour/realminute = 10
        int dayFactor = (TimeManager.current.days) * dayMultiplier;
        boundary = shiftFactor + dayFactor + baseBoundary;
        // print("shiftFactor: " + shiftFactor);
        // print("dayFactor: " + dayFactor);
        // print("BOUNDARY: " + boundary);
    }

    private void CalculateTotal() {
        total = deposit - (boundary + lateFee);
    }

    public void UpdateTexts() {
        total = deposit - (boundary + lateFee);

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

    public void AddToDeposit() {
        deposit += storage.value;

        // CalculateTotal();
        UpdateTexts();

        //animate
        storage.Clear();

        //audio
    }

    public void CompleteShift() {
        if(total >= 0 || !doBoundary) {
            deposit -= boundary + lateFee;
            string text = "CONGRATULATIONS! YOU MADE THE BOUNDARY!\n\nSaving game...\n";
            if(!doBoundary) text = "Saving game...";
            lateFee = 0;

            Fader.current.FadeToBlack(1f, text, () => {
                //Reset
                TimeManager.current.NewShift();
                SaveLoadSystem.current.SaveGame();

                CalculateNewBoundary();
                UpdateTexts();
                
                LeanTween.delayedCall(1f, () => {
                    Fader.current.SetText("DAY " + TimeManager.current.days);
                });

                LeanTween.delayedCall(2f, () => {
                    Fader.current.FadeFromBlack(1f, "DAY " + TimeManager.current.days, null);
                });
            });
        } else {
            Fader.current.FadeToBlack(1f, "YOU'RE FIRED!\n\nLoading previous save...\n", () => {
                //Reset
                SaveLoadSystem.current.LoadGame();
                
                LeanTween.delayedCall(1f, () => {
                    Fader.current.SetText("DAY " + TimeManager.current.days);
                });

                LeanTween.delayedCall(2f, () => {
                    Fader.current.FadeFromBlack(1f, "DAY " + TimeManager.current.days, null);
                });
            });
        }
    }
}
