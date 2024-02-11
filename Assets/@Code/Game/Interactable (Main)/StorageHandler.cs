using UnityEngine;
using System.Collections.Generic;

public class StorageHandler : MonoBehaviour, ITooltipable { 
    public int value;
    public List<GameObject> items;
    private AudioHandler audioHandler;

    private void Start() {
        audioHandler = GetComponent<AudioHandler>();
    }

    private void Update() {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Money")) print("Trigger");
    }

    private void AddValue(int newValue) {
        value += newValue;
    }

    public void AddItem(GameObject newItem) {
        //Remove from storage
        // print("newItem.GetComponent<ItemHandler>().storage: " + newItem.GetComponent<ItemHandler>().storage);
        // print("newItem.GetComponent<ItemHandler>().storage.GetComponent<StorageHandler>(): " + newItem.GetComponent<ItemHandler>().storage.GetComponent<StorageHandler>());
        if(newItem.GetComponent<ItemHandler>().storage != null && newItem.GetComponent<ItemHandler>().storage.GetComponent<StorageHandler>()) {
            newItem.GetComponent<ItemHandler>().storage.GetComponent<StorageHandler>().RemoveItem(newItem);
        }

        items.Add(newItem);
        newItem.GetComponent<ItemHandler>().storage = transform;
        AddValue(newItem.GetComponent<Value>().value);
        //Set random position within placeArea
        // float spawnX = Random.Range(transform.localPosition.x - (transform.localScale.x/2), transform.localPosition.x + (transform.localScale.x/2));
        // float spawnZ = Random.Range(transform.localPosition.z - (transform.localScale.z/2), transform.localPosition.z + (transform.localScale.z/2));
        float spawnX = Random.Range(-0.5f, 0.5f);
        float spawnZ = Random.Range(-0.5f, 0.5f);
        
        float rotY = Random.Range(0, 361); //Technically, it should be 360 but whatever
        
        // newItem.transform.localPosition = new Vector3(spawnX, transform.localPosition.y - 0.5f, spawnZ);
        // newItem.transform.localPosition = new Vector3(spawnX, transform.localPosition.y  + transform.localScale.y, spawnZ);
        LeanTween.moveLocal(newItem, new Vector3(spawnX, transform.localPosition.y, spawnZ), 0.5f).setEaseInOutExpo();
        // newItem.transform.Rotate(new Vector3(0, 0, 0));
        newItem.transform.Rotate(new Vector3(0, rotY, 0));
        newItem.transform.rotation = Quaternion.identity;

        newItem.transform.SetParent(transform);

        audioHandler.Play(0);
    }

    public void RemoveItem(GameObject removeItem) {
        removeItem.GetComponent<ItemHandler>().storage = null;
        AddValue(-removeItem.GetComponent<Value>().value);
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
