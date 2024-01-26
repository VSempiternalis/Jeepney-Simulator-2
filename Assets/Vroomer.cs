using UnityEngine;

public class Vroomer : MonoBehaviour {
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    private float currentSpeed;

    private Rigidbody rb;
    private AudioSource audioSource;

    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;
    private float pitchRatio;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate() {
        currentSpeed = rb.velocity.magnitude;

        if(currentSpeed <= minSpeed) {
            audioSource.pitch = minPitch;
        }

        else if(currentSpeed > maxSpeed) {
            audioSource.pitch = maxPitch;
        }

        else {
            pitchRatio = currentSpeed / maxSpeed;
            audioSource.pitch = minPitch + (pitchRatio*maxPitch);
        }
    }
}