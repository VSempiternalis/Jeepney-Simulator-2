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
    [SerializeField] private bool allFree; //DEBUG ONLY

    //SEATS
    [SerializeField] private List<Transform> seats1;
    [SerializeField] private List<Transform> seats2;
    public bool seats1Bought;
    public bool seats2Bought;

    [Serializable] public struct Upgrade {
        public GameObject go;
        public GameObject button;
        public string upgName;
        public int price;
        public int maxHealthAdd; //number of health this upgrade adds to jeep
        public int massAdd;
        public int damageAdd;
        public bool enablesMusic;
        public int extraSeats;
    }

    private void Awake() {
        current = this;
    }

    private void Start() {
        bm = BoundaryManager.current;
    }

    private void Update() {
        if(carcon == null) carcon = PlayerDriveInput.current.carCon;
    }

    public void TryBuy(string newUpgName) {
        print("BUYING " + newUpgName);
        foreach(Upgrade upgrade in upgrades) {
            if(upgrade.upgName == newUpgName) {
                // print("FOUND " + newUpgName);
                // check if can afford
                if(allFree || bm.deposit >= upgrade.price) {
                    bm.AddToDeposit(-upgrade.price);
                    AudioManager.current.PlayUI(15);
                    Toggle(upgrade.upgName, true); //must be string for JeepneySLS

                    //STEAM ACH
                    SteamAchievements.current.UnlockAchievement("ACH_STARTING_TO_SHINE");
                } else {
                    NotificationManager.current.NewNotifColor("INSUFFICIENT FUNDS!", "You do not have enough money in your deposit to afford this.", 2);
                }
            }
        }
    }

    public void Toggle(string newUpgName, bool toggleOn) { //must be string for JeepneySLS
        print("TOGGLING " + newUpgName);
        //STEAM ACH
        bool isPatokJeepney = true;
        foreach(Upgrade upgrade in upgrades) {
            print("UPGRADE: " + upgrade.upgName + " / " + newUpgName);
            if(upgrade.upgName == newUpgName) {
                print("FOUND: " + newUpgName);
                //go
                upgrade.go.SetActive(toggleOn);

                //stats
                if(toggleOn) {
                    print("TOGGLED ON: " + newUpgName);
                    carcon.maxHealth += upgrade.maxHealthAdd;
                    carcon.AddHealth(upgrade.maxHealthAdd);
                    carcon.GetComponent<Rigidbody>().mass -= upgrade.massAdd;
                    carcon.damage += upgrade.damageAdd;

                    //music player
                    if(upgrade.enablesMusic) {
                        carcon.mp.SetActive(true);

                        //STEAM ACH
                        SteamAchievements.current.UnlockAchievement("ACH_TURN_UP_THE_MUSIC");
                    }

                    //SEATS
                    if(upgrade.extraSeats == 1) {
                        print("SEATS 1");
                        foreach(Transform seatspot in seats1) {
                            carcon.seatSpots.Add(seatspot);
                            seats1Bought = true;
                        }
                    } else if(upgrade.extraSeats == 2) {
                        print("SEATS 2");
                        foreach(Transform seatspot in seats2) {
                            carcon.seatSpots.Add(seatspot);
                            seats2Bought = true;
                        }
                    }

                    //button
                    upgrade.button.SetActive(false);
                } else {
                    print("TOGGLED OFF: " + newUpgName);
                    carcon.maxHealth -= upgrade.maxHealthAdd;
                    carcon.AddHealth(-upgrade.maxHealthAdd);
                    carcon.GetComponent<Rigidbody>().mass += upgrade.massAdd;
                    carcon.damage -= upgrade.damageAdd;

                    //music player
                    // carcon.mp.SetActive(false);

                    //button
                    upgrade.button.SetActive(true);

                    //SEATS
                    if(upgrade.extraSeats == 2) {
                        print("SEATS 2");
                        foreach(Transform seatspot in seats2) {
                            if(carcon.seatSpots.Contains(seatspot)) {
                                carcon.seatSpots.Remove(seatspot);
                                seats2Bought = false;
                            }
                        }
                    } else if(upgrade.extraSeats == 1) {
                        print("SEATS 1");
                        foreach(Transform seatspot in seats1) {
                            if(carcon.seatSpots.Contains(seatspot)) {
                                carcon.seatSpots.Remove(seatspot);
                                seats1Bought = false;
                            }
                        }
                    }
                }

                JeepneyPanel.current.UpdateReqs();

            }
            if(!upgrade.go.activeSelf) isPatokJeepney = false;
        }

        if(isPatokJeepney) SteamAchievements.current.UnlockAchievement("ACH_PATOK_JEEPNEY");
    }

    public void SetAsDefault() {
        foreach(Upgrade upgrade in upgrades) {
            if(upgrade.go.transform.childCount > 0 && upgrade.go.transform.GetChild(0).GetComponent<StorageHandler>()) upgrade.go.transform.GetChild(0).GetComponent<StorageHandler>().Clear();

            if(!upgrade.go.name.Contains("Seatspot")) upgrade.go.SetActive(false);
            upgrade.button.SetActive(true);

            foreach(Transform seatspot in seats1) {
                carcon.seatSpots.Remove(seatspot);
                seats1Bought = false;
            }
            foreach(Transform seatspot in seats2) {
                carcon.seatSpots.Remove(seatspot);
                seats2Bought = false;
            }
        }
                    
        carcon.mp.SetActive(false);
    }

    public void UpdateScreen() {

    }
}
