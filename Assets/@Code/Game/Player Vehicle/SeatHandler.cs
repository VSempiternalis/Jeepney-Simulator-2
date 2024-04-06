using UnityEngine;

public class SeatHandler : MonoBehaviour, IInteractable, ITooltipable {
    [SerializeField] private string header;
    [SerializeField] private string controls;
    [SerializeField] [TextArea] private string desc;

    [SerializeField] private CarController carCon;
    public Vector3 localPosPlayer;
    public Vector3 localPosModel;
    public int modelYRot;
    [SerializeField] private Transform exitPoint;

    [SerializeField] private AudioSource audioSource;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {

    }

    public void Interact(GameObject interactor) {
        // print("interacting");
        Transform player = interactor.transform;

        bool isTutorial = player.GetComponent<PlayerDriveInputTUTORIAL>();

        if(transform.childCount > 1) {
            //EXIT
            player.position = exitPoint.position;
            player.SetParent(null);

            if(carCon) {
                if(isTutorial) {
                    player.GetComponent<PlayerDriveInputTUTORIAL>().SetIsDriving(false, carCon, transform);
                    carCon.DriverExit();
                } else {
                    player.GetComponent<PlayerDriveInput>().SetIsDriving(false, carCon, transform);
                    carCon.DriverExit();
                }
            }
            else if(isTutorial) player.GetComponent<PlayerDriveInputTUTORIAL>().SetIsSitting(false, false, transform);
            else player.GetComponent<PlayerDriveInput>().SetIsSitting(false, false, transform);
        } else {
            //ENTER
            if(isTutorial) {
                if(carCon) player.GetComponent<PlayerDriveInputTUTORIAL>().SetIsDriving(true, carCon, transform);
                else player.GetComponent<PlayerDriveInputTUTORIAL>().SetIsSitting(true, false, transform);
            } else {
                if(carCon) player.GetComponent<PlayerDriveInput>().SetIsDriving(true, carCon, transform);
                else player.GetComponent<PlayerDriveInput>().SetIsSitting(true, false, transform);
            }

            player.SetParent(transform);
            player.localPosition = localPosPlayer;
            player.localEulerAngles = Vector3.zero;
            // print("!player y rot: " + player.localRotation.y);
        }

        if(audioSource) audioSource.Play();
    }

    public string GetHeader() {
        if(header != "") return header;
        return "Seat";
    }

    public string GetControls() {
        return "[L Mouse] Sit/Exit";
    }

    public string GetDesc() {
        return desc;
    }
}
