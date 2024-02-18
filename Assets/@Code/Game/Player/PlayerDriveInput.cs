using UnityEngine;

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
    [SerializeField] private Transform playerModel;

    private void Awake() {
        current = this;
    }

    private void Start() {
    }

    private void Update() {
        if(isDriving) carCon.GetInput();
    }

    public void SetIsSitting(bool newIsSitting, bool isDriversSeat) {
        print("sitting");
        isSitting = newIsSitting;
        
        fpm.enabled = !isSitting;
        jump.enabled = !isSitting;
        crouch.enabled = !isSitting;
        fpa.SetActive(!isSitting);

        GetComponent<Rigidbody>().isKinematic = isSitting;
        GetComponent<CapsuleCollider>().isTrigger = isSitting;

        Vector3 newPos = playerModel.localPosition;
        float newY = isSitting? 0.25f : 0;
        newPos.y = newY;
        playerModel.localPosition = newPos;

        // if(!isDriving) SetIsDriving(false, null);

        if(isDriversSeat) {
            head.isDriving = isDriving;
            head.CheckState();
        } else {
            head.isDriving = false;
            head.isSitting = isSitting;
            head.CheckState();
        }
    }

    public void SetIsDriving(bool newIsDriving, CarController newCarCon) {
        isDriving = newIsDriving;
        carCon = newCarCon;

        SetIsSitting(isDriving, true);
    }
}
