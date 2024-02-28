using UnityEngine;

public class uiAnimGroup : MonoBehaviour {
    public bool isIn;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    [SerializeField] private bool isActivateOnStart;

    public enum AnimationType {
        Move,
        Scale,
        Rotate,
        Fade
    }

    [Space(10)]
    [Header("IN")]
    public AnimationType inAnimationType = AnimationType.Move;
    public Vector3 inTargetPosition;
    public float inTime = 1.0f;
    public LeanTweenType inEasingType = LeanTweenType.easeOutQuart;
    [Header("FADE IN")]
    public bool isFadeIn = false; // New variable for fading in
    public float inFadeTime = 1.0f;
    public LeanTweenType inFadeEasingType = LeanTweenType.easeOutQuart;

    [Space(10)]
    [Header("OUT")]
    public AnimationType outAnimationType = AnimationType.Move;
    public Vector3 outTargetPosition;
    public float outTime = 1.0f;
    public LeanTweenType outEasingType = LeanTweenType.easeOutQuart;
    [Header("FADE OUT")]
    public bool isFadeOut = false; // New variable for fading in
    public float outFadeTime = 1.0f;
    public LeanTweenType outFadeEasingType = LeanTweenType.easeOutQuart;

    private void Start() {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if(rectTransform == null) {
            Debug.LogError("UIAnimator requires a RectTransform component.");
            enabled = false;
            return;
        }

        if((inAnimationType == AnimationType.Fade || outAnimationType == AnimationType.Fade) && canvasGroup == null) {
            Debug.LogError("UIAnimator with Fade animation type requires a CanvasGroup component.");
            enabled = false;
            return;
        }

        if(isActivateOnStart) In();
    }

    public void In() {
        switch (inAnimationType) {
            case AnimationType.Move:
                LeanTween.move(rectTransform, inTargetPosition, inTime).setEase(inEasingType);
                break;
            case AnimationType.Scale:
                LeanTween.scale(rectTransform, inTargetPosition, inTime).setEase(inEasingType);
                break;
            case AnimationType.Rotate:
                LeanTween.rotate(rectTransform, inTargetPosition, inTime).setEase(inEasingType);
                break;
            case AnimationType.Fade:
                LeanTween.alphaCanvas(canvasGroup, inTargetPosition.x, inTime).setEase(inEasingType);
                break;
        }

        if(isFadeIn) {
            LeanTween.alphaCanvas(canvasGroup, 1, inFadeTime).setEase(inFadeEasingType);
        }

        isIn = true;
    }

    public void Out() {
        switch (outAnimationType) {
            case AnimationType.Move:
                // print("should be moving");
                LeanTween.move(rectTransform, outTargetPosition, outTime).setEase(outEasingType);
                break;
            case AnimationType.Scale:
                LeanTween.scale(rectTransform, outTargetPosition, outTime).setEase(outEasingType);
                break;
            case AnimationType.Rotate:
                LeanTween.rotate(rectTransform, outTargetPosition, outTime).setEase(outEasingType);
                break;
            case AnimationType.Fade:
                LeanTween.alphaCanvas(canvasGroup, outTargetPosition.x, outTime).setEase(outEasingType);
                break;
        }

        if(isFadeOut) {
            LeanTween.alphaCanvas(canvasGroup, 0, outFadeTime).setEase(outFadeEasingType);
        }

        isIn = false;
    }
}
