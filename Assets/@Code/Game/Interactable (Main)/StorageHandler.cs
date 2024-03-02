using UnityEngine;
using System.Collections.Generic;

public class StorageHandler : MonoBehaviour, ITooltipable { 
    public int value;
    public List<GameObject> items;
    private AudioHandler audioHandler;
    [SerializeField] private Transform goober; //hitpos to localPos
    [SerializeField] private bool isPayPoint; //updates player UI

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

        if(audioHandler) audioHandler.Play(0);
    }

    public void AddItemRandom(GameObject newItem) {
        // print("Adding item: " + newItem.name);
        //Remove new item from storage if stored
        // print("newItem.GetComponent<ItemHandler>().storage: " + newItem.GetComponent<ItemHandler>().storage);
        // print("newItem.GetComponent<ItemHandler>().storage.GetComponent<StorageHandler>(): " + newItem.GetComponent<ItemHandler>().storage.GetComponent<StorageHandler>());
        if(newItem.GetComponent<ItemHandler>().storage != null && newItem.GetComponent<ItemHandler>().storage.GetComponent<StorageHandler>()) {
            newItem.GetComponent<ItemHandler>().storage.GetComponent<StorageHandler>().RemoveItem(newItem);
        }

        items.Add(newItem);
        newItem.GetComponent<ItemHandler>().storage = transform;
        if(newItem.GetComponent<Value>()) AddValue(newItem.GetComponent<Value>().value);
        //Set random position within placeArea
        // float spawnX = Random.Range(transform.localPosition.x - (transform.localScale.x/2), transform.localPosition.x + (transform.localScale.x/2));
        // float spawnZ = Random.Range(transform.localPosition.z - (transform.localScale.z/2), transform.localPosition.z + (transform.localScale.z/2));
        float spawnX = Random.Range(-0.5f, 0.5f);
        float spawnZ = Random.Range(-0.5f, 0.5f);
        
        float rotY = Random.Range(0, 361); //Technically, it should be 360 but whatever
        
        // newItem.transform.localPosition = new Vector3(spawnX, transform.localPosition.y - 0.5f, spawnZ);
        // newItem.transform.localPosition = new Vector3(spawnX, transform.localPosition.y  + transform.localScale.y, spawnZ);
        LeanTween.moveLocal(newItem, new Vector3(spawnX, transform.localPosition.y, spawnZ), 0.25f).setEaseInOutExpo();
        // newItem.transform.Rotate(new Vector3(0, 0, 0));
        newItem.transform.eulerAngles = new Vector3(0, rotY, 0);
        // newItem.transform.Rotate(new Vector3(0, rotY, 0));
        // newItem.transform.rotation = Quaternion.identity;

        newItem.transform.SetParent(transform);

        if(audioHandler) audioHandler.Play(0);
    }

    public void RemoveItem(GameObject removeItem) {
        removeItem.GetComponent<ItemHandler>().storage = null;
        if(removeItem.GetComponent<Value>()) AddValue(-removeItem.GetComponent<Value>().value);
        items.Remove(removeItem);
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
        return "STORAGE";
    }

    public string GetText() {
        return "A place to store your valuables";
    }
}
