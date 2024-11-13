using UnityEngine;

[ExecuteInEditMode]
public class Zoom : MonoBehaviour {
    // [SerializeField] private PlayerInteraction player;
    // [SerializeField] private bool canScroll;

    // Camera cam;
    // public float defaultFOV = 60;
    // public float maxZoomFOV = 15;
    // [Range(0, 1)]
    // public float currentZoom;
    // public float sensitivity = 1;

    // //REWORK
    // [Range(0, 5)]
    // private int zoomLevel; //0 is standard, 5 is max
    // private int zoomAdd; //amount of FOV added per zoom level


    // private void Awake() {
    //     // Get the camera on this gameObject and the defaultZoom.
    //     cam = GetComponent<Camera>();
    //     if(cam) defaultFOV = cam.fieldOfView;
    // }

    // private void Update() {
    //     if(player != null) canScroll = player.CanScroll();

    //     // Update the currentZoom and the camera's fieldOfView.
    //     if(Input.mouseScrollDelta.y != 0 && canScroll) {
    //         currentZoom += Input.mouseScrollDelta.y * sensitivity * .05f;
    //         currentZoom = Mathf.Clamp01(currentZoom);
    //         cam.fieldOfView = Mathf.Lerp(defaultFOV, maxZoomFOV, currentZoom);
    //     }
    // }
}
