using UnityEngine;
using System.Collections;
using TMPro;

public class FuelPump : MonoBehaviour, IPayable, ITooltipable {
    public int deposit;
    // private GameObject interactor;
    [SerializeField] private CarController carCon;
    [SerializeField] private TMP_Text depositText;
    private AudioHandler audioHandler;

    [SerializeField] private WorldButton button;
    private bool isOn;
    private bool isPumpingFuel;

    private void Start() {
        audioHandler = GetComponent<AudioHandler>();
    }

    private void Update() {
        
    }

    public void Pay(int newAmount) {
        deposit += newAmount;
        depositText.text = "P" + deposit;

        if(audioHandler) audioHandler.Play(0);
    }

    private IEnumerator PumpingFuel() {
        isPumpingFuel = true;
        while(true) {
            if(carCon != null && (carCon.fuelAmount < carCon.fuelCapacity) && deposit > 0) {
                print("Adding fuel");
                Pay(-(GameManager.current.pricePerLiter/20));
                carCon.AddFuel(50); //5 secs per liter

                if(audioHandler) audioHandler.Play(1);
            } else {
                StopPumpingFuel();

                print("Stop Pumping fuel");
                yield break;
            }

            yield return new WaitForSeconds(0.25f);
        }
    }

    public void Interaction() {
        isOn = !isOn;
        print("interactor: " + button.interactor.name);
        // carCon = button.interactor.GetComponent<PlayerDriveInput>().carCon;

        if(isOn && !isPumpingFuel) {
            StartCoroutine(PumpingFuel());
        } else {
            StopCoroutine(PumpingFuel());
            StopPumpingFuel();
        }
    }

    private void StopPumpingFuel() {
        isPumpingFuel = false;
        if(button.isOn) button.Interact(gameObject);

        if(audioHandler) audioHandler.Play(2);
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
