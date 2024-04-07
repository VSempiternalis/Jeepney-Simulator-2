using UnityEngine;

public class Toggler : MonoBehaviour, IInteractable, ITooltipable {
    [SerializeField] private string header;
    [SerializeField] private string controls;
    [SerializeField] private string desc;

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
        return header;
    }

    public string GetControls() {
        return controls;
    }

    public string GetDesc() {
        return desc;
    }
}