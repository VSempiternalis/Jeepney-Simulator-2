using UnityEngine;
using UnityEngine.Animations.Rigging;
using TMPro;

public class PlayerDriveInput : MonoBehaviour {
    public static PlayerDriveInput current;
    public CarController carCon;
    [SerializeField] private Head head;
    [SerializeField] private FirstPersonMovement fpm;
    [SerializeField] private Jump jump;
    [SerializeField] private Crouch crouch;
    // [SerializeField] private Head head;

    public bool isSitting;
    public bool isDriving;
    // public bool isTakingPassengers; //settings
    public bool isPickups; //in-game
    [SerializeField] private GameObject pickupsOffSign;

    [SerializeField] private GameObject fpa; //firstpersonaudio

    [Space(10)]
    [Header("SEATING")]
    [SerializeField] private Transform player;
    [SerializeField] private PlayerInteraction pi;
    [SerializeField] private Transform playerModel;
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    [SerializeField] private TwoBoneIKConstraint rightHandIK;
    [SerializeField] private Vector3 localDrivePos;
    [SerializeField] private Vector3 localSitPos;

    [Space(10)]
    [Header("Keybinds")]
    private KeyCode Key_GiveChange;
    private KeyCode Key_ChangerScrollUp;
    private KeyCode Key_ChangerScrollDown;
    private KeyCode Key_TakePayment;

    [Space(10)]
    [Header("NAME")]
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private GameObject nameNoJeepneyDetected;

    private void Awake() {
        current = this;
    }

    private void Start() {
        pi = GetComponent<PlayerInteraction>();

        Keybinds.current.onKeyChangeEvent += OnKeyChangeEvent;
        OnKeyChangeEvent();
    }

    private void Update() {
        if(isDriving) {
            carCon.GetInput();

            //Get pay
            if(Input.GetKeyDown(Key_TakePayment)) pi.GrabAllFromStorage(carCon.payPoint.GetComponent<StorageHandler>());
            //Give change
            else if(Input.GetKeyDown(Key_GiveChange)) pi.PutAllInStorage(carCon.changePoint.GetComponent<StorageHandler>());
            //Scroll up
            else if(Input.GetKeyDown(Key_ChangerScrollUp)) carCon.changePoint.GetComponent<ChangeHandler>().Scroll(1);
            //Scroll down
            else if(Input.GetKeyDown(Key_ChangerScrollDown)) carCon.changePoint.GetComponent<ChangeHandler>().Scroll(-1);
        }

        leftHandIK.weight = isDriving? 1.0f:0f;
        rightHandIK.weight = isDriving? 1.0f:0f;
    }

    public void SetIsSitting(bool newIsSitting, bool isDriversSeat, Transform seat) {
        // print("set is sitting");
        isSitting = newIsSitting;
        if(!isDriversSeat) isDriving = false;
        
        fpm.enabled = !isSitting;
        jump.enabled = !isSitting;
        crouch.enabled = !isSitting;
        fpa.SetActive(!isSitting);

        // transform.SetParent(seat);
        // transform.localPosition = Vector3.zero;
        GetComponent<Rigidbody>().isKinematic = isSitting;
        GetComponent<CapsuleCollider>().isTrigger = isSitting;

        // Vector3 newPos = playerModel.localPosition;
        // float newY = isSitting? 0.25f : 0;
        // newPos.y = newY;
        // playerModel.localPosition = newPos;

        //PLAYER MODEL
        if(isSitting) {
            playerModel.SetParent(seat);
            Vector3 modelRot = new Vector3(0, seat.GetComponent<SeatHandler>().modelYRot, 0);
            playerModel.localEulerAngles = modelRot;
            // head.transform.localEulerAngles = Vector3.zero;
            if(isDriving) playerModel.localPosition = localDrivePos;
            else playerModel.localPosition = seat.GetComponent<SeatHandler>().localPosModel;
        } else {
            playerModel.SetParent(player);
            playerModel.localEulerAngles = Vector3.zero; //
            playerModel.localPosition = Vector3.zero;
            // head.transform.localEulerAngles = Vector3.zero;
        }

        if(isDriversSeat) {
            head.isDriving = isDriving;
            head.isSitting = false;
            head.CheckState();
            // transform.localPosition = new Vector3(0f, 0f, -0.15f);
        } else {
            head.isDriving = false;
            head.isSitting = isSitting;
            head.CheckState();
            // transform.localPosition = new Vector3(0f, 0f, -0.15f);
        }
    }

    public void SetIsDriving(bool newIsDriving, CarController newCarCon, Transform seat) {
        // print("set is driving: " + newCarCon.name);
        isDriving = newIsDriving;
        carCon = newCarCon;
        JeepneyPanel.current.SetCarcon(carCon);
        if(carCon) {
            carCon.moveInput = 0;
            carCon.steerInput = 0; 
        }

        SetIsSitting(isDriving, true, seat);

        //NAMING
        if(!isDriving) {
            //UI
            nameNoJeepneyDetected.SetActive(true);
            nameInput.interactable = false;
        } else {
            nameNoJeepneyDetected.SetActive(false);
            nameInput.name = carCon.jeepName;
            nameInput.interactable = true;
        }
    }

    private void OnKeyChangeEvent() {
        Key_GiveChange = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_GiveChange", "R"));
        Key_ChangerScrollUp = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_ChangerScrollUp", "E"));
        Key_ChangerScrollDown = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_ChangerScrollDown", "Q"));
        Key_TakePayment = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_TakePayment", "F"));
    }

    // public void StopPickups() {
    //     NotificationManager.current.NewNotif("PICKUPS STOPPED", "You can no longer pick up passengers.");

    //     isPickups = false;
    // }

    public void SetPickups(bool newVal) {
        print("SET PICKUPS: " + newVal);
        if(isPickups == newVal) return;

        isPickups = newVal;

        if(!isPickups) NotificationManager.current.NewNotif("PICKUPS STOPPED", "You can no longer pick up passengers. Finish shift to reset!");

        pickupsOffSign.SetActive(!isPickups);
        // print(pickupsOffSign.activeSelf? "true":"false");
    }

    public void RenameJeep() {
        carCon.Rename(nameInput.text);
    }
}
