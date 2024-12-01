using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using TMPro;
using UnityEngine.UI;

public class CrimeManager : MonoBehaviour {
    public static CrimeManager current;
    private PlayerDriveInput pdi;
    private NotificationManager nm;
    private SpawnArea sa;
    private Settings settings;
    private BoundaryManager bm;
    private AudioManager am;
    public bool isOn;

    public List<PoliceCar> policeCars; //contains all police cars
    public int copCarRange;
    private int carsChasing;
    public List<Transform> targets;

    public bool isPlayerWanted;
    private int wantedLevel;

    //UI
    [SerializeField] private uiAnimGroup finesUI;
    [SerializeField] private TMP_Text finesText;
    [SerializeField] private uiAnimGroup breakingNews;
    // [SerializeField] private GameObject breakingNews;
    [SerializeField] private GameObject star1;
    [SerializeField] private GameObject star2;
    [SerializeField] private GameObject star3;
    [SerializeField] private GameObject star4;
    [SerializeField] private GameObject star5;

    //Aggro
    public bool isAggroLoss;
    public float aggroProgress;
    [SerializeField] private float aggroMax;
    [SerializeField] private RectTransform aggroBar;

    //Arresting
    public float arrestProgress;
    private float prevArrestProgress;
    [SerializeField] private float arrestMax;
    [SerializeField] private RectTransform arrestBar;
    [SerializeField] private uiAnimGroup arrestPanel;
    [SerializeField] private RectTransform arrestRedBG;
    private Coroutine redBlinkCoroutine;

    //Violations
    [SerializeField] private int redLightCost = 50;
    [SerializeField] private int collisionCost = 100;
    [SerializeField] private int manslaughterCost = 150;
    [SerializeField] private int illegalUnloadCost = 100;
    [SerializeField] private int briberyCost; //bribe penalty

    private int redLightCount;
    private int collisionCount;
    private int manslaughterCount;
    private int briberyCount;
    private int illegalUnloadCount;
    private int finesTotal;

    [Space]
    [Header("UI")]
    [SerializeField] private TMP_Text redLightCountText;
    [SerializeField] private TMP_Text collisionCountText;
    [SerializeField] private TMP_Text manslaughterCountText;
    [SerializeField] private TMP_Text briberyCountText;
    [SerializeField] private TMP_Text illegalUnloadCountText;
    [SerializeField] private TMP_Text redLightFinesText;
    [SerializeField] private TMP_Text collisionFinesText;
    [SerializeField] private TMP_Text manslaughterFinesText;
    [SerializeField] private TMP_Text briberyFinesText;
    [SerializeField] private TMP_Text illegalUnloadFinesText;
    [SerializeField] private TMP_Text totalText;

    [SerializeField] private TMP_Text depositText;

    [Space]
    [Header("FINES")]
    [SerializeField] private uiAnimGroup finesPanel; //Under Arrest Panel

    //Bribery
    [SerializeField] private TMP_InputField bribePriceInput;
    [SerializeField] private Button bribeButton;
    [SerializeField] private TMP_Text bribeChanceText;
    [SerializeField] private int bribePrice; //current try bribe price
    [SerializeField] private int bribePercent; //current try bribe price

    //Audio
    [SerializeField] private AudioSource audioSource;

    //Event
    public static event Action OnPlayerIsWanted;
    public static event Action OnPlayerRefueling;

    private void Awake() {
        current = this;
    }

    private void Start() {
        pdi = PlayerDriveInput.current;
        nm = NotificationManager.current;
        sa = SpawnArea.current;
        settings = Settings.current;
        bm = BoundaryManager.current;
        am = AudioManager.current;
    }

    private void Update() {
        //TEST
        // if(Input.GetKeyDown(KeyCode.Alpha0)) {
        //     NewViolation(2);
        // }

        if(!isOn) return;
        if(!isPlayerWanted) {
            if(arrestProgress > 0) arrestProgress = 0;
            if(arrestPanel.isIn) arrestPanel.Out();
            return;
        }

        //AGGRO
        if(aggroProgress > 0) {
            float aggroRatio = aggroProgress / aggroMax;
            aggroBar.localScale = new Vector3(aggroRatio, 1, 1);
        }

        //ARREST
        if(arrestProgress > 0) {
            if(!arrestPanel.isIn) arrestPanel.In();
            float arrestRatio = arrestProgress / arrestMax;
            arrestBar.localScale = new Vector3(arrestRatio, 1, 1);

            //Red blinker
            if(redBlinkCoroutine == null && arrestProgress > prevArrestProgress) {
                // print("red on");
                redBlinkCoroutine = StartCoroutine(RedBlinker());
            } else if(arrestProgress <= prevArrestProgress) {
                // print("red off");
                LeanTween.color(arrestRedBG, new Color(1, 0, 0, 0f), 0.25f);
                if(redBlinkCoroutine != null) {
                    StopCoroutine(redBlinkCoroutine);
                    redBlinkCoroutine = null;
                }
                aggroProgress --;
            }

            //set prev arrest prog
            prevArrestProgress = arrestProgress;
        } else {
            if(arrestPanel.isIn) arrestPanel.Out();

            //Stop blinking
            if(redBlinkCoroutine != null) {
                StopCoroutine(redBlinkCoroutine);
                redBlinkCoroutine = null;
                arrestRedBG.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            }

            //Keep to lose aggro when not being arrested
            aggroProgress --;
        }

        if(arrestProgress >= arrestMax) {
            arrestProgress = arrestMax;
            UnderArrest();
        }

    }

    private IEnumerator RedBlinker() {
        while(true) {
            // Fade in
            LeanTween.color(arrestRedBG, new Color(1, 0, 0, 0.39f), 0.5f);
            yield return new WaitForSeconds(0.5f); // Wait for the duration of the fade-in .setEase(LeanTweenType.easeInOutSine)

            // Fade out
            LeanTween.color(arrestRedBG, new Color(1, 0, 0, 0f), 0.5f);
            yield return new WaitForSeconds(0.5f); // Wait for the duration of the fade-out
        }
    }

    //called 50 times per second
    private void FixedUpdate() {
        if(!isOn) return;

        if(isPlayerWanted && isAggroLoss) {
            //dont lose aggro when being arrested
            // if(prevArrestProgress < arrestProgress) aggroProgress --;

            if(aggroProgress <= 0) {
                SetIsPlayerWanted(false);
            }
        }
    }

    private void LateUpdate() {
        isAggroLoss = true;
    }

    public void PlayerRefueling() {
        OnPlayerRefueling?.Invoke();
    }

    private void UnderArrest() {
        print("UNDER ARREST");
        SetIsPlayerWanted(false);
        
        //Reset values
        arrestProgress = 0;
        arrestPanel.Out();
        carsChasing = 0;

        UpdateViolationsUI();

        finesPanel.In();

        //deactivate player
        settings.ToggleCursor();
        settings.UpdateCursor();
        settings.SetPlayer(false);
        pdi.carCon.carRb.isKinematic = true;
    }

    private void UpdateViolationsUI() {
        redLightCountText.text = redLightCount.ToString();
        collisionCountText.text = collisionCount.ToString();
        manslaughterCountText.text = manslaughterCount.ToString();
        illegalUnloadCountText.text = illegalUnloadCount.ToString();
        briberyCountText.text = briberyCount.ToString();

        redLightFinesText.text = "P" + (redLightCount * redLightCost);
        collisionFinesText.text = "P" + (collisionCount * collisionCost);
        manslaughterFinesText.text = "P" + (manslaughterCount * manslaughterCost);
        illegalUnloadFinesText.text = "P" + (illegalUnloadCount * illegalUnloadCost);
        briberyFinesText.text = "P" + (briberyCount * briberyCost);
        finesTotal = redLightCount * redLightCost + collisionCount * collisionCost + manslaughterCount * manslaughterCost + illegalUnloadCount * illegalUnloadCost + briberyCount * briberyCost;
        totalText.text = "P" + finesTotal;

        depositText.text = "P" + bm.deposit;
    }

    private void ArrestFinish() {
        settings.ToggleCursor();
        settings.UpdateCursor();
        settings.SetPlayer(true);

        finesTotal = 0;
        finesPanel.Out();
        pdi.carCon.carRb.isKinematic = false;

        //Reset counts
        redLightCount = 0;
        collisionCount = 0;
        manslaughterCount = 0;
        briberyCount = 0;

        //reset bribe buttons
        bribePriceInput.text = "0";
        bribeChanceText.text = "0%";
        bribePriceInput.interactable = true;
        bribeButton.interactable = true;
    }

    public void TryPayFines() {
        // if(!isPlayerWanted) return;

        if(bm.CanPay(finesTotal)) {
            ArrestFinish();
            // TowTruck.current.PoliceTow();

            //sfx
            am.PlayUI(2);
            am.PlayUI(24);
        } else {
            nm.NewNotif("NOT ENOUGH MONEY!", "You can't afford to pay your fines. Try bribing or go to jail!");

            //sfx
            am.PlayUI(21);
        }
    }

    public void GoToJail() {
        bm.GetArrested();

        ArrestFinish();

        //sfx
        am.PlayUI(9);
        am.PlayUI(21);
    }

    public void UpdateBribe() {
        //get bribe price from input
        string bribeText = bribePriceInput.text;
        int.TryParse(bribeText, out int newBribePrice);
        bribePrice = newBribePrice;

        //correct when bribing higher than whats in deposit
        if(bribePrice > bm.deposit) bribePrice = bm.deposit;
        else if(bribePrice > finesTotal) bribePrice = finesTotal;
        bribePriceInput.text = bribePrice.ToString();

        //get ratio
        float bribeRatio = (float)bribePrice / (float)finesTotal;
        bribePercent = (int)(bribeRatio*100);
        bribeChanceText.text = bribePercent + "%";
        if(bribePercent >= 100) bribeChanceText.text = "100%";
    }

    public void TryBribe() {
        int randInt = UnityEngine.Random.Range(0, 101);
        if(randInt <= bribePercent) {
            //success
            print(Time.time + " randInt: " + randInt + "SUCCESS");
            bm.CanPay(bribePrice);

            nm.NewNotifColor("BRIBE SUCCESSFUL!", "You have successfully bribed a police officer!\n\nBRIBE ROLL: " + randInt + "\nBRIBE CHANCE: " + bribePercent + "%\nRESULT: SUCCESS!", 1);

            ArrestFinish();

            //sfx
            am.PlayUI(2);
            am.PlayUI(23);
        } else {
            //fail
            print(Time.time + "FAIL");

            //bribery
            briberyCount ++;
            briberyCost = (int)finesTotal/2;
            UpdateViolationsUI();

            nm.NewNotifColor("BRIBE UNSUCCESSFUL!", "You have failed to bribe a police officer! Your fines have been increased by 50%. You now have to pay P" + finesTotal + "\n\nBRIBE ROLL: " + randInt + "%\nBRIBE CHANCE: " + bribePercent + "%\nRESULT: FAILURE!", 3);

            //remove bribery as an option
            bribePriceInput.interactable = false;
            bribeButton.interactable = false;

            //sfx
            am.PlayUI(22);
        }
    }

    private void SetIsPlayerWanted(bool newVal) {
        if(!isOn) return;

        // print("SET IS PLAYER WANTED: " + (newVal? "TRUE":"FALSE"));
        isPlayerWanted = newVal;

        //stop spawning vics
        // sa.isSpawningVehicles = !isPlayerWanted;
        sa.currentMaxVicCount = isPlayerWanted? sa.wantedVicCount : sa.maxVicCount;

        //UI
        // finesUI.GetComponent<TMP_Text>().text = "FINES: P" + finesTotal;
        // finesText.text = "FINES: P" + finesTotal;
        // finesUI.SetActive(isPlayerWanted);
        if(isPlayerWanted) finesUI.In();
        else finesUI.Out();
        
        if(isPlayerWanted) breakingNews.In();
        else breakingNews.Out();

        //UPDATE STARS
        star1.SetActive(false);
        star2.SetActive(false);
        star3.SetActive(false);
        star4.SetActive(false);
        star5.SetActive(false);
        if(wantedLevel >= 1) star1.SetActive(isPlayerWanted);
        if(wantedLevel >= 2) star2.SetActive(isPlayerWanted);
        if(wantedLevel >= 3) star3.SetActive(isPlayerWanted);
        if(wantedLevel >= 4) star4.SetActive(isPlayerWanted);
        if(wantedLevel >= 5) star5.SetActive(isPlayerWanted);
        // print("setisPlayerWanted. wanted level: " + wantedLevel);

        //AGGRO
        aggroProgress = aggroMax;
        // if(isPlayerWanted) aggroProgress = aggroMax;

        //Audio
        if(isPlayerWanted && !audioSource.isPlaying) audioSource.Play();
        else if(!isPlayerWanted) audioSource.Stop();

        if(!isPlayerWanted) {
            wantedLevel = 0;
            finesTotal = 0;
            arrestProgress = 0;

            foreach(PoliceCar pc in policeCars) {
                pc.SetIsChasing(false);
            }
        }

        //Alert passengers
        OnPlayerIsWanted?.Invoke();
    }

    public void CheckIllegalUnloading() {
        if(pdi.GetComponent<PlayerInteraction>().isInIllegalUnloadArea) {
            print("IN ILLEGAL UNLOADING AREA");
            NewViolation(4);
        }
    }

    public void NewCopCarChasing(PoliceCar pc, bool newIsChasing) {
        // print("new cop car chasing: " + pc.name + (newIsChasing? " TRUE":" FALSE"));
        carsChasing += newIsChasing? 1 : -1;

        //give target
        if(newIsChasing) pc.target = targets[carsChasing - 1];
        // if(newIsChasing) print("NEW IS CHASING");;
    }

    public void NewViolation(int violation) {
        print("NEW VIOLATION: " + violation);
        if(!isOn || !pdi.isDriving || finesPanel.isIn) return;

        /*
        VIOLATIONS
        1 - Red light violation
        2 - Vehicle collision
        3 - Vehicular manslaughter
        4 - Illegal unloading
        */

        bool isPoliceAlert = false; //true if police car sees player commit crime

        //CHECK VIOLATIONS
        foreach(PoliceCar pc in policeCars) {
            //DETECT CRIME
            if(pc.gameObject.activeSelf && Vector3.Distance(pc.transform.position, pdi.transform.position) <= copCarRange) {
                // print("police alert!");
                if(!isPoliceAlert) isPoliceAlert = true;

                break;
            }
        }

        //VIOLATIONS AND WANTED LEVEL
        if(isPoliceAlert) {
            //Update wanted level
            //Disregard red light violation when already wanted
            // if(wantedLevel > 1 && (violation == 1 || violation == 4)) {
            //     return;
            // } else {
                if(violation == 3) wantedLevel += 2;
                else wantedLevel ++;
                if(wantedLevel > 5) wantedLevel = 5;
            // }

            //update fines
            if(violation == 1) {
                finesTotal += redLightCost;
                redLightCount ++;
            } else if(violation == 2) {
                finesTotal += collisionCost;
                collisionCount ++;
            } else if(violation == 3) {
                finesTotal += manslaughterCost;
                manslaughterCount ++;
            } else if(violation == 4) {
                finesTotal += illegalUnloadCost;
                illegalUnloadCount ++;
            }

            // if(!isPlayerWanted) 
            SetIsPlayerWanted(true);

            //update fines text
            finesText.text = "FINES: P" + finesTotal;
        }

        //CHASE
        //Make closest cars chase
        if(carsChasing < wantedLevel) {
            foreach(PoliceCar pc in policeCars) {
                if(!pc.isChasingTarget && pc.gameObject.activeSelf && Vector3.Distance(pc.transform.position, pdi.transform.position) <= copCarRange) {
                    //set this police car to chase
                    if(carsChasing < wantedLevel) pc.SetIsChasing(true);
                }
            }
        }
        //Make distant, active cars chase
        if(carsChasing < wantedLevel) {
            foreach(PoliceCar pc in policeCars) {
                if(!pc.isChasingTarget && pc.gameObject.activeSelf) {
                    //set this police car to chase
                    if(carsChasing < wantedLevel) pc.SetIsChasing(true);
                }
            }
        }
        //Spawn and make reserve cars chase
        if(carsChasing < wantedLevel) {
            foreach(PoliceCar pc in policeCars) {
                
            }
        }

        //NOTIFICATION
        if(isPoliceAlert) {
            if(violation == 1) nm.NewNotifColor("RED LIGHT VIOLATION", "A nearby police car just saw you running a red light!", 3);
            else if(violation == 2) nm.NewNotifColor("VEHICLE COLLISION", "A nearby police car just saw you colliding with a car!", 3);
            else if(violation == 3) nm.NewNotifColor("VEHICULAR MANSLAUGHTER", "A nearby police car just saw you crashing into a pedestrian!", 3);
            else if(violation == 4 && !isPlayerWanted) nm.NewNotifColor("ILLEGAL UNLOADING", "A nearby police car just saw you illegaly unloading passengers!", 3);
        } else {
            if(violation == 1) nm.NewNotifColor("RED LIGHT VIOLATION", "You just ran a red light!\n\nLuckily for you, no cop car was around to see it...", 2);
            else if(violation == 2) nm.NewNotifColor("VEHICLE COLLISION", "You have collided with a car!\n\nLuckily for you, no cop car was around to see it...", 2);
            else if(violation == 3) nm.NewNotifColor("VEHICULAR MANSLAUGHTER", "You have crashed into a pedestrian!\n\nLuckily for you, no cop car was around to see it...", 2);
            else if(violation == 4) nm.NewNotifColor("ILLEGAL UNLOADING", "You are unloading passengers illegaly!\n\nLuckily for you, there is no cop car around to see it...", 2);
        }
    }
}
