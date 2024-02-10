using UnityEngine;

public class DriversSeat : MonoBehaviour, IInteractable, ITooltipable {
    [SerializeField] private string header;
    [SerializeField] private string text;
    [SerializeField] private CarController carCon;
    [SerializeField] private AudioSource audioSource;

    private void Start() {
        
    }

    private void Update() {
        
    }

    public void Interact(GameObject interactor) {
        carCon.ToggleDriverSeat(interactor.transform);

        audioSource.Play();
    }

    public string GetHeader() {
        return header;
    }

    public string GetText() {
        return text;
    }
}
