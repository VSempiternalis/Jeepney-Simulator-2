using UnityEngine;

public class FinishShiftButton : MonoBehaviour, IInteractable {
    private GameManager gameManager;
    private int boundary;

    private void Start() {
        gameManager = GameManager.current;
    }

    private void Update() {
        
    }
    
    public void Interact(GameObject interactor) {
        if(1 > gameManager.boundary) {
            //MADE BOUNDARY
            //fired
        } else {
            //DIDNT MAKE BOUNDARY
            //fader
        }
    }
}
