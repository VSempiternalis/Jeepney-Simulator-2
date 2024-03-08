using UnityEngine;
using TMPro;

public class Fader : MonoBehaviour {
    public static Fader current;
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    // public Image blackScreen;
    public TMP_Text text;

    private float timeSinceLastYawn;
    [SerializeField] private Vector2 yawnIntervalRange;
    private float yawningInterval;
    public bool isYawning;
    private int yawnDuration = 1;

    private void Awake() {
        current = this;
    }

    private void Start() {
        timeSinceLastYawn = 0f;
        ResetYawnInterval();
        isYawning = false;
    }

    private void Update() {
        if(isYawning) {
            timeSinceLastYawn += Time.deltaTime;
            if(timeSinceLastYawn >= yawningInterval) {
                timeSinceLastYawn = 0f;
                ResetYawnInterval();
                Yawn(2, "", yawnDuration);
            }
        }
    }

    public void SetYawning(bool newIsYawning, int duration) {
        isYawning = newIsYawning;
        yawnDuration = duration;
    }

    private void ResetYawnInterval() {
        yawningInterval = Random.Range(yawnIntervalRange.x, yawnIntervalRange.x);
    }

    public void Yawn(float duration, string newText, float blackDuration) {
        FadeToBlack(duration, newText, () => {
            LeanTween.delayedCall(blackDuration, () => {
                FadeFromBlack(duration, newText);
            });
        });
    }

    public void YawnGray(float duration, string newText, float blackDuration) {
        FadeToGray(duration, newText, () => {
            LeanTween.delayedCall(blackDuration, () => {
                FadeFromGray(duration, newText);
            });
        });
    }

    public void FadeFromGray(float duration, string newText, System.Action onComplete = null) {
        text.text = newText;
        fadeCanvasGroup.gameObject.SetActive(true);
        fadeCanvasGroup.alpha = 0.5f;
        LeanTween.alphaCanvas(fadeCanvasGroup, 0f, duration)
            .setEaseInQuart()
            .setOnComplete(() => {
                fadeCanvasGroup.gameObject.SetActive(false);
                if (onComplete != null) {
                    onComplete();
                }
            });
    }

    public void FadeToGray(float duration, string newText, System.Action onComplete = null) {
        text.text = newText;
        fadeCanvasGroup.gameObject.SetActive(true);
        fadeCanvasGroup.alpha = 0f;
        LeanTween.alphaCanvas(fadeCanvasGroup, 0.5f, duration)
            .setEaseInQuart()
            .setOnComplete(() => {
                if (onComplete != null) {
                    onComplete();
                }
            });
    }

    public void BasicFadeFromBlack() {
        FadeFromBlack(1f, "");
    }

    public void SetText(string newText) {
        text.text = newText;
    }

    public void FadeFromBlack(float duration, string newText, System.Action onComplete = null) {
        // DebugText.current.NewText("FADER");
        // DebugText.current.NewText(newText);
        text.text = newText;
        fadeCanvasGroup.gameObject.SetActive(true);
        // DebugText.current.NewText("alpha start: " + fadeCanvasGroup.alpha);
        fadeCanvasGroup.alpha = 1f;
        LeanTween.alphaCanvas(fadeCanvasGroup, 0f, duration)
            .setEaseInQuad()
            .setOnComplete(() => {
                fadeCanvasGroup.gameObject.SetActive(false);
                // DebugText.current.NewText("alpha end: " + fadeCanvasGroup.alpha);
                if (onComplete != null) {
                    onComplete();
                }
            });
    }

    public void FadeToBlack(float duration, string newText, System.Action onComplete = null) {
        text.text = newText;
        fadeCanvasGroup.gameObject.SetActive(true);
        fadeCanvasGroup.alpha = 0f;
        LeanTween.alphaCanvas(fadeCanvasGroup, 1f, duration)
            .setEaseOutQuad()
            .setOnComplete(() => {
                if (onComplete != null) {
                    onComplete();
                }
            });
    }
}
