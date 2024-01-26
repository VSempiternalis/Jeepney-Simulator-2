using UnityEngine;

public class PlayerDriveInput : MonoBehaviour {
    public bool isDriving;
    public CarController carCon;

    [SerializeField] private FirstPersonMovement fpm;
    [SerializeField] private Jump jump;
    [SerializeField] private Crouch crouch;
    [SerializeField] private FirstPersonAudio fpa;

    private void Start() {
    }

    private void Update() {
        if(isDriving) carCon.GetInput();
    }

    public void SetIsDriving(bool newIsDriving) {
        isDriving = newIsDriving;

        fpm.enabled = !isDriving;
        jump.enabled = !isDriving;
        crouch.enabled = !isDriving;
        fpa.enabled = !isDriving;
    }
}
