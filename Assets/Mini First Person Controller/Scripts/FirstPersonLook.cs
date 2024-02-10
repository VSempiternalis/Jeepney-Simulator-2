using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    public bool isOn = true;
    [SerializeField]
    Transform character;
    public float sensitivity = 2;
    public float smoothing = 1.5f;

    Vector2 velocity;
    Vector2 frameVelocity;
    [SerializeField] private PlayerDriveInput pdi;

    void Reset()
    {
        // Get the character from the FirstPersonMovement in parents.
        character = GetComponentInParent<FirstPersonMovement>().transform;
    }

    void Start()
    {
        // Lock the mouse cursor to the game screen.
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if(!isOn) return;

        // Get smooth velocity.
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        if(pdi.isDriving) velocity.y = Mathf.Clamp(velocity.y, -90, 60);
        else if(rb.velocity.magnitude >= 4f) velocity.y = Mathf.Clamp(velocity.y, -90, 10);
        else velocity.y = Mathf.Clamp(velocity.y, -90, 55);

        // Rotate camera up-down and controller left-right from velocity.
        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
    }
}
