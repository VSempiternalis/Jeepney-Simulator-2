using UnityEditor;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
    [SerializeField] private Camera playerCam;
    
    private GameObject itemOver;
    private float reachDist = 1.5f;

    private bool lMouseDown;
    private bool rMouseDown;
    private bool mMouseDown;
    private bool tabDown;
    private bool f12Down;
    private bool lMouseHold;
    private bool rMouseHold;
    private float mouseScroll;
    [SerializeField] private float lClickTime = 0f;
    [SerializeField] private float rClickTime = 0f;
    private float clickHoldThresh = 0.2f;

    private void Start() {
        
    }

    private void Update() {
        GetInput();

        SetItemOver();
        Interaction();

        ClearInput();
    }

    private void GetInput() {
        lMouseDown = Input.GetMouseButtonDown(0);
        rMouseDown = Input.GetMouseButtonDown(1);
        mMouseDown = Input.GetMouseButtonDown(2);

        lMouseHold = Input.GetMouseButton(0);
        rMouseHold = Input.GetMouseButton(1);

        if(lMouseDown) {
            lClickTime = Time.time;
        } else if(lMouseHold) {
            if((Time.time - lClickTime) > clickHoldThresh) {
                lMouseHold = true;
            } else lMouseHold = false;
        }
        if(rMouseDown) {
            rClickTime = Time.time;
        } else if(rMouseHold) {
            if((Time.time - rClickTime) > clickHoldThresh) {
                rMouseHold = true;
            } else rMouseHold = false;
        }

        f12Down = Input.GetKeyDown(KeyCode.F12);

        mouseScroll = Input.mouseScrollDelta.y;
    }

    private void ClearInput() {
        lMouseDown = false;
        rMouseDown = false;
        mMouseDown = false;
        lMouseHold = false;
        rMouseHold = false;
        tabDown = false;
        f12Down = false;
        mouseScroll = 0;
    }

    private void SetItemOver() {
        //Clear ItemOver
        if(itemOver && itemOver.GetComponent<Outline>()) itemOver.GetComponent<Outline>().OutlineWidth = 0;
        itemOver = null;

        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
        var hits = Physics.RaycastAll(ray, reachDist);

        //[Look for item in hits]
        foreach(RaycastHit hit in hits) {
            if(hit.collider && (hit.collider.gameObject.layer == 8)){ //Make sure collider is layer 8 (Interactable)
                itemOver = hit.collider.gameObject;
            }
        }

        if(itemOver && itemOver.GetComponent<Outline>()) itemOver.GetComponent<Outline>().OutlineWidth = 5;
    }

    private void Interaction() {
        if(itemOver && itemOver.layer == 8) {
            if(lMouseDown && itemOver.GetComponent<IInteractable>() != null) itemOver.GetComponent<IInteractable>().Interact(gameObject);
        }
    }
}