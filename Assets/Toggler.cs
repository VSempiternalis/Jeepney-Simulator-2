using UnityEngine;

public class Toggler : MonoBehaviour, IInteractable, ITooltipable {
    [SerializeField] private GameObject toToggle;
    private AudioSource audioSource;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        
    }

    public void Interact(GameObject interactor) {
        toToggle.SetActive(!toToggle.activeSelf);
        if(audioSource)     audioSource.Play();
    }

    public string GetHeader() {
        return "LIGHT";
    }

    public string GetText() {
        return "Click to toggle on/off";
    }
}