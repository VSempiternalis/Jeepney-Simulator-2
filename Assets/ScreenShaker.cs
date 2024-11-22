using UnityEngine;

public class ScreenShaker : MonoBehaviour {
    public static ScreenShaker current;
    private Transform parent;

    private Camera cam;
    public float mainFOV;
    public float maxFOV;
    private float currentFOV;
    private float dynamicFOVAdd = 0;

    //ZOOM
    [SerializeField] private PlayerInteraction player;
    [SerializeField] private bool canScroll;
    [Range(0, 6)]
    private int zoomLevel = 0; //0 is standard, 5 is max
    private int zoomAdd = 10; //amount of FOV added per zoom level
    private int zoomFOVAdd; //amount current zoom adds to main fov

    private bool isShaking;

    private void Awake() {
        current = this;
    }

    private void Start() {
        parent = transform.parent;
        cam = GetComponent<Camera>();
        mainFOV = cam.fieldOfView;
        currentFOV = mainFOV;
    }

    private void Update() {
        SetZoom();
        // SetDynamicFOV();

        cam.fieldOfView = mainFOV + dynamicFOVAdd - zoomFOVAdd;
    }

    private void SetZoom() {
        if(player != null) canScroll = player.CanScroll();

        // Update the currentZoom and the camera's fieldOfView.
        if(canScroll) {
            if(Input.mouseScrollDelta.y > 0 && zoomLevel < 5) {
                zoomLevel ++;
                zoomFOVAdd = zoomLevel * zoomAdd;
                // currentFOV += zoomFOVAdd;
            }
            else if(Input.mouseScrollDelta.y < 0 && zoomLevel > 0) {
                zoomLevel --;
                zoomFOVAdd = zoomLevel * zoomAdd;
                // currentFOV += zoomFOVAdd;
            }
        }
    }

    public void SetMainFOV(float newFOV) {
        mainFOV = newFOV;
        maxFOV = mainFOV + 30f;
    }

    public void Shake(float duration, float magnitude) {
        print("SHAKING: " + magnitude);

        if(isShaking) return;
        isShaking = true;

        // Store the original position
        Vector3 originalPosition = parent.localPosition;

        // Start the shake effect
        LeanTween.moveLocalX(parent.gameObject, originalPosition.x + Random.Range(-magnitude, magnitude), duration / 2f)
            .setEaseShake()
            // .setLoopPingPong(1)
            .setOnComplete(() => {
                // Reset the position after shaking
                LeanTween.moveLocalX(parent.gameObject, originalPosition.x, duration*1.5f)
                    .setEaseOutQuart()
                    .setOnComplete(() => {
                        isShaking = false;
                    });
            });
        
        // // LeanTween.moveLocalX(parent.gameObject, originalPosition.x, duration / 2f)
        // //     .setEase(LeanTweenType.easeInShakesetEaseShake)
        // //     .setOnComplete(() => {
        // //         // Reset the position after shaking
        // //         localPosition = originalPosition;
        // //     });
        
        LeanTween.moveLocalY(parent.gameObject, originalPosition.y + Random.Range(-magnitude, magnitude), duration / 2f)
            .setEaseShake()
            // .setLoopPingPong(1)
            .setOnComplete(() => {
                // Reset the position after shaking
                LeanTween.moveLocalY(parent.gameObject, originalPosition.y, duration*1.5f)
                    .setEaseOutQuart();
            });
    }

    public void DynamicFOV(float t) {
        dynamicFOVAdd = Mathf.Lerp(0, 30, t);
    }
}
