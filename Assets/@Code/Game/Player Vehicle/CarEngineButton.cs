using UnityEngine;

public class CarEngineButton : MonoBehaviour, IInteractable, ITooltipable {
    [SerializeField] private string desc;
    
    [SerializeField] private CarController carCon;
    // public bool isOn;
    [SerializeField] private Vector3 onPosition;
    [SerializeField] private Vector3 offPosition;
    [SerializeField] private float pressTime;
    [SerializeField] private AudioHandler audioHandler;
    [SerializeField] private AudioSource audioSource;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        
    }

    public void Interact(GameObject player) {
        if(carCon.isEngineOn && carCon.fuelAmount <= 0) {
            print("CANT INTERACT. NO FUEL LEFT");
            carCon.SetEngine(false);
            if(!audioSource.gameObject.activeSelf) audioHandler.Play(3);
            return;
        }

        // carCon.isEngineOn = !carCon.isEngineOn;
        carCon.SetEngine(!carCon.isEngineOn);

        //Animation
        if(carCon.isEngineOn) LeanTween.moveLocal(gameObject, onPosition, pressTime).setEaseOutElastic();
        else LeanTween.moveLocal(gameObject, offPosition, pressTime).setEaseOutElastic();

        //Audio
        if(!audioSource.isPlaying) { //stops repeating audio bug
            audioHandler.Play(1);
            if(carCon.isEngineOn) audioHandler.Play(2);
            else audioHandler.Play(3);
        }
    }

    public string GetHeader() {
        return "Engine Button";
    }

    public string GetControls() {
        return "[L Mouse] Toggle engine";
    }

    public string GetDesc() {
        return desc;
    }
}
