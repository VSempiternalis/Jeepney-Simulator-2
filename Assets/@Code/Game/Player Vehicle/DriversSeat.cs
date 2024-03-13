using UnityEngine;

public class DriversSeat : MonoBehaviour, IInteractable {
    [SerializeField] private string header;
    [SerializeField] private string controls;
    [SerializeField] private string desc;

    [SerializeField] private CarController carCon;
    [SerializeField] private AudioSource audioSource;

    private void Start() {
        
    }

    private void Update() {
        
    }

    public void Interact(GameObject interactor) {
        // carCon.ToggleDriverSeat(interactor.transform, transform);

        audioSource.Play();
    }
}
