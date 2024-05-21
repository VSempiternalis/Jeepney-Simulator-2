using UnityEngine;

public class WithdrawButton : MonoBehaviour {
    [SerializeField] private GameObject objectToWithdrawPF;
    // [SerializeField] private int cost;
    // [SerializeField] private int withdrawCost;
    [SerializeField] private StorageHandler placeArea;
    [SerializeField] private Transform withdrawPoint;

    private void Start() {
        
    }

    private void Update() {
        
    }

    public void Withdraw() {
        int value = objectToWithdrawPF.GetComponent<Value>().value;

        if(BoundaryManager.current.deposit < value) {
            NotificationManager.current.NewNotifColor("INSUFFICIENT FUNDS!", "I don't have enough money deposited to withdraw P" + value, 2);
            return;
        }
        
        GameObject newObject = Instantiate(objectToWithdrawPF);
        newObject.transform.position = withdrawPoint.position;
        newObject.name = "Money - P" + value;
        BoundaryManager.current.AddToDeposit(-value);

        placeArea.GetComponent<StorageHandler>().AddItemRandom(newObject);
        // GetComponent<WorldButton>().pressed = false;
    }
}
