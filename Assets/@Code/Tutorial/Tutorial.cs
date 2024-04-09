using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Tutorial : MonoBehaviour {
    public static Tutorial current;
    [SerializeField] private PlayerInteractionTUTORIAL pit;
    [SerializeField] private PlayerDriveInputTUTORIAL pdit;
    [SerializeField] private CarController carcon;
    private RouteSelector rs;

    private int step = 0;
    private NotificationManager nm;

    // [SerializeField] private List<GameObject> stepObjects;
    [SerializeField] private Target chessTarget;
    [SerializeField] private Target settingsScreenTarget;
    [SerializeField] private Target boundaryScreenTarget;
    [SerializeField] private Target jeepneyManagementTarget;
    [SerializeField] private Target chairTarget;
    [SerializeField] private Target routeSystemTarget;
    [SerializeField] private Target driversSeatTarget;
    [SerializeField] private Target roadTarget;
    [SerializeField] private Target departuresTarget;
    [SerializeField] private Target payPointTarget;
    [SerializeField] private Target coinHoldersTarget;
    [SerializeField] private Target changePointTarget;
    [SerializeField] private Target tabletTarget;
    [SerializeField] private Target tondooTarget;
    [SerializeField] private Target factoryTarget;
    [SerializeField] private Target stopPickupsTarget;
    [SerializeField] private Target gasStationTarget;
    [SerializeField] private Target fuelPumpTarget;
    [SerializeField] private Target terminalTarget;
    [SerializeField] private Target officeTarget;
    [SerializeField] private Target boundaryTarget;
    [SerializeField] private Target payBoundaryTarget;

    [SerializeField] private GameObject inventoryUI; //2
    [SerializeField] private GameObject destinationsUI; //2
    [SerializeField] private GameObject controlsUI; //2
    [SerializeField] private GameObject dashboardUI; //2

    [SerializeField] private Transform terminalEntrance;
    [SerializeField] private Transform officeEntrance;

    private int oldPassengerCount;
    public bool refueling;
    public bool inIllegalArea;

    //Exiting
    [SerializeField] private uiAnimGroup loadingScreen;
    [SerializeField] private RectTransform loadingProgress;
    [SerializeField] private float loadTransitionTime;

    private void Awake() {
        current = this;
    }

    private void Start() {
        nm = NotificationManager.current;
        rs = RouteSelector.current;

        TimeManager.current.SetTimeTo(480);
        SpawnArea.current.maxVicCount = 25;
        SpawnArea.current.maxPersonCount = 50;

        nm.NewNotif("WELCOME TO THE TUTORIAL!", "My name is Billy, and this is the official Billy's Boundaries tutorial! Wait, you don't look excited. Do you want me to fire you?\n\n(Press [SPACE] to show excitement and not be fired)");
    }

    private void Update() {
        if(carcon.fuelAmount <= 0) {
            carcon.AddFuel(carcon.fuelCapacity);
        }
        if(carcon.health <= 50) {
            carcon.AddHealth(50);
        }

        if(step == 0 && Input.GetKeyDown(KeyCode.Space)) NextStep();
        else if(step == 1 && pit.rightHand.childCount > 0) NextStep();
        else if(step == 2 && Input.GetKeyDown(KeyCode.Space)) NextStep();
        else if(step == 3 && pit.rightHand.childCount == 10) NextStep();
        else if(step == 4 && pit.rightHand.childCount == 0) NextStep();
        else if(step == 5 && pit.rightHand.childCount == 10) NextStep();
        else if(step == 6 && pit.rightHand.childCount == 0) NextStep();
        else if(step == 7 && (Input.mouseScrollDelta.y != 0)) NextStep();
        else if(step == 8 && Input.GetKeyDown(KeyCode.Space)) NextStep();
        else if(step == 9 && Input.GetKeyDown(KeyCode.Space)) NextStep();
        else if(step == 10 && Input.GetKeyDown(KeyCode.Space)) NextStep();
        else if(step == 11 && pdit.isSitting) NextStep();
        else if(step == 12 && !pdit.isSitting) NextStep();
        else if(step == 13 && Input.GetKeyDown(KeyCode.Space)) NextStep();
        else if(step == 14 && Input.GetKeyDown(KeyCode.Space)) NextStep();
        else if(step == 15 && Input.GetKeyDown(KeyCode.Space)) NextStep();
        else if(step == 16 && Input.GetKeyDown(KeyCode.Space)) NextStep();
        else if(step == 17 && rs.destinations.Contains("Tondoo")) NextStep();
        else if(step == 18 && rs.destinations.Contains("Terminal")) NextStep();
        else if(step == 19 && Input.GetKeyDown(KeyCode.Space)) NextStep();
        else if(step == 20 && pdit.isDriving) NextStep();
        else if(step == 21 && carcon.GetComponent<Rigidbody>().velocity.magnitude > 3) NextStep();
        else if(step == 22 && Vector3.Distance(carcon.transform.position, departuresTarget.GetComponent<Transform>().position) < 5) NextStep();
        // else if(step == 19 && carcon.passengerCount > 0) NextStep();
        else if(step == 23 && carcon.payPoint.GetComponent<StorageHandler>().value > 0) NextStep();
        else if(step == 24 && pit.rightHand.childCount > 0) NextStep();
        else if(step == 25 && pit.rightHand.childCount == 0) NextStep();
        else if(step == 26 && carcon.changePoint.changees.Count > 0) NextStep();
        else if(step == 27 && carcon.changePoint.GetComponent<StorageHandler>().value > 0) NextStep();
        else if(step == 28 && Input.GetKeyDown(KeyCode.Space)) NextStep();
        else if(step == 29 && Input.GetKeyDown(KeyCode.Tab)) NextStep();
        else if(step == 30 && Input.GetKeyDown(KeyCode.Space)) NextStep();
        else if(step == 31 && Input.GetKeyDown(KeyCode.F5)) NextStep();
        else if(step == 32 && Vector3.Distance(carcon.transform.position, tondooTarget.GetComponent<Transform>().position) < 10) NextStep();
        else if(step == 33 && inIllegalArea) NextStep();
        else if(step == 34 && Input.GetKeyDown(KeyCode.Space)) NextStep();
        else if(step == 35 && !pdit.isPickups) NextStep();
        else if(step == 36 && Vector3.Distance(carcon.transform.position, gasStationTarget.GetComponent<Transform>().position) < 10) NextStep();
        // else if(step == 33 && !pdit.isDriving) NextStep();
        else if(step == 37 && refueling) NextStep();
        else if(step == 38 && Vector3.Distance(carcon.transform.position, terminalEntrance.position) < 5) NextStep();
        else if(step == 39 && Vector3.Distance(carcon.transform.position, officeEntrance.position) < 5) NextStep();
        else if(step == 40 && BoundaryManager.current.deposit > 0) NextStep(); //boundary
        else if(step == 41 && Input.GetKeyDown(KeyCode.Space)) NextStep(); //boundary
        // else if(step == 37 && Vector3.Distance(carcon.transform.position, officeEntrance.position) < 5) NextStep(); //boundary
    }

    public void NextStep() {
        step ++;

        //NOTIFICATIONS
        if(step == 1) {
            nm.NewNotif("GRABBING ITEMS", "That's the spirit! Go over to the chess board and grab a piece. You can also read their descriptions by hovering over them.\n\n(Press [LEFT CLICK] to grab an item)");
            chessTarget.enabled = true;
        }
        else if(step == 2) {
            nm.NewNotif("INVENTORY", "Good job! Notice that the items you grab will appear in your inventory on the bottom-right. Jump again to show me that you understand.");
            inventoryUI.SetActive(true);
        }
        else if(step == 3) {
            nm.Clear();
            nm.NewNotif("GRABBING MULTIPLE ITEMS", "Next up, grab as many pieces as you can.\n\n(Hold [LEFT CLICK] to grab multiple items)");
        }
        else if(step == 4) nm.NewNotif("DROPPING ITEMS", "Now, place all the pieces back on the chess board. It works the same way as grabbing.\n\n(Press/Hold [RIGHT CLICK] to drop items)");
        else if(step == 5) nm.NewNotif("GRABBING ALL ITEMS", "You can also try to grab all the items in one click.\n\n(Press [MIDDLE CLICK] on the CHESS BOARD to grab all items)");
        else if(step == 6) nm.NewNotif("DROP ALL ITEMS", "Before we continue, you should drop all the pieces again.\n\n(Press/Hold [RIGHT CLICK] to drop items)");
        else if(step == 7) {
            nm.NewNotif("ZOOMIES", "Try zooming in and out.\n\n(Use [MOUSE SCROLL] to zoom in and out)");
            jeepneyManagementTarget.enabled = false;
            chessTarget.enabled = false;
        }
        else if(step == 8) {
            nm.Clear();
            nm.NewNotif("SETTINGS SCREEN", "The SETTINGS SCREEN is where you can see details about your current game such as your shift time, passenger count, traffic count, and more.\n\nPress [SPACE] to continue");
            // Target.enabled = false;            
            settingsScreenTarget.enabled = true;
        }
        else if(step == 9) {
            nm.NewNotif("BOUNDARY SCREEN", "The BOUNDARY SCREEN is where you'll find your deposit and boundary/quota. Don't press the 'PAY BOUNDARY' button yet.\n\nPress [SPACE] to continue");
            settingsScreenTarget.enabled = false;            
            boundaryScreenTarget.enabled = true;
        }
        else if(step == 10) {
            nm.NewNotif("JEEPNEY MANAGEMENT", "Finally, you'll be able to repair, refuel, upgrade, and customize your jeepney to your heart's content on this screen.\n\nPress [SPACE] to continue");
            boundaryScreenTarget.enabled = false;            
            jeepneyManagementTarget.enabled = true;
        }
        else if(step == 11) {
            nm.Clear();
            nm.NewNotif("TAKE A REST", "You must be tired from all that! Take a seat for a second.\n\n([LEFT CLICK] on CHAIR to sit)");        
            jeepneyManagementTarget.enabled = false;
            chairTarget.enabled = true;
        }
        else if(step == 12) nm.NewNotif("NO TIME TO WASTE", "Alright, that's enough! Before you can exit the office and do your job, you need to set your route first!\n\n([LEFT CLICK] on CHAIR to stand)");
        else if(step == 13) {
            nm.NewNotif("ROUTE SYSTEM", "Go the Route System screen. This is a very important part of your work, so I need you to show me you're excited again!\n\n(Press [SPACE] to continue)");
            chairTarget.enabled = false;
            routeSystemTarget.enabled = true;
        } 
        else if(step == 14) {
            nm.Clear();
            nm.NewNotif("MANELLA CITY LANDMARKS", "This screen is a map of Manella City and its various landmarks. These are the places you'll take your passengers to.\n\n(Press [SPACE] to continue)");
        }
        else if(step == 15) nm.NewNotif("LANDMARK TOGGLING", "Since there are a lot of landmarks, you can turn them ON or OFF by clicking on them. Passengers will not enter your jeep if their destination if OFF (Red)\n\n(Press [SPACE] to continue)");
        else if(step == 16) nm.NewNotif("LOCKED LANDMARKS", "For every shift, you will be given a number of LOCKED landmarks (green). These cannot be turned off.\n\n(Press [SPACE] to continue)");
        else if(step == 17) nm.NewNotif("TONDOO", "For this tutorial, you'll only have FACTORY as the LOCKED landmark. And since TONDOO is on the way there, turn it on so you can unload passengers along the way.\n\n([LEFT CLICK] 'TONDOO' to turn ON)");
        else if(step == 18) nm.NewNotif("TERMINAL", "At the end of your shift, you need to go back to the TERMINAL to pay your boundary. So you may as well turn it on so you can unload here too.\n\n([LEFT CLICK] 'TERMINAL' to turn ON)");
        else if(step == 19) {
            nm.Clear();
            nm.NewNotif("ROUTE SYSTEM END", "That's all you need to know about the ROUTE SYSTEM. Got it? Well, if not, you can always check the HELP manual in the settings.\n\n(Press [ESCAPE] to open settings)\n\n(Press [SPACE] to continue)");
        }
        else if(step == 20) {
            nm.NewNotif("JEEPNEY TIME", "You're doing great so far! Now, go to your jeepney and take the DRIVER'S SEAT. Remember, I'll be watching you.");
            routeSystemTarget.enabled = false;
            driversSeatTarget.enabled = true;
        }
        else if(step == 21) {
            nm.NewNotif("GET PASSENGERS", "Awesome! Drive forward, turn left, and follow the road, TRY NOT TO KILL ANYONE!\n\n(Notice the controls in the botttom-left)\n\n(Use [WASD] keys to drive)");
            driversSeatTarget.enabled = false;
            roadTarget.enabled = true;
            controlsUI.SetActive(true);
        }
        else if(step == 22) {
            nm.Clear();
            nm.NewNotif("DRIVE", "Stop at the DEPARTURES area and wait for people to enter your jeepney.\n\n(Press [SPACE] to brake)");
            roadTarget.enabled = false;
            departuresTarget.enabled = true;
        } 
        else if(step == 23) {
            nm.NewNotif("WAIT FOR PAYMENT", "While waiting for someone to pay, take a look at the DASHBOARD on the bottom of the screen and the DESTINATIONS list in the top-right.");
            dashboardUI.SetActive(true);
            destinationsUI.SetActive(true);
        } 
        else if(step == 24) {
            nm.NewNotif("TAKE PAYMENT", "Someone just paid their fare! You can see how much they paid on the right side of the screen. Press [F] to grab it.");
            departuresTarget.enabled = false;
            payPointTarget.enabled = true;
        }
        else if(step == 25) {
            nm.Clear();
            nm.NewNotif("STORE PAYMENT", "Place your hard-earned income in one of the coin holders on your dashboard.\n\n([RIGHT CLICK] to place items)");
            payPointTarget.enabled = false;
            coinHoldersTarget.enabled = true;
        }
        else if(step == 26) {
            nm.NewNotif("WAIT FOR CHANGE", "Good job! Let's wait for someone who needs change.");
            coinHoldersTarget.enabled = false;
        }
        else if(step == 27) {
            nm.NewNotif("A LITTLE CHANGE", "Someone needs change! Grab money from the coin holders and press [R] to give it to them. You can see how much they need on the right side of the screen.\n\n([LEFT CLICK] to grab items)");
            changePointTarget.enabled = true;
        }
        else if(step == 28) {
            nm.Clear();
            nm.NewNotif("ON TO TONDOO", "Awesome work! Now take more passengers and drive to TONDOO while taking payments and giving change!\n\n(Press [SPACE])");
            changePointTarget.enabled = false;
            oldPassengerCount = carcon.passengerCount;
        }
        else if(step == 29) {
            nm.NewNotif("TABLET MAP", "Check the tablet map in front of you to know where you're going. Press the plus and minus buttons to zoom in and out.\n\n(Press [TAB] to bring the tablet closer)");
            tabletTarget.enabled = true;
        }
        else if(step == 30) {
            nm.NewNotif("UNLOADING ZONES", "The green squares on the map's roads are UNLOADING ZONES. Stop here to unload passengers.\n\n(Press [SPACE])");
        }
        else if(step == 31) {
            nm.Clear();
            nm.NewNotif("CAMERA ANGLES", "You can also change your camera angles.\n\n(Press [F1-F5])");
            tabletTarget.enabled = false;
        }
        else if(step == 32) {
            nm.NewNotif("UNLOAD PASSENGERS AT TONDOO", "Stop at the designated UNLOADING ZONE. You can also identify them from the bright green lights and signs near the road.\n\nOnce there, you can also pick up more passengers.");
            tondooTarget.enabled = true;
        }
        else if(step == 33) {
            nm.NewNotif("QUICKLY TO THE FACTORY", "You're a natural! Now, take the rest of your passengers to the FACTORY.");
            tondooTarget.enabled = false;
            factoryTarget.enabled = true;
        }
        else if(step == 34) {
            nm.Clear();
            nm.NewNotif("ILLEGAL UNLOADING ZONE!", "Make sure there are no police cars nearby when you stop at ILLEGAL UNLOADING ZONES.\n\n(Press [SPACE])");
            factoryTarget.enabled = false;
        }
        else if(step == 35) {
            nm.NewNotif("STOP PASSENGER PICKUPS", "Now press the 'STOP PASSENGER PICKUPS' button on your tablet to stop picking up passengers. This cannot be undone until you start your next shift.");
            stopPickupsTarget.enabled = true;
        }
        else if(step == 36) {
            nm.Clear();
            nm.NewNotif("GAS STATION", "Go to a nearby GAS STATION (you can find them on the map), and refill your fuel tank.\n\nOnce you're there, stop near one of the fuel pumps and exit the jeepney.");
            stopPickupsTarget.enabled = false;
            gasStationTarget.enabled = true;
        }
        else if(step == 37) {
            nm.Clear();
            nm.NewNotif("FUEL PUMP", "Deposit some money in the fuel pump, press the shiny red button, and wait as you refuel.\n\nRemember that you cannot drive while refueling. If you need change, you can also visit ALBERTO at the EZ EXCHANGER.");
            gasStationTarget.enabled = false;
            fuelPumpTarget.enabled = true;
        }
        else if(step == 38) {
            nm.NewNotif("BACK TO TERMINAL", "Looking good! Once you're done refueling, go back to the TERMINAL. If you want, you can also unload passengers at the FACTORY and TONDOO.");
            fuelPumpTarget.enabled = false;
            terminalTarget.enabled = true;
        }
        else if(step == 39) {
            nm.NewNotif("BILLY'S OFFICE", "Turn right from here to go to the ARRIVALS area. Drop-off any passengers you might have, then stop in front of my office.");
            terminalTarget.enabled = false;
            officeTarget.enabled = true;
        }
        else if(step == 40) {
            nm.Clear();
            nm.NewNotif("DEPOSIT INCOME", "Grab your earnings as you can and deposit them in my office. Don't forget to turn your engine off.\n\n([RIGHT CLICK] on boundary mat to deposit money)");
            officeTarget.enabled = false;
            boundaryTarget.enabled = true;
        }
        else if(step == 41) {
            nm.NewNotif("FEES AND CHARGES", "Remember that in the real thing, there is an extra fee for being late and for having undelivered passengers.\n\n(Press [SPACE])");
        }
        else if(step == 42) {
            nm.Clear();
            nm.NewNotif("PAY BOUNDARY", "Once you've deposited enough money to pay the boundary, press the 'PAY BOUNDARY' button. It's okay if you didn't make enough money for the boundary since you're still training.\n\nBut your work also starts tomorrow, so you know... git gud.\n\n([LEFT CLICK] on 'PAY BOUNDARY' button to finish tutorial and return to MAIN MENU)");
            boundaryTarget.enabled = false;
            payBoundaryTarget.enabled = true;
        }

        //STEP OBJECTS
        // foreach(GameObject stepObject in stepObjects) {
        //     if(stepObject != null) stepObject.SetActive(false);
        // }
        // if(stepObjects[step] != null) stepObjects[step].SetActive(true);
    }

    public void FinishTutorial() {
        string text = "CONGRATULATIONS! YOU FINISHED THE TUTORIAL!\n\nLoading Main Menu...\n";
        Fader.current.FadeToBlack(1f, text, () => {
            Settings.current.ToggleSettings();
            ExitToMain();
        });
    }

    public void InIllegalArea() {
        inIllegalArea = true;
    }

    public void Refueling() {
        refueling = true;
    }

    public void ExitToMain() {
        StartCoroutine(ExitToMainMenu());
    }

    IEnumerator ExitToMainMenu() {
        loadingScreen.In();
        loadingScreen.GetComponent<CanvasGroup>().blocksRaycasts = true;

        yield return new WaitForSeconds(loadTransitionTime);

        // SceneManager.LoadScene("SCENE - Game");

        AsyncOperation operation = SceneManager.LoadSceneAsync("SCENE - MainMenu");

        //LOADING BAR
        while(!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress/0.9f);
            print(progress);
            loadingProgress.LeanScaleX(progress, 1f);
            yield return null;
        }
    }
}
