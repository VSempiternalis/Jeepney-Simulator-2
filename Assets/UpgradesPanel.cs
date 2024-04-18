using System.Collections.Generic;
using System;
using UnityEngine;

public class UpgradesPanel : MonoBehaviour {
    public static UpgradesPanel current;
    private BoundaryManager bm;
    private CarController carcon;

    //upgName, price
    // [SerializeField] private Dictionary<string, int> upgradesDict = new Dictionary<string, int>();

    public List<Upgrade> upgrades;
    // public List<string> upgradesBought;
    [SerializeField] private bool allFree; //DEBUG ONLY

    [Serializable] public struct Upgrade {
        public GameObject go;
        public GameObject button;
        public string upgName;
        public int price;
        public int maxHealthAdd; //number of health this upgrade adds to jeep
        public int massAdd;
        public int damageAdd;
        public bool enablesMusic;
    }

    private void Awake() {
        current = this;
    }

    private void Start() {
        bm = BoundaryManager.current;
    }

    private void Update() {
        carcon = PlayerDriveInput.current.carCon;
    }

    public void TryBuy(string newUpgName) {
        print("BUYING " + newUpgName);
        foreach(Upgrade upgrade in upgrades) {
            if(upgrade.upgName == newUpgName) {
                print("FOUND " + newUpgName);
                // check if can afford
                if(allFree || bm.deposit >= upgrade.price) {
                    bm.AddToDeposit(-upgrade.price);
                    AudioManager.current.PlayUI(15);
                    Toggle(upgrade.upgName, true); //must be string for JeepneySLS
                } else {
                    NotificationManager.current.NewNotif("INSUFFICIENT FUNDS!", "You do not have enough money in your deposit to afford this.");
                }
            }
        }
    }

    public void Toggle(string newUpgName, bool toggleOn) { //must be string for JeepneySLS
        // print("TOGGLING " + newUpgName);
        foreach(Upgrade upgrade in upgrades) {
            if(upgrade.upgName == newUpgName) {
                //go
                upgrade.go.SetActive(toggleOn);

                //stats
                if(toggleOn) {
                    carcon.maxHealth += upgrade.maxHealthAdd;
                    carcon.AddHealth(upgrade.maxHealthAdd);
                    carcon.GetComponent<Rigidbody>().mass -= upgrade.massAdd;
                    carcon.damage += upgrade.damageAdd;

                    // upgradesBought.Add(upgrade.upgName);

                    //button
                    upgrade.button.SetActive(false);
                } else {
                    carcon.maxHealth -= upgrade.maxHealthAdd;
                    carcon.AddHealth(-upgrade.maxHealthAdd);
                    carcon.GetComponent<Rigidbody>().mass += upgrade.massAdd;
                    carcon.damage -= upgrade.damageAdd;

                    // upgradesBought.Remove(upgrade.upgName);

                    //button
                    upgrade.button.SetActive(true);
                }

                JeepneyPanel.current.UpdateReqs();

            }
        }
    }

    public void SetAsDefault() {
        // upgradesBought.Clear();

        foreach(Upgrade upgrade in upgrades) {
            if(upgrade.go.transform.GetChild(0).GetComponent<StorageHandler>()) upgrade.go.transform.GetChild(0).GetComponent<StorageHandler>().Clear();

            upgrade.go.SetActive(false);
            upgrade.button.SetActive(true);
        }
    }

    public void UpdateScreen() {

    }
}
