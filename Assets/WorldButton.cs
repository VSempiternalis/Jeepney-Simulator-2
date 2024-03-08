using UnityEngine;
using UnityEngine.Events;

public class WorldButton : MonoBehaviour, IInteractable {
    private bool isOn;
    [SerializeField] private Vector3 onPosition;
    [SerializeField] private Vector3 offPosition;
    [SerializeField] private float pressTime;
    [SerializeField] private AudioHandler audioHandler;

    [System.Serializable]
    public class OnClickEvent : UnityEvent {}
    public OnClickEvent onClickEvent;

    private void Start() {
        
    }

    private void Update() {
        
    }

    public void Interact(GameObject player) {
        isOn = !isOn;

        //Animation
        if(isOn) LeanTween.moveLocal(gameObject, onPosition, pressTime).setEaseOutElastic();
        else LeanTween.moveLocal(gameObject, offPosition, pressTime).setEaseOutElastic();

        onClickEvent.Invoke();

        //Audio
        if(audioHandler) {
            audioHandler.Play(1);
            if(isOn) audioHandler.Play(2);
            else audioHandler.Play(3);
        }
    }
}