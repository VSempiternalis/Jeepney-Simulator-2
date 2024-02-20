using UnityEngine;

[ExecuteInEditMode]
public class Zoom : MonoBehaviour {
    [SerializeField] private PlayerInteraction player;

    Camera camera;
    public float defaultFOV = 60;
    public float maxZoomFOV = 15;
    [Range(0, 1)]
    public float currentZoom;
    public float sensitivity = 1;


    void Awake()
    {
        // Get the camera on this gameObject and the defaultZoom.
        camera = GetComponent<Camera>();
        if (camera)
        {
            defaultFOV = camera.fieldOfView;
        }
    }

    void Update()
    {
        // Update the currentZoom and the camera's fieldOfView.
        if(Input.mouseScrollDelta.y != 0 && player.CanScroll()) {
            currentZoom += Input.mouseScrollDelta.y * sensitivity * .05f;
            currentZoom = Mathf.Clamp01(currentZoom);
            camera.fieldOfView = Mathf.Lerp(defaultFOV, maxZoomFOV, currentZoom);
        }
    }
}
