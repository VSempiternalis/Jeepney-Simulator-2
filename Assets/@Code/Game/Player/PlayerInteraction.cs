using UnityEditor;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
    [SerializeField] private Camera playerCam;
    
    private GameObject itemOver;
    [SerializeField] private float reachDist = 1.5f;

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
    [SerializeField] private LayerMask interactionMask;
    [SerializeField] private float raycastOffset;

    [Space(10)]
    [Header("ITEMS")]
    [SerializeField] private Transform rightHand;
    private RaycastHit hit;

    [Space(10)]
    [Header("LAYERS")]
    [SerializeField] private int layerInteractable;
    [SerializeField] private int layerItem;
    [SerializeField] private int layerStorage;

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

        Vector3 raycastOrigin = playerCam.transform.position + (Vector3.up * raycastOffset);
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(raycastOrigin, ray.direction, Color.red);
        if(Physics.Raycast(raycastOrigin, ray.direction, out hit, reachDist, interactionMask)) {
            if (hit.collider) { // Check if the collider is on the Interactable layer
                itemOver = hit.collider.gameObject;
            }
        }

        //[Look for item in hits]
        // var hits = Physics.RaycastAll(ray, reachDist);
        // foreach(RaycastHit hit in hits) {
        //     if(hit.collider && (hit.collider.gameObject.layer == layerInteractable)){ //Make sure collider is layer layerInteractable (Interactable)
        //         itemOver = hit.collider.gameObject;
        //     }
        // }

        if(itemOver && itemOver.GetComponent<Outline>()) itemOver.GetComponent<Outline>().OutlineWidth = 5;
    }

    private void Interaction() {
        if(!itemOver) return;

        //LEFT CLICK
        if(lMouseDown) {
            if(itemOver.layer == layerInteractable) {
                if(itemOver.GetComponent<IInteractable>() != null) itemOver.GetComponent<IInteractable>().Interact(gameObject);
            } else if(itemOver.layer == layerItem) {
                TakeItemOver();
            } 
        }

        //RIGHT CLICK
        else if(rMouseDown) {
            if(itemOver.layer == layerStorage && rightHand.childCount > 0) {
                PlaceItem();
            }
        }
    }

    private void TakeItemOver() {
        rightHand.GetComponent<StorageHandler>().AddItemRandom(itemOver);

        //audio
    }

    private void PlaceItem() {
        print("placing item");
        //Place item on itemover(storage)
        GameObject dropItem = rightHand.GetChild(0).gameObject;

        if(itemOver.GetComponent<StorageHandler>()) {
            StorageHandler storage = itemOver.GetComponent<StorageHandler>();
            storage.AddItem(dropItem, hit.point);
        }

        //+ UpdateOnhandUI();
    
        //audio
    }
}