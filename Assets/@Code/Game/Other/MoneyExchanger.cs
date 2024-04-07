using UnityEngine;
using System.Collections.Generic;

public class MoneyExchanger : MonoBehaviour, IPayable, ITooltipable {
    [SerializeField] private StorageHandler placeArea;

    [SerializeField] private GameObject coin1PF;
    [SerializeField] private GameObject coin5PF;
    [SerializeField] private GameObject coin10PF;
    [SerializeField] private GameObject cash20PF;
    [SerializeField] private GameObject cash50PF;
    private List<GameObject> money = new List<GameObject>();

    [SerializeField] private string header;
    [SerializeField] private string controls;
    [SerializeField] [TextArea] private string desc;

    private void Start() {
        money.Add(coin1PF);
        money.Add(coin5PF);
        money.Add(coin10PF);
        money.Add(cash20PF);
        money.Add(cash50PF);
    }

    public void Pay(int moneyToExchange) {
        int returnMoney = 0;

        for(int i = money.Count; i > 0; i--) {
            while(returnMoney < moneyToExchange) {
                int newMoneyValue = money[i-1].GetComponent<Value>().value;
                if(newMoneyValue >= moneyToExchange && moneyToExchange != 1) break;

                else if(returnMoney + newMoneyValue <= moneyToExchange) {
                    GameObject newMoney = Instantiate(money[i-1]);
                    newMoney.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                    placeArea.AddItemRandom(newMoney);
                    // newMoney.transform.SetParent(placeArea);

                    newMoney.name = "Money - P" + newMoney.GetComponent<Value>().value;
                    returnMoney += newMoney.GetComponent<Value>().value;
                } else {
                    break;
                }
            }
        }
    }

    public string GetHeader() {
        return header; //"ALBERTO";
    }

    public string GetControls() {
        return controls; //"ALBERTO";
    }

    public string GetDesc() {
        return desc; //"R Click me while holding the money you want exchanged";
    }
}
