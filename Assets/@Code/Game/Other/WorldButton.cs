using UnityEngine;
using UnityEngine.Events;

public class WorldButton : MonoBehaviour, IInteractable, ITooltipable {
    public bool isOn;
    [SerializeField] private bool isMovable;
    public GameObject interactor;
    [SerializeField] private Vector3 onPosition;
    [SerializeField] private Vector3 offPosition;
    [SerializeField] private float pressTime;
    [SerializeField] private bool isAudioUI;
    [SerializeField] private AudioHandler audioHandler;

    [System.Serializable]
    public class OnClickEvent : UnityEvent {}
    public OnClickEvent onClickEvent;

    [SerializeField] private string header;
    [TextArea] [SerializeField] private string desc;

    private void Start() {
        
    }

    private void Update() {
        
    }

    public void Interact(GameObject player) {
        interactor = player;
        isOn = !isOn;

        //Animation
        if(isMovable) {
            if(isOn) LeanTween.moveLocal(gameObject, onPosition, pressTime).setEaseOutElastic();
            else LeanTween.moveLocal(gameObject, offPosition, pressTime).setEaseOutElastic();
        }

        onClickEvent.Invoke();

        //AUDIO
        if(isAudioUI) {
            AudioManager.current.PlayUI(1);
        } else if(audioHandler) {
            audioHandler.Play(1);
            if(isOn) audioHandler.Play(2);
            else audioHandler.Play(3);
        }
    }

    public string GetHeader() {
        return header;
    }

    public string GetControls() {
        return "[L Mouse] Toggle on/off";
    }

    public string GetDesc() {
        return desc;
    }
}