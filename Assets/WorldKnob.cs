using UnityEngine;
using UnityEngine.Events;

public class WorldKnob : MonoBehaviour, IKnob, ITooltipable {
    [SerializeField] private Vector3 onRotation;
    [SerializeField] private Vector3 offRotation;
    [SerializeField] private float rotationAdd;
    [SerializeField] private float pressTime;

    [System.Serializable]
    public class OnClickEvent : UnityEvent {}
    public OnClickEvent onLeftClickEvent;
    public OnClickEvent onRightClickEvent;

    [SerializeField] private string header;
    [SerializeField] private string desc;

    private void Start() {
        
    }

    private void Update() {
        
    }

    public void LeftClick() {
        onLeftClickEvent.Invoke();

        LeanTween.rotateAroundLocal(gameObject, Vector3.up, -rotationAdd, pressTime).setEasePunch();
        // LeanTween.rotateLocal(gameObject, onRotation, pressTime).setEasePunch();
    }

    public void RightClick() {
        onRightClickEvent.Invoke();

        LeanTween.rotateAroundLocal(gameObject, Vector3.up, rotationAdd, pressTime).setEasePunch();
        // LeanTween.rotateLocal(gameObject, offRotation, pressTime).setEasePunch();
    }

    public string GetHeader() {
        return header;
    }

    public string GetControls() {
        return "[L CLICK] to increase\n[R CLICK] to decrease";
    }

    public string GetDesc() {
        return desc;
    }
}
