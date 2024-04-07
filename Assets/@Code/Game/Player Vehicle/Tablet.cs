using UnityEngine;

public class Tablet : MonoBehaviour {
    private bool isIn;
    
    [Space(10)]
    [Header("IN")]
    public Vector3 inTargetPosition;
    public Vector3 inTargetRotation;
    public float inTime = 1.0f;
    public LeanTweenType inEasingType = LeanTweenType.easeOutQuart;

    [Space(10)]
    [Header("OUT")]
    public Vector3 outTargetPosition;
    public Vector3 outTargetRotation;
    public float outTime = 1.0f;
    public LeanTweenType outEasingType = LeanTweenType.easeOutQuart;

    private void Start() {
        
    }

    private void Update() {
        
    }

    public void Toggle() {
        isIn = !isIn;
        if(isIn) In();
        else Out();

        //audio
    }

    private void In() {
        LeanTween.moveLocal(gameObject, inTargetPosition, inTime).setEase(inEasingType);
        LeanTween.rotateLocal(gameObject, inTargetRotation, inTime).setEase(inEasingType);
    }

    public void Out() {
        LeanTween.moveLocal(gameObject, outTargetPosition, outTime).setEase(outEasingType);
        LeanTween.rotateLocal(gameObject, outTargetRotation, outTime).setEase(outEasingType);
    }
}