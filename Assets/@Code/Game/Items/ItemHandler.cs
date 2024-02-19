using UnityEngine;

public class ItemHandler : MonoBehaviour, IInteractable {
    public Transform storage;
    public float yPlaceOffset;

    public void TakeItem(Transform newParent) {
        //Put in hand
        transform.SetParent(newParent);
        transform.localPosition = new Vector3(0, 0, 0);
        // GetComponent<Rigidbody>().isKinematic = true;

        //Remove from previous storage
        if(storage != null && storage.GetComponent<StorageHandler>()) {
            storage.GetComponent<StorageHandler>().RemoveItem(gameObject);
        }

        //Set new storage
        GetComponent<ItemHandler>().storage = transform.parent;
    }

    public void Interact(GameObject interactor) {

    }
}
