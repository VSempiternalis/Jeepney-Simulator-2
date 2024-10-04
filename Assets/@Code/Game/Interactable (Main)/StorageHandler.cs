using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class StorageHandler : MonoBehaviour, ITooltipable { 
    [SerializeField] private string header;
    [SerializeField] private string controls;
    [SerializeField] [TextArea] private string desc;

    public int value;
    public List<GameObject> items;
    private AudioHandler audioHandler;
    [SerializeField] private Transform goober; //hitpos to localPos
    [SerializeField] private bool isPayPoint; //updates player UI

    [System.Serializable]
    public class OnAddItem : UnityEvent {}
    public OnAddItem onAddItem;

    private void Start() {
        audioHandler = GetComponent<AudioHandler>();
    }

    private void Update() {

    }

    private void OnTriggerEnter(Collider other) {
        // if(other.gameObject.CompareTag("Money")) print("Trigger");
    }

    private void AddValue(int newValue) {
        value += newValue;

        if(isPayPoint) {
            if(value > 0) PayChangeUIManager.current.SetStorageValue(value);
            else PayChangeUIManager.current.SetStorageValue(0);
        }
    }

    public void AddItem(GameObject newItem, Vector3 placePos) {
        goober.position = placePos;

        //Remove new item from storage if stored
        ItemHandler newItemHandler = newItem.GetComponent<ItemHandler>();
        if(newItemHandler.storage != null && newItemHandler.storage.GetComponent<StorageHandler>()) {
            newItemHandler.storage.GetComponent<StorageHandler>().RemoveItem(newItem);
        }

        items.Add(newItem);
        newItemHandler.storage = transform;
        if(newItem.GetComponent<Value>()) AddValue(newItem.GetComponent<Value>().value);

        float rotY = Random.Range(0, 361); //Technically, it should be 360 but whatever

        newItem.transform.eulerAngles = new Vector3(0, rotY, 0);
        // newItem.transform.Rotate(new Vector3(0, rotY, 0));
        // newItem.transform.rotation = Quaternion.identity;

        newItem.transform.SetParent(transform);
        LeanTween.moveLocal(newItem, new Vector3(goober.localPosition.x, goober.localPosition.y + newItemHandler.yPlaceOffset, goober.localPosition.z), 0.25f).setEaseInOutExpo();
        // LeanTween.move(newItem, new Vector3(placePos.x, placePos.y + newItemHandler.yPlaceOffset, placePos.z), 0.25f).setEaseInOutExpo();

        if(audioHandler) audioHandler.Play(newItemHandler.placeAudioInt);

        onAddItem?.Invoke();
    }

    public void AddItemRandom(GameObject newItem) {
        if(newItem.GetComponent<ItemHandler>().storage != null && newItem.GetComponent<ItemHandler>().storage.GetComponent<StorageHandler>()) {
            newItem.GetComponent<ItemHandler>().storage.GetComponent<StorageHandler>().RemoveItem(newItem);
        }

        items.Add(newItem);
        newItem.GetComponent<ItemHandler>().storage = transform;
        if(newItem.GetComponent<Value>()) AddValue(newItem.GetComponent<Value>().value);
        //Set random position within placeArea
        float spawnX = Random.Range(-0.4f, 0.4f);
        float spawnZ = Random.Range(-0.4f, 0.4f);
        // goober.localPosition = new Vector3(spawnX, transform.localPosition.y, spawnZ);
        
        float rotY = Random.Range(0, 360);

        newItem.transform.SetParent(transform);
        // LeanTween.moveLocal(newItem, new Vector3(goober.localPosition.x, transform.localPosition.y, goober.localPosition.z), 0.25f).setEaseInOutExpo();
        LeanTween.moveLocal(newItem, new Vector3(spawnX, transform.localPosition.y, spawnZ), 0.35f).setEaseInOutExpo();
        // newItem.transform.localPosition = new Vector3(spawnX, transform.localPosition.y, spawnZ);
        newItem.transform.eulerAngles = new Vector3(0, rotY, 0);

        if(audioHandler) audioHandler.Play(newItem.GetComponent<ItemHandler>().placeAudioInt);
    }

    public void RemoveItem(GameObject removeItem) {
        removeItem.GetComponent<ItemHandler>().storage = null;
        if(removeItem.GetComponent<Value>()) AddValue(-removeItem.GetComponent<Value>().value);
        items.Remove(removeItem);
    }

    public void DestroyItem(GameObject removeItem) {
        RemoveItem(removeItem);

        Destroy(removeItem);
    }

    public void Clear() {
        List<GameObject> removeList = new List<GameObject>();

        foreach(GameObject item in items) {
            removeList.Add(item);
        }

        foreach(GameObject item in removeList) {
            if(item != null) {
                RemoveItem(item);
                Destroy(item);
            }
        }
    }

    public string GetHeader() {
        if(header != "") return "Storage Mat";
        else return header;
    }

    public string GetControls() {
        return "[R Mouse] Place item\n[Mid Mouse] Take all items";
    }

    public string GetDesc() {
        return desc;
    }
}
