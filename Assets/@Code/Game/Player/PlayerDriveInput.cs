using UnityEngine;
using UnityEngine.Animations.Rigging;
// using UnityEngine.Animations.Rigging;

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
    public bool isTakingPassengers;

    [SerializeField] private GameObject fpa; //firstpersonaudio

    [Space(10)]
    [Header("SEATING")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerModel;
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    [SerializeField] private TwoBoneIKConstraint rightHandIK;
    [SerializeField] private Vector3 localDrivePos;
    [SerializeField] private Vector3 localSitPos;

    private void Awake() {
        current = this;
    }

    private void Start() {

    }

    private void Update() {
        if(isDriving) carCon.GetInput();
        
        leftHandIK.weight = isDriving? 1.0f:0f;
        rightHandIK.weight = isDriving? 1.0f:0f;
    }

    public void SetIsSitting(bool newIsSitting, bool isDriversSeat, Transform seat) {
        print("set is sitting");
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
            // playerModel.localEulerAngles = Vector3.zero;
            if(isDriving) playerModel.localPosition = localDrivePos;
            else playerModel.localPosition = seat.GetComponent<SeatHandler>().localPosModel;
        } else {
            playerModel.SetParent(player);
            playerModel.localEulerAngles = Vector3.zero;
            playerModel.localPosition = Vector3.zero;
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
        print("set is driving");
        isDriving = newIsDriving;
        carCon = newCarCon;
        if(carCon) {
            carCon.moveInput = 0;
            carCon.steerInput = 0; 
        }

        SetIsSitting(isDriving, true, seat);
    }
}
