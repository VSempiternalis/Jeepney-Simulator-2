using UnityEngine;

public class PlayerDriveInput : MonoBehaviour {
    public static PlayerDriveInput current;
    public CarController carCon;

    public bool isDriving;
    public bool isTakingPassengers;

    [SerializeField] private FirstPersonMovement fpm;
    [SerializeField] private Jump jump;
    [SerializeField] private Crouch crouch;
    // [SerializeField] private FirstPersonAudio fpa;
    [SerializeField] private GameObject fpa;
    [SerializeField] private Transform playerModel;

    private void Awake() {
        current = this;
    }

    private void Start() {
    }

    private void Update() {
        if(isDriving) carCon.GetInput();
    }

    public void SetIsDriving(bool newIsDriving, CarController newCarCon) {
        isDriving = newIsDriving;

        fpm.enabled = !isDriving;
        jump.enabled = !isDriving;
        crouch.enabled = !isDriving;
        // fpa.enabled = !isDriving;
        fpa.SetActive(!isDriving);

        carCon = newCarCon;
        GetComponent<Rigidbody>().isKinematic = isDriving;
        GetComponent<CapsuleCollider>().isTrigger = isDriving;

        Vector3 newPos = playerModel.localPosition;
        float newY = isDriving? 0.25f : 0;
        newPos.y = newY;
        playerModel.localPosition = newPos;
    }
}
