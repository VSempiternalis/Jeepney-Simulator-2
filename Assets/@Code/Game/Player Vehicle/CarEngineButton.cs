using UnityEngine;

public class CarEngineButton : MonoBehaviour, IInteractable {
    [SerializeField] private CarController carCon;
    private bool isOn;
    [SerializeField] private Vector3 onPosition;
    [SerializeField] private Vector3 offPosition;
    [SerializeField] private float pressTime;
    [SerializeField] private AudioHandler audioHandler;

    private void Start() {
        
    }

    private void Update() {
        
    }

    public void Interact(GameObject player) {
        isOn = !isOn;
        carCon.SetEngine(isOn);

        //Animation
        if(isOn) LeanTween.moveLocal(gameObject, onPosition, pressTime).setEaseOutElastic();
        else LeanTween.moveLocal(gameObject, offPosition, pressTime).setEaseOutElastic();

        //Audio
        audioHandler.Play(1);
        if(isOn) audioHandler.Play(2);
        else audioHandler.Play(3);
    }
}
