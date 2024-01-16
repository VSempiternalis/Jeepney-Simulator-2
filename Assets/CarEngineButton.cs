using UnityEngine;

public class CarEngineButton : MonoBehaviour, IInteractable {
    [SerializeField] private CarController carCon;
    private bool isOn;
    [SerializeField] private Vector3 onPosition;
    [SerializeField] private Vector3 offPosition;
    [SerializeField] private float pressTime;

    private void Start() {
        
    }

    private void Update() {
        
    }

    public void Interact(GameObject player) {
        isOn = !isOn;
        carCon.isEngineOn = isOn;

        if(isOn) LeanTween.moveLocal(gameObject, onPosition, pressTime).setEaseOutElastic();
        else LeanTween.moveLocal(gameObject, offPosition, pressTime).setEaseOutElastic();
    }
}
