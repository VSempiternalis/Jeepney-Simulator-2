using UnityEngine;
using System.Collections;
using TMPro;

public class FuelPump : MonoBehaviour, IPayable, ITooltipable {
    public int deposit;
    // private GameObject interactor;
    [SerializeField] private CarController carCon;
    [SerializeField] private TMP_Text depositText;

    [SerializeField] private WorldButton button;
    private bool isOn;
    private bool isPumpingFuel;

    private AudioManager am;

    private void Start() {
        am = AudioManager.current;
    }

    private void Update() {
        
    }

    public void Pay(int newAmount) {
        AddToDeposit(newAmount);
        am.PlayUI(1);
    }

    private void AddToDeposit(int val) {
        deposit += val;
        depositText.text = "P" + deposit;
    }

    private IEnumerator PumpingFuel() {
        isPumpingFuel = true;
        while(true) {
            if(carCon != null && (carCon.fuelAmount < carCon.fuelCapacity) && deposit > 0) {
                print("Adding fuel");
                AddToDeposit(-GameManager.current.pricePerLiter);
                carCon.AddFuel(1000); //1 secs per liter
            } else {
                StopPumpingFuel();

                print("Stop Pumping fuel");
                yield break;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    public void Interaction() {
        isOn = !isOn;
        // print("interactor: " + button.interactor.name);
        // carCon = button.interactor.GetComponent<PlayerDriveInput>().carCon;

        if(isOn && !isPumpingFuel) {
            StartCoroutine(PumpingFuel());
        } else {
            StopCoroutine(PumpingFuel());
            StopPumpingFuel();
        }

        am.PlayUI(12);
    }

    private void StopPumpingFuel() {
        isPumpingFuel = false;
        if(button.isOn) button.Interact(gameObject);

        // am.PlayUI(2);
    }

    public string GetHeader() {
        return "Fuel Pump";
    }

    public string GetControls() {
        return "[R Mouse] Deposit money into fuel pump";
    }

    public string GetDesc() {
        string returnString = "Deposit money here and then press the shiny, red button!";

        return returnString;
    }
}
