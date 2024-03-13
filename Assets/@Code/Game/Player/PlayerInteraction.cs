using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour {
    [SerializeField] private Camera playerCam;
    
    private GameObject itemOver;
    [SerializeField] private float reachDist = 1.5f;

    private bool lMouseDown;
    private bool rMouseDown;
    private bool mMouseDown;
    private bool tabDown;
    private bool hudToggleDown;
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
    [SerializeField] private int maxItemsOnHand;
    [SerializeField] private Transform rightHand;
    private RaycastHit hit;

    [Space(10)]
    [Header("LAYERS")]
    [SerializeField] private int layerInteractable;
    [SerializeField] private int layerItem;
    [SerializeField] private int layerStorage;
    [SerializeField] private int layerDrop;
    [SerializeField] private int layerArea;

    [Space(10)]
    [Header("UI")]
    [SerializeField] private TMP_Text areaUI;
    // [SerializeField] private GameObject dropUI;
    [SerializeField] private GameObject onhandItemPF;
    [SerializeField] private Transform onhandUI;
    [SerializeField] private TMP_Text tooltipHeader;
    [SerializeField] private TMP_Text tooltipControls;
    [SerializeField] private TMP_Text tooltipDesc;
    [SerializeField] private uiAnimGroup gameHUD;
    private TooltipMask tm;
    private GameObject oldItemOver;
    private bool isItemOverSame;

    [Space(10)]
    [Header("DESTINATIONS")]
    private bool inDrop;
    private bool inArea;
    [SerializeField] private uiAnimGroup playerDestUI;
    [SerializeField] private uiAnimGroup inAreaUI;

    [Space(10)]
    [Header("KEYBINDS")]
    private KeyCode Key_HUDToggle;

    private void Start() {
        Keybinds.current.onKeyChangeEvent += OnKeyChangeEvent;
        OnKeyChangeEvent();

        tm = TooltipMask.current;
    }

    private void Update() {
        GetInput();

        SetItemOver();
        Interaction();

        ClearInput();
    }

    private void OnKeyChangeEvent() {
        Key_HUDToggle = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_HUDToggle", "F12"));
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

        hudToggleDown = Input.GetKeyDown(Key_HUDToggle);

        mouseScroll = Input.mouseScrollDelta.y;
    }

    private void ClearInput() {
        lMouseDown = false;
        rMouseDown = false;
        mMouseDown = false;
        lMouseHold = false;
        rMouseHold = false;
        tabDown = false;
        hudToggleDown = false;
        mouseScroll = 0;
    }

    private void SetItemOver() {
        //Clear ItemOver
        if(itemOver && itemOver.GetComponent<Outline>()) itemOver.GetComponent<Outline>().OutlineWidth = 0;
        oldItemOver = itemOver;
        itemOver = null;
        isItemOverSame = false;

        Vector3 raycastOrigin = playerCam.transform.position + (Vector3.up * raycastOffset);
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(raycastOrigin, ray.direction, Color.red);
        if(Physics.Raycast(raycastOrigin, ray.direction, out hit, reachDist, interactionMask)) {
            if(hit.collider) { // Check if the collider is on the Interactable layer
                itemOver = hit.collider.gameObject;
            }
        }

        if(itemOver == oldItemOver) isItemOverSame = true;

        if(itemOver) {
            //OUTLINE
            if(itemOver.GetComponent<Outline>()) itemOver.GetComponent<Outline>().OutlineWidth = 5;

            //TOOLTIP
            if(itemOver.GetComponent<ITooltipable>() != null) {
            // if(itemOver.GetComponent<ITooltipable>() != null && !isItemOverSame) {
                ITooltipable tooltipable = itemOver.GetComponent<ITooltipable>();
                tooltipHeader.text = tooltipable.GetHeader();
                tooltipControls.text = tooltipable.GetControls();
                tooltipDesc.text = tooltipable.GetDesc();
                // tooltipHeader.gameObject.SetActive(true);
                // tooltipText.gameObject.SetActive(true);

                tm.In();
            }
        } else {
            tm.Out();
        }
    }

    private void Interaction() {
        //MAP
        if(tabDown) {
            //AUDIO
            // audioHandler.Play(1);

            // MapManager.current.Toggle();
        } 
        //HIDE UI
        else if(hudToggleDown) {
            // gameHUD.SetActive(!gameHUD.activeSelf);
            if(gameHUD.GetComponent<CanvasGroup>().alpha > 0) gameHUD.Out();
            else gameHUD.In();
        }
        
        if(!itemOver) return;

        //LEFT CLICK
        if(lMouseDown) {
            //INTERACTABLE
            if(itemOver.layer == layerInteractable) {
                if(itemOver.GetComponent<IInteractable>() != null) itemOver.GetComponent<IInteractable>().Interact(gameObject);
            } 
            //ITEM
            else if(itemOver.layer == layerItem) {
                TakeItemOver();
            } 
        }
        //RIGHT CLICK
        else if(rMouseDown) {
            if(itemOver.layer == layerStorage && rightHand.childCount > 0) {
                PlaceItem();
            }
        }
        //MID CLICK
        else if(mMouseDown) {
            if(itemOver.layer == layerStorage) {
                GrabAllFromStorage(itemOver.GetComponent<StorageHandler>());
                //AUDIO
                // audioHandler.Play(0);
            }
        }
        //LEFT HOLD
        else if(lMouseHold) {
            if(itemOver.layer == layerItem) TakeItemOver();
        } 
        //RIGHT HOLD
        else if(rMouseHold) {
            if(itemOver.layer == layerStorage && rightHand.childCount > 0) PlaceItem();
            // else if(itemOver.GetComponent<IPayable>() != null && rightHand.childCount > 0) PayItem();
        }

        //MOUSE SCROLL
        if(mouseScroll != 0 && itemOver.GetComponent<IScrollable>() != null) {
            itemOver.GetComponent<IScrollable>().Scroll(mouseScroll);
        }
    }

    public void GrabAllFromStorage(StorageHandler storage) {
        for(int i = 0; i < maxItemsOnHand; i++) {
            List<GameObject> items = storage.items;
            // print("i: " + i + " items: " + items.Count + " right hand items: " + rightHand.childCount);
            if(items.Count == 0 || rightHand.childCount == maxItemsOnHand) break;

            rightHand.GetComponent<StorageHandler>().AddItemRandom(items[0]);
        }
        UpdateOnhandUI();
    }

    public void PutAllInStorage(StorageHandler storage) {
        List<GameObject> toStore = new List<GameObject>();

        foreach(Transform item in rightHand) {
            // print("[1] Putting in storage: " + item.name + " items total: " + rightHand.childCount);
            toStore.Add(item.gameObject);
            // storage.AddItemRandom(item.gameObject);
            // UpdateOnhandUI();
        }

        foreach(GameObject item in toStore) {
            // print("[2] Putting in storage: " + item.name + " items total: " + rightHand.childCount);
            storage.AddItemRandom(item.gameObject);
        }
        UpdateOnhandUI();
    }

    private void TakeItemOver() {
        if(rightHand.childCount >= maxItemsOnHand) {
            // Fader.current.YawnGray(0.5f, "My hands are full", 0.5f);
            return;
        }
        rightHand.GetComponent<StorageHandler>().AddItemRandom(itemOver);

        UpdateOnhandUI();

        //audio
    }

    private void PlaceItem() {
        // print("placing item");
        //Place item on itemover(storage)
        GameObject dropItem = rightHand.GetChild(0).gameObject;

        if(itemOver.GetComponent<StorageHandler>()) {
            StorageHandler storage = itemOver.GetComponent<StorageHandler>();
            storage.AddItem(dropItem, hit.point);
        }

        UpdateOnhandUI();
    
        //audio
    }

    private void UpdateOnhandUI() {
        ClearOnhandUI();

        //Repopulate onhand ui
        for(int i = 0; i < rightHand.childCount; i++) {
            string itemName = rightHand.GetChild(i).name;
            GameObject onhandItem = Instantiate(onhandItemPF);
            onhandItem.transform.GetChild(0).GetComponent<TMP_Text>().text = itemName;
            onhandItem.name = itemName;
            onhandItem.transform.SetParent(onhandUI);
            onhandItem.SetActive(true);

            onhandUI.gameObject.SetActive(false);
            onhandUI.gameObject.SetActive(true);

            // if(i == 0) onhandItem.GetComponent<VerticalLayoutGroup>().padding.right = 40;
            // else onhandItem.GetComponent<VerticalLayoutGroup>().padding.right = 20;
        }
    }

    private void ClearOnhandUI() {
        List<Transform> removeList = new List<Transform>();

        foreach(Transform onhandItem in onhandUI) {
            removeList.Add(onhandItem);
        }
        foreach(Transform remove in removeList) {
            Destroy(remove.gameObject);
        }
    }

    public bool CanScroll() {
        bool returnBool = true;

        if(itemOver && itemOver.GetComponent<ChangeHandler>() && itemOver.GetComponent<ChangeHandler>().changees.Count > 1) returnBool = false;

        return returnBool;
    }
    

    private void OnTriggerEnter(Collider other) {
        GameObject go = other.gameObject;
         
        if(!playerDestUI.isIn && !inDrop && go.layer == layerDrop) { // && areaUI.text != other.name
            inDrop = true;

            playerDestUI.In();
            if(areaUI.text != other.name) areaUI.text = other.name;

            //Audio
            // audioHandler.Play(8);
        } else if(!inAreaUI.isIn && go.layer == layerArea) { // && areaUI.text != other.name
            inArea = true;

            inAreaUI.In();
        }

        if(go.CompareTag("StartShiftTrigger")) {
            TimeManager.current.TryStartShift();
        } else if(go.CompareTag("EndShiftTrigger")) {
            TimeManager.current.TryEndShift();
        }
    }

    private void OnTriggerStay(Collider other) {
        GameObject go = other.gameObject;
    }

    private void OnTriggerExit(Collider other) {
        GameObject go = other.gameObject;
        if(playerDestUI.isIn && go.layer == layerDrop) {
            inDrop = false;
            playerDestUI.Out();

            //Audio
            // if(!GetComponent<AudioSource>().isPlaying) audioHandler.Play(9);
        }
        if(inAreaUI.isIn && go.layer == layerArea) {
            inAreaUI.Out();
        }
    }
}