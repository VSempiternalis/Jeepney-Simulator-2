using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class LotteryManager : MonoBehaviour {
    public static LotteryManager current;

    private List<int> bronzeNums = new List<int>();
    private List<int> silverNums = new List<int>();
    private List<int> goldNums = new List<int>();
    private List<int> allNums = new List<int>();

    [SerializeField] private int bronzePrize;
    [SerializeField] private int silverPrize;
    [SerializeField] private int goldPrize;

    [SerializeField] private StorageHandler ticketStorage;
    [SerializeField] private GameObject ticketPrinter;

    private TimeManager tm;
    private float secsPerRealMinute;

    private bool bronzeAnnounced;
    private bool silverAnnounced;
    private bool goldAnnounced;

    public List<GameObject> tickets;

    [SerializeField] private TMP_Text bronze1Text;
    [SerializeField] private TMP_Text bronze2Text;
    [SerializeField] private TMP_Text bronze3Text;
    [SerializeField] private TMP_Text silver1Text;
    [SerializeField] private TMP_Text silver2Text;
    [SerializeField] private TMP_Text gold1Text;

    private void Awake() {
        current = this;
    }

    private void Start() {
        tm = TimeManager.current;
        secsPerRealMinute = tm.secsPerRealMinute;

        NewNums();
    }

    private void Update() {
        StartCoroutine(CheckShiftTime());
    }

    //Runs every second
    private IEnumerator CheckShiftTime() {
        while(true) {
            yield return new WaitForSeconds(secsPerRealMinute);

            //9 mins left: remove ticket printer + announce bronze nums
            // if(tm.shiftTimeLeft < 540 && !bronzeAnnounced) {
            //     // NotificationManager.current.NewNotif("BRONZE WINNERS!", "The Luto BRONZE PRIZE numbers are the following: " + bronzeNums[0] + ", " + bronzeNums[1] + ", and " + bronzeNums[2] + "\n\nIf you have winning numbers, please proceed to the GCSO building in BBC to claim your P400 reward.\n\nCONGRATULATIONS!");
            //     // ticketPrinter.SetActive(false);
            //     bronzeAnnounced = true;
            // } //7 mins left: announce silver nums
            // else 
            if(tm.shiftTimeLeft < 540 && !bronzeAnnounced) {
                //SET TEXTS IN GCSO building
                bronze1Text.text = bronzeNums[0].ToString();
                bronze2Text.text = bronzeNums[1].ToString();
                bronze3Text.text = bronzeNums[2].ToString();
                silver1Text.text = silverNums[0].ToString();
                silver2Text.text = silverNums[1].ToString();
                gold1Text.text = goldNums[0].ToString();

                // NotificationManager.current.NewNotif("SILVER WINNERS!", "The Luto SILVER PRIZE numbers are the following: " + silverNums[0] + " and " + silverNums[1] + "\n\nIf you have winning numbers, please proceed to the GCSO building in BBC to claim your P750 reward.\n\nCONGRATULATIONS!");
                // silverAnnounced = true;

                NotificationManager.current.NewNotifColor("BRONZE PRIZE WINNERS!", "The Luto BRONZE PRIZE numbers are in! They are: " +
                bronzeNums[0] + ", " + bronzeNums[1] + ", and " + bronzeNums[2] + "!\n\nPRIZE: P40,000", 1);
                // "\n\nSILVER:" + silverNums[0] + " and " + silverNums[1] +
                // "\n\nGOLD:" + goldNums[0] + 
                // "\n\nCONGRATULATIONS To the winners! You may now proceed to the GCSO building in BBC to claim your P400 prize!");
                
                // ticketPrinter.SetActive(false);
                ticketStorage.gameObject.SetActive(true);
                bronzeAnnounced = true;
            } else if(tm.shiftTimeLeft < 530 && !silverAnnounced) {
                NotificationManager.current.NewNotifColor("SILVER PRIZE WINNERS!", "The Luto SILVER PRIZE numbers are in! They are: " +
                silverNums[0] + " and " + silverNums[1] + "!\n\nPRIZE: P75,000", 1);
                // "\n\nSILVER:" + silverNums[0] + " and " + silverNums[1] +
                // "\n\nGOLD:" + goldNums[0] + 
                // "\n\nCONGRATULATIONS To the winners! You may now proceed to the GCSO building in BBC to claim your P750 prize!");
                
                silverAnnounced = true;
            } else if(tm.shiftTimeLeft < 520 && !goldAnnounced) {
                NotificationManager.current.NewNotifColor("GOLD PRIZE WINNERS!", "And finally, the Luto GOLD JACKPOT numbers is...\n\n" +
                goldNums[0] + "!\n\nPRIZE: P200,000" +
                // "\n\nSILVER:" + silverNums[0] + " and " + silverNums[1] +
                // "\n\nGOLD:" + goldNums[0] + 
                "\n\nCONGRATULATIONS To the winners! You may now proceed to the GCSO building in BBC to claim your rewards!", 1);
                
                goldAnnounced = true;
            }
            
            //5 mins left: announce gold nums
            // else if(tm.shiftTimeLeft < 300 && !goldAnnounced) {
            //     // NotificationManager.current.NewNotif("GOLD WINNERS!", "And finally... the Luto GOLD JACKPOT number is: " + goldNums[0] + "!\n\nIf you have winning numbers, please proceed to the GCSO building in BBC to claim your P2000 reward.\n\nCONGRATULATIONS!");
            //     goldAnnounced = true;
            // }
        }
    }

    public void CheckTicketStorage() {
        // List<GameObject> removeList = new List<GameObject>();

        foreach(GameObject item in ticketStorage.items) {
            print("checking: " + item.name);
            if(item.GetComponent<LotteryTicket>()) {
                foreach(int num in allNums) {
                    string ticketNum = item.GetComponent<LotteryTicket>().GetNum();
                    string stringNum = num.ToString();

                    if(num < 10) {
                        stringNum = "0" + num;
                    }

                    if(stringNum == ticketNum) {
                        print("WINNING NUM: " + ticketNum);

                        if(bronzeNums.Contains(num)) {
                            print("BRONZE");
                            // removeList.Add(item);
                            BoundaryManager.current.AddToDeposit(bronzePrize);
                            NotificationManager.current.NewNotifColor("BRONZE PRIZE!", "CONGRATULATIONS! You have won the BRONZE PRIZE of P40,000 and after the 99% tax rate, P400 has been added to your deposit!\n\nDEPOSIT: " + BoundaryManager.current.deposit, 1);
                        }
                        else if(silverNums.Contains(num)) {
                            print("SILVER");
                            // removeList.Add(item);
                            BoundaryManager.current.AddToDeposit(silverPrize);
                            NotificationManager.current.NewNotifColor("SILVER PRIZE!", "CONGRATULATIONS! You have won the SILVER PRIZE of P75,000 and after the 99% tax rate, P750 has been added to your deposit!\n\nDEPOSIT: " + BoundaryManager.current.deposit, 1);
                        }
                        else if(goldNums.Contains(num)) {
                            print("GOLD");
                            // removeList.Add(item);
                            BoundaryManager.current.AddToDeposit(goldPrize);
                            NotificationManager.current.NewNotifColor("GOLD PRIZE!", "CONGRATULATIONS! You have won the GOLD PRIZE of P200,000 and after the 99% tax rate, P2,000 has been added to your deposit!\n\nDEPOSIT: " + BoundaryManager.current.deposit, 1);
                        }
                        else print("ERROR!");
                    }
                }
            }

            tickets.Remove(item);
        }
        ticketStorage.Clear();

        // foreach(GameObject item in removeList) {
        //     ticketStorage.DestroyItem(item);
        // }
    }

    public void NewNums() {
        bronzeNums.Clear();
        silverNums.Clear();
        goldNums.Clear();
        allNums.Clear();

        bronzeAnnounced = false;
        silverAnnounced = false;
        goldAnnounced = false;

        ticketPrinter.SetActive(true);
        ticketStorage.gameObject.SetActive(false);

        //RESET TEXTS IN GCSO building
        bronze1Text.text = "?";
        bronze2Text.text = "?";
        bronze3Text.text = "?";
        silver1Text.text = "?";
        silver2Text.text = "?";
        gold1Text.text = "?";

        //remove all previous tickets
        foreach(GameObject ticket in tickets) {
            ticket.GetComponent<ItemHandler>().TakeItem(transform);
            Destroy(ticket);
        }
        tickets.Clear();

        PlayerDriveInput.current.GetComponent<PlayerInteraction>().UpdateOnhandUI();

        while(true) {
            int rNum = Random.Range(0, 100);

            if(!bronzeNums.Contains(rNum) && !silverNums.Contains(rNum) && !silverNums.Contains(rNum)) {
                if(bronzeNums.Count < 3) {
                    bronzeNums.Add(rNum);
                    allNums.Add(rNum);
                }
                else if(silverNums.Count < 2) {
                    silverNums.Add(rNum);
                    allNums.Add(rNum);
                }
                else if(goldNums.Count < 1) {
                    goldNums.Add(rNum);
                    allNums.Add(rNum);
                }
                else break;
            }
        }
    }
}
