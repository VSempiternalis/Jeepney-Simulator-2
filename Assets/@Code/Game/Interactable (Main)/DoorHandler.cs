using UnityEngine;

public class DoorHandler : MonoBehaviour, IInteractable, ITooltipable {
    [SerializeField] private string header;
    [SerializeField] private string text;

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

    // private AudioHandler audioHandler;

    public bool isLocked;
    [SerializeField] private bool isHouseDoor;

    private void Start() {
        // audioHandler = GetComponent<AudioHandler>();
        if(!hasPivot) pivot = transform;

        initialXRot = pivot.transform.localRotation.eulerAngles.x;
    }

    private void Update() {
        if(state == "Closing") {
            if(pivot.transform.localRotation.eulerAngles.y >= closedYRot - limit && pivot.transform.localRotation.eulerAngles.y <= closedYRot + limit) {
                state = "Closed";
                pivot.transform.localRotation = Quaternion.Euler(closedXRot, closedYRot, closedZRot);
            } else {
                pivot.transform.localRotation = Quaternion.Slerp(pivot.transform.localRotation, Quaternion.Euler(closedXRot, closedYRot, closedZRot), rotationSpeed*Time.deltaTime);
            }
        } else if(state == "Opening") {
            if(pivot.transform.localRotation.eulerAngles.y >= openYRot - limit && pivot.transform.localRotation.eulerAngles.y <= openYRot + limit) {
                state = "Open";
                pivot.transform.localRotation = Quaternion.Euler(openXRot, openYRot, openZRot);
            } else {
                pivot.transform.localRotation = Quaternion.Slerp(pivot.transform.localRotation, Quaternion.Euler(openXRot, openYRot, openZRot), rotationSpeed*Time.deltaTime);
            }
        }
    }

    #region INTERFACE FUNCTIONS

    public void Interact(GameObject interactor) {
        if(isLocked) {
            //Play jiggle audio
            // audioHandler.Play(2);
            // if(isHouseDoor) Fader.current.YawnGray(0.5f, "I can't get in unless I pay the bills first", 0.5f);
            return;
        }
        
        if(state == "Open") {
            state = "Closing";
            // audioHandler.Play(0);
        } else if(state == "Closed") {
            state = "Opening";
            // audioHandler.Play(1);
        } else if(state == "Opening" && pivot.transform.localRotation.eulerAngles.y > openYRot) {
            state = "Closing";
            // audioHandler.Play(0);
        } else if(state == "Closing" && pivot.transform.localRotation.eulerAngles.y < closedYRot) {
            state = "Opening";
            // audioHandler.Play(1);
        }
    }

    public string GetHeader() {
        return header;
    }

    public string GetText() {
        return text;
    }

    #endregion
}