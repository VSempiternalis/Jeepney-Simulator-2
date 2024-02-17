using UnityEngine;

public class PlayerDriveInput : MonoBehaviour {
    public static PlayerDriveInput current;
    public CarController carCon;
    [SerializeField] private Head head;
    [SerializeField] private FirstPersonMovement fpm;
    [SerializeField] private Jump jump;
    [SerializeField] private Crouch crouch;
    // [SerializeField] private Head head;

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

    public void SetIsSitting(bool newIsSitting) {
        fpm.enabled = !isDriving;
        jump.enabled = !isDriving;
        crouch.enabled = !isDriving;
        fpa.SetActive(!isDriving);

        GetComponent<Rigidbody>().isKinematic = isDriving;
        GetComponent<CapsuleCollider>().isTrigger = isDriving;

        Vector3 newPos = playerModel.localPosition;
        float newY = isDriving? 0.25f : 0;
        newPos.y = newY;
        playerModel.localPosition = newPos;

        // if(!isDriving) SetIsDriving(false, null);
    }

    public void SetIsDriving(bool newIsDriving, CarController newCarCon) {
        isDriving = newIsDriving;
        carCon = newCarCon;

        SetIsSitting(isDriving);

        head.isDriving = isDriving;
        head.CheckState();
    }
}
