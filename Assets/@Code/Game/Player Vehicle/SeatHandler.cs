using UnityEngine;

public class SeatHandler : MonoBehaviour, IInteractable, ITooltipable {
    [SerializeField] private string header;
    [SerializeField] private string text;

    [SerializeField] private CarController carCon;
    public Vector3 localPosPlayer;
    public Vector3 localPosModel;
    public int modelYRot;
    [SerializeField] private Transform exitPoint;

    [SerializeField] private AudioSource audioSource;

    private void Start() {
        
    }

    private void Update() {

    }

    public void Interact(GameObject interactor) {
        // print("interacting");
        Transform player = interactor.transform;
        if(transform.childCount > 1) {
            //EXIT
            player.position = exitPoint.position;
            player.SetParent(null);

            if(carCon) {
                player.GetComponent<PlayerDriveInput>().SetIsDriving(false, null, transform);
                carCon.DriverExit();
            }
            else player.GetComponent<PlayerDriveInput>().SetIsSitting(false, false, transform);
        } else {
            //ENTER
            if(carCon) player.GetComponent<PlayerDriveInput>().SetIsDriving(true, carCon, transform);
            else player.GetComponent<PlayerDriveInput>().SetIsSitting(true, false, transform);

            player.SetParent(transform);
            player.localPosition = localPosPlayer;
            player.localEulerAngles = Vector3.zero;
            // print("!player y rot: " + player.localRotation.y);
        }

        if(audioSource) audioSource.Play();
    }

    public string GetHeader() {
        return header;
    }

    public string GetText() {
        return text;
    }
}
