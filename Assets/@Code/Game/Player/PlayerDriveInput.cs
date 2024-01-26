using UnityEngine;

public class PlayerDriveInput : MonoBehaviour {
    public bool isDriving;
    public CarController carCon;

    [SerializeField] private FirstPersonMovement fpm;
    [SerializeField] private Jump jump;
    [SerializeField] private Crouch crouch;
    // [SerializeField] private FirstPersonAudio fpa;
    [SerializeField] private GameObject fpa;

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
    }
}
