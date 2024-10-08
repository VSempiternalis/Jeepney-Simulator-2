using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInteraction : MonoBehaviour {
    [SerializeField] private Camera playerCam;
    [SerializeField] private VicCamManager vcm;
    private PlayerDriveInput pdi;
    
    private GameObject itemOver;
    [SerializeField] private float reachDist = 1.5f;

    private bool lMouseDown;
    private bool rMouseDown;
    private bool mMouseDown;
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
    public int maxItemsOnHand;
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
    [SerializeField] private Color green;
    [SerializeField] private Color red;

    [Space(10)]
    [Header("KEYBINDS")]
    private KeyCode Key_HUDToggle;
    private KeyCode Key_GrabAllItems;

    [Space(10)]
    [Header("AUDIO")]
    private AudioManager am;

    private void Start() {
        Keybinds.current.onKeyChangeEvent += OnKeyChangeEvent;
        OnKeyChangeEvent();

        tm = TooltipMask.current;
        am = AudioManager.current;
        pdi = GetComponent<PlayerDriveInput>();
    }

    private void Update() {
        GetInput();

        SetItemOver();
        Interaction();

        ClearInput();
    }

    private void OnKeyChangeEvent() {
        Key_HUDToggle = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_HUDToggle", "F12"));
        Key_GrabAllItems = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_GrabAllItems", KeyCode.Mouse2.ToString()));
    }

    private void GetInput() {
        lMouseDown = Input.GetMouseButtonDown(0);
        rMouseDown = Input.GetMouseButtonDown(1);
        mMouseDown = Input.GetKeyDown(Key_GrabAllItems);
        // mMouseDown = Input.GetMouseButtonDown(2);

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
        //HIDE UI
        if(hudToggleDown) {
            // gameHUD.SetActive(!gameHUD.activeSelf);
            if(gameHUD.GetComponent<CanvasGroup>().alpha > 0) gameHUD.Out();
            else gameHUD.In();
        }
        
        if(!itemOver) return;
        if(!vcm.CanInteract()) return;

        //LEFT CLICK
        if(lMouseDown) {
            //INTERACTABLE
            if(itemOver.layer == layerInteractable) {
                // pdi.TryGrabIK(itemOver.transform); //MAKES HAND INTERACT WITH INTERACTABLES
                if(itemOver.GetComponent<IKnob>() != null && !GetComponent<Crouch>().IsCrouched) itemOver.GetComponent<IKnob>().LeftClick();
                else if(itemOver.GetComponent<IInteractable>() != null && !GetComponent<Crouch>().IsCrouched) itemOver.GetComponent<IInteractable>().Interact(gameObject);
            } 
            //ITEM
            else if(itemOver.layer == layerItem) {
                TakeItemOver();
            } 
        }
        //RIGHT CLICK
        else if(rMouseDown) {
            //INTERACTABLE
            if(itemOver.layer == layerInteractable) {
                // pdi.TryGrabIK(itemOver.transform); //MAKES HAND INTERACT WITH INTERACTABLES
                if(itemOver.GetComponent<IKnob>() != null && !GetComponent<Crouch>().IsCrouched) itemOver.GetComponent<IKnob>().RightClick();
            } 

            //STORAGE
            if(itemOver.layer == layerStorage && rightHand.childCount > 0) {
                PlaceItem();
            } else if(itemOver.GetComponent<IPayable>() != null && rightHand.childCount > 0) {
                PayItem();
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
            else if(itemOver.GetComponent<IPayable>() != null && rightHand.childCount > 0) PayItem();
        }

        //MOUSE SCROLL
        if(mouseScroll != 0 && itemOver.GetComponent<IScrollable>() != null) {
            itemOver.GetComponent<IScrollable>().Scroll(mouseScroll);
        }
    }

    private void PayItem() {
        //Place item on itemover(storage)
        GameObject giveItem = rightHand.GetChild(0).gameObject;
        if(giveItem) pdi.TryGrabIK(giveItem.transform);
        IPayable payable = itemOver.GetComponent<IPayable>();
        payable.Pay(giveItem.GetComponent<Value>().value);

        Destroy(giveItem);

        //Remove onhand from ui
        Destroy(onhandUI.GetChild(0).gameObject);

        //audio
        am.PlayUI(0);
    }

    public void GrabAllFromStorage(StorageHandler storage) {
        for(int i = 0; i < maxItemsOnHand; i++) {
            List<GameObject> items = storage.items;
            // print("i: " + i + " items: " + items.Count + " right hand items: " + rightHand.childCount);
            if(items.Count == 0 || rightHand.childCount == maxItemsOnHand) break;

            if(items[0]) pdi.TryGrabIK(items[0].transform);
            rightHand.GetComponent<StorageHandler>().AddItemRandom(items[0]);
        }
        UpdateOnhandUI();

        //audio
        am.PlayUI(0);
    }

    public void PutAllInStorage(StorageHandler storage) {
        List<GameObject> toStore = new List<GameObject>();

        foreach(Transform item in rightHand) {
            if(item) pdi.TryGrabIK(item.transform);
            toStore.Add(item.gameObject);
        }

        foreach(GameObject item in toStore) {
            storage.AddItemRandom(item.gameObject);
        }
        UpdateOnhandUI();

        //audio
        am.PlayUI(0);
    }

    private void TakeItemOver() {
        if(rightHand.childCount >= maxItemsOnHand) {
            NotificationManager.current.NewNotif("HANDS FULL", "You are holding too many items! ");
            // AudioManager.current.PlayUI(7);
            return;
        }

        if(itemOver) pdi.TryGrabIK(itemOver.transform);

        rightHand.GetComponent<StorageHandler>().AddItemRandom(itemOver);

        UpdateOnhandUI();

        //audio
        am.PlayUI(0);
    }

    private void PlaceItem() {
        //Place item on itemover(storage)
        GameObject dropItem = rightHand.GetChild(0).gameObject;

        if(itemOver.GetComponent<StorageHandler>()) {
            StorageHandler storage = itemOver.GetComponent<StorageHandler>();
            
            pdi.TryGrabIK(itemOver.transform);

            storage.AddItem(dropItem, hit.point);
        }

        UpdateOnhandUI();

        //audio
        am.PlayUI(0);
    }

    public void ClearItems() {
        // print("clearitems");
        rightHand.GetComponent<StorageHandler>().Clear();
        ClearOnhandUI();
        // UpdateOnhandUI();
    }

    public void UpdateOnhandUI() {
        // print("updateonhandui");
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
        // print("clearonhandui");
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
            am.PlayUI(6);
        } else if(!inAreaUI.isIn && go.layer == layerArea && go.GetComponent<DropSpot>()) { // && areaUI.text != other.name
            inArea = true;
            inAreaUI.In();

            if(go.GetComponent<DropSpot>().isIllegal) {
                inAreaUI.GetComponent<Image>().color = red;
                inAreaUI.transform.GetChild(0).GetComponent<TMP_Text>().color = red;
                inAreaUI.transform.GetChild(0).GetComponent<TMP_Text>().text = "YOU ARE IN AN ILLEGAL UNLOADING AREA";
            } else {
                inAreaUI.GetComponent<Image>().color = green;
                inAreaUI.transform.GetChild(0).GetComponent<TMP_Text>().color = green;
                inAreaUI.transform.GetChild(0).GetComponent<TMP_Text>().text = "YOU ARE IN AN UNLOADING AREA";
            }

            //Audio
            am.PlayUI(8);
        }

        if(go.CompareTag("StartShiftTrigger")) {
            TimeManager.current.TryStartShift();
        } else if(go.CompareTag("EndShiftTrigger")) {
            TimeManager.current.TryEndShift();
            GameManager.current.SetPlayerHouse(go.GetComponent<House>().houseNum);
            // GameManager.current.playerHouse = go.GetComponent<House>().houseNum;
        }

        //Audio Area
        if(go.CompareTag("AudioArea")) am.NewAmb(go.GetComponent<AudioArea>().audioInt);
    }

    private void OnTriggerStay(Collider other) {
        // GameObject go = osther.gameObject;
    }

    private void OnTriggerExit(Collider other) {
        GameObject go = other.gameObject;
        if(playerDestUI.isIn && go.layer == layerDrop) {
            inDrop = false;
            playerDestUI.Out();

            //Audio
            am.PlayUI(7);
        }
        if(inAreaUI.isIn && go.layer == layerArea) {
            inAreaUI.Out();
        }
    }
}