using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class JeepneyPanel : MonoBehaviour {
    public static JeepneyPanel current;
    private BoundaryManager bm;
    private CarController carcon;
    
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject noJeepneyDetected;
    // public bool isFreeride;

    [Header("HEALTH")]
    private int health;
    private int maxHealth;
    private int missingHealth;
    [SerializeField] private float pesoPerHealth;
    private int repairCost;
    // [SerializeField] private TMP_Text repairButtonText;
    // [SerializeField] private TMP_Text healthText;
    // [SerializeField] private Transform healthBar;
    [SerializeField] private List<TMP_Text> repairButtonTexts;
    [SerializeField] private List<TMP_Text> healthTexts;
    [SerializeField] private List<Transform> healthBars;

    [Header("HEALTH UPGRADE")]
    [SerializeField] private int maxHealthUpg; //health upgrades stop when this value is reached
    [SerializeField] private int maxHealthUpgCost;
    [SerializeField] private int maxHealthUpgAdd; //max health added with each upgrade

    [Header("FUEL")]
    private int fuel;
    private int fuelCap;
    private int missingFuel;
    private int refuelCost;
    [SerializeField] private TMP_Text refuelButtonText;
    [SerializeField] private TMP_Text fuelText;
    [SerializeField] private Transform fuelBar;
    [SerializeField] private List<TMP_Text> refuelButtonTexts;
    [SerializeField] private List<TMP_Text> fuelTexts;
    [SerializeField] private List<Transform> fuelBars;

    [Header("FUEL CAPACITY")]
    [SerializeField] private int fuelCapUpgAdd;
    [SerializeField] private int maxFuelCap;
    [SerializeField] private int fuelCapUpgCost;
    [SerializeField] private TMP_Text fuelCapUpgButtonText;
    [SerializeField] private List<TMP_Text> fuelCapUpgButtonTexts;

    [Header("FUEL EFFICIENCY")]
    private int eff;
    [SerializeField] private int effAdd;
    [SerializeField] private int maxEff;
    [SerializeField] private int effCost;
    [SerializeField] private TMP_Text effButtonText;
    [SerializeField] private TMP_Text effText;
    [SerializeField] private Transform effBar;

    [Header("GEAR")]
    [SerializeField] private int maxGear;
    [SerializeField] private int gearUpgCost;
    [SerializeField] private TMP_Text maxGearButtonText;
    [SerializeField] private TMP_Text gearText;
    [SerializeField] private Transform maxGearBar;
    [SerializeField] private List<TMP_Text> maxGearButtonTexts;
    [SerializeField] private List<TMP_Text> gearTexts;
    [SerializeField] private List<Transform> maxGearBars;

    private void Awake() {
        current = this;
    }

    private void Start() {
        bm = BoundaryManager.current;
    }

    public void Setup() {
        SetCarcon(PlayerDriveInput.current.carCon);

        UpdateReqs();
    }

    public void SetCarcon(CarController newCarcon) {
        carcon = newCarcon;

        // if(SaveLoadSystem.current.gameMode == "Freeride") SetMaxGear(9);
    }

    public void UpdateReqs() {
        // if(carcon == null) {
            // noJeepneyDetected.SetActive(true);
            // main.SetActive(false);
        // } else {
            // noJeepneyDetected.SetActive(false);
            // main.SetActive(true);

        //Health
        health = Mathf.RoundToInt(carcon.health);
        maxHealth = Mathf.RoundToInt(carcon.maxHealth);
        missingHealth = maxHealth - health;
        repairCost = Mathf.RoundToInt(missingHealth * pesoPerHealth);
        // repairButtonText.text = "REPAIR - P" + repairCost;
        // healthText.text = health + "/" + maxHealth;
        // UpdateBar(healthBar, health, maxHealth);
        foreach(TMP_Text text in repairButtonTexts) {
            text.text = "REPAIR - P" + repairCost;
        }
        foreach(TMP_Text text in healthTexts) {
            text.text = health + "/" + maxHealth;
        }
        foreach(Transform bar in healthBars) {
            UpdateBar(bar, health, maxHealth);
        }

        //Fuel
        fuel = Mathf.RoundToInt(carcon.fuelAmount);
        fuelCap = Mathf.RoundToInt(carcon.fuelCapacity);
        missingFuel = fuelCap - fuel;
        refuelCost = Mathf.RoundToInt((missingFuel/1000) * GameManager.current.pricePerLiter);
        // refuelButtonText.text = "REFUEL - P" + refuelCost;
        // fuelText.text = (fuel/1000) + "L/" + (fuelCap/1000) + "L";
        // UpdateBar(fuelBar, fuel, fuelCap);
        foreach(TMP_Text text in refuelButtonTexts) {
            text.text = "REFUEL - P" + refuelCost;
        }
        foreach(TMP_Text text in fuelTexts) {
            text.text = (fuel/1000) + "L/" + (fuelCap/1000) + "L";
        }
        foreach(Transform bar in fuelBars) {
            UpdateBar(bar, fuel, fuelCap);
        }

        //Fuel capacity
        // fuelCapUpgButtonText.text = "UPGRADE - P" + fuelCapUpgCost;
        foreach(TMP_Text text in fuelCapUpgButtonTexts) {
            text.text = "UPGRADE - P" + fuelCapUpgCost;
        }

        //Efficiency
        eff = carcon.fuelLoss;
        effButtonText.text = "UPGRADE - P" + effCost;
        effText.text = (eff * 10) + "% Fuel Loss";
        UpdateBar(effBar, eff, maxEff);

        //Gear
        int carMaxGear = carcon.maxGear;
        // maxGearButtonText.text = "UPGRADE - P" + gearUpgCost;
        // gearText.text = (carMaxGear-1) + "/" + (maxGear-1);
        // UpdateBar(maxGearBar, carMaxGear-2, maxGear-2);
        foreach(TMP_Text text in maxGearButtonTexts) {
            text.text = "UPGRADE - P" + gearUpgCost;
        }
        foreach(TMP_Text text in gearTexts) {
            text.text = (carMaxGear-1) + "/" + (maxGear-1);
        }
        foreach(Transform bar in maxGearBars) {
            UpdateBar(bar, carMaxGear-2, maxGear-2);
        }
        // }
    }

    private void UpdateBar(Transform bar, float value, float maxValue) {
        float ratio = value/maxValue;
        if(float.IsNaN(ratio)) ratio = 0.01f;
        bar.localScale = new Vector3(ratio, 1, 1);
        bar.localPosition = new Vector3(-((1-ratio)/2), 0, -0.0001f);
    }

    // CONDITION ================================================================

    public void Repair() {
        if(carcon.health == carcon.maxHealth) {
            NotificationManager.current.NewNotif("HEALTH FULL", "The vehicle cannot be repaired any further");
            return;
        }

        if(bm.deposit >= repairCost) {
            carcon.AddHealth(missingHealth);
            Purchase(repairCost, 16);
        } else Fail();
    }

    public void UpgradeMaxHealth() {
        if(carcon.maxHealth == maxHealthUpg) {
            NotificationManager.current.NewNotif("MAX HEALTH REACHED", "Your vehicle's max health cannot be upgraded any further");
            return;
        }

        if(bm.deposit >= maxHealthUpgCost) {
            carcon.maxHealth += maxHealthUpgAdd;
            carcon.AddHealth(maxHealthUpgAdd);
            Purchase(maxHealthUpgCost, 18);
        } else Fail();
    }

    // FUEL ================================================================

    public void Refuel() {
        if(carcon.fuelAmount == carcon.fuelCapacity) {
            NotificationManager.current.NewNotif("FUEL TANK FULL", "The fuel tank cannot be filled any further");
            return;
        }

        if(bm.deposit >= refuelCost) {
            carcon.AddFuel(missingFuel);
            Purchase(refuelCost, 19);
        } else Fail();
    }

    public void UpgradeFuelCap() {
        print("UPGRADING FUEL CAP: " + fuelCap + " / " + maxFuelCap);
        if(fuelCap >= maxFuelCap) {
            NotificationManager.current.NewNotif("MAX FUEL CAPACITY REACHED", "The fuel capacity cannot be upgraded any further");
            return;
        }

        if(bm.deposit >= fuelCapUpgCost) {
            carcon.fuelCapacity += fuelCapUpgAdd;
            carcon.AddFuel(fuelCapUpgAdd);

            Purchase(fuelCapUpgCost, 18);
        } else Fail();
    }

    public void UpgradeEfficiency() {
        if(eff == maxEff) {
            NotificationManager.current.NewNotif("MAX EFFICIENCY REACHED", "The fuel efficiency cannot be upgraded any further");
            return;
        }

        if(bm.deposit >= effCost) {
            carcon.fuelLoss -= effAdd;

            Purchase(effCost, 18);
        } else Fail();
    }

    #region OTHERS ==================================================

    public void UpgradeMaxGear() {
        print("UPGRADING MAX GEAR");
        if(carcon.maxGear == maxGear) {
            NotificationManager.current.NewNotif("MAXIMUM GEAR REACHED", "Your vehicle's gear cannot be upgraded any further");
            return;
        }

        if(bm.deposit >= gearUpgCost) {
            // carcon.maxGear ++;
            carcon.SetMaxGear(carcon.maxGear + 1);
            Purchase(gearUpgCost, 18);
        } else Fail();
    }

    #endregion
    #region REPETITIVE ==================================================

    private void Purchase(int cost, int audioIndex) {
        bm.AddToDeposit(-cost);
        UpdateReqs();

        //audio
        // AudioManager.current.PlayUI(2);
        AudioManager.current.PlayUI(audioIndex);
    }

    private void Fail() {
        NotificationManager.current.NewNotifColor("INSUFFICIENT FUNDS!", "You do not have enough money in your deposit to afford this.", 2);
    }

    #endregion

    // public void SetMaxGear(int i) {
    //     carcon.maxGear = i;
    //     UpdateReqs();
    // }
}
