using UnityEngine;

public class DoorHandler : MonoBehaviour, IInteractable, ITooltipable {
    [SerializeField] private string header;
    [SerializeField] private string controls;
    [SerializeField] [TextArea] private string desc;

    public string state; //open, closed, opening, closing
    [SerializeField] private bool hasPivot;
    [SerializeField] private Transform pivot;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private int openXRot = 0;
    [SerializeField] private int openYRot = 0;
    [SerializeField] private int openZRot = 0;
    [SerializeField] private int closedXRot = 0;
    [SerializeField] private int closedYRot = 0;
    [SerializeField] private int closedZRot = 0;
    private float initialXRot = 0;

    private float limit = 1f;

    [SerializeField] private AudioHandler audioHandler;

    public bool isLocked;
    [SerializeField] private bool isOfficeDoor;
    [SerializeField] private bool isOfficeDoorTUTORIAL;

    private void Start() {
        // audioHandler = GetComponent<AudioHandler>();
        if(!hasPivot) pivot = transform;

        initialXRot = pivot.transform.localRotation.eulerAngles.x;

        if(GetComponent<AudioHandler>() != null) audioHandler = GetComponent<AudioHandler>(); 
    }

    private void Update() {
        
    }

    #region INTERFACE FUNCTIONS

    public void Interact(GameObject interactor) {
        if(isLocked) {
            //Play jiggle audio
            // audioHandler.Play(2);
            return;
        }
        
        if(isOfficeDoor && RouteSelector.current.destinations.Count <= 3) {
            NotificationManager.current.NewNotif("NOT ENOUGH LANDMARKS", "You must have at least four active (ON or LOCKED) landmarks to start your shift. Go to the route selector and click on a red landmark!");
            AudioManager.current.PlayUI(7);
            return;
        }
        
        if(isOfficeDoorTUTORIAL && RouteSelector.current.destinations.Count <= 1) {
            NotificationManager.current.NewNotif("NOT ENOUGH LANDMARKS", "You must have at least three active (ON or LOCKED) landmarks in the tutorial to start your shift. Go to the route selector and click on a red landmark!");
            AudioManager.current.PlayUI(7);
            return;
        }
        
        if(state == "Open") {
            state = "Closing";
            audioHandler.Play(2);
            // Tween close rotation
            LeanTween.rotateLocal(pivot.gameObject, new Vector3(closedXRot, closedYRot, closedZRot), rotationSpeed)
                .setEaseOutExpo()
                .setOnComplete(() => state = "Closed");
        } else if(state == "Closed") {
            state = "Opening";
            audioHandler.Play(1);
            // Tween open rotation
            LeanTween.rotateLocal(pivot.gameObject, new Vector3(openXRot, openYRot, openZRot), rotationSpeed)
                .setEaseOutExpo()
                .setOnComplete(() => state = "Open");
        } else if(state == "Opening" && pivot.transform.localRotation.eulerAngles.y > openYRot) {
            state = "Closing";
            audioHandler.Play(2);
            // Tween close rotation
            LeanTween.rotateLocal(pivot.gameObject, new Vector3(closedXRot, closedYRot, closedZRot), rotationSpeed)
                .setEaseOutExpo()
                .setOnComplete(() => state = "Closed");
        } else if(state == "Closing" && pivot.transform.localRotation.eulerAngles.y < closedYRot) {
            state = "Opening";
            audioHandler.Play(1);
            // Tween open rotation
            LeanTween.rotateLocal(pivot.gameObject, new Vector3(openXRot, openYRot, openZRot), rotationSpeed)
                .setEaseOutExpo()
                .setOnComplete(() => state = "Open");
        }
    }

    public string GetHeader() {
        return "Door";
    }

    public string GetControls() {
        return "[L Mouse] Open/Close";
    }

    public string GetDesc() {
        return desc;
    }

    #endregion
}