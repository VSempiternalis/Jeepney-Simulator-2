using UnityEngine;

public class Vroomer : MonoBehaviour {
    private Rigidbody rb;
    private AudioSource audioSource;

    private float currentSpeed;

    private float pitchRatio;

    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;

    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;

    [Space(10)]
    [Header("MUFFLER")]
    [SerializeField] private bool hasMuffler;
    [SerializeField] private AudioSource mufflerAudio;

    [SerializeField] private float mufflerStartSpeed;
    // [SerializeField] private float mufflerMinSpeed;
    [SerializeField] private float mufflerMaxSpeed;

    [SerializeField] private float mufflerMinPitch;
    [SerializeField] private float mufflerMaxPitch;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate() {
        currentSpeed = rb.velocity.magnitude * 3.6f; //convert to kph

        //ENGINE
        if(currentSpeed <= minSpeed) audioSource.pitch = minPitch;

        else if(currentSpeed > maxSpeed) audioSource.pitch = maxPitch;

        else {
            pitchRatio = currentSpeed / maxSpeed;
            audioSource.pitch = minPitch + (pitchRatio*maxPitch);
        }

        //MUFFLER
        if(hasMuffler) {
            if(currentSpeed >= mufflerStartSpeed) {
                // float mufflerPitchRatio = (currentSpeed / (mufflerMaxSpeed - mufflerStartSpeed) - 1) * 2f;
                float mufflerPitchRatio = (currentSpeed / (mufflerMaxSpeed - mufflerStartSpeed)) * 2f;
                mufflerAudio.pitch = mufflerPitchRatio + 1;
                mufflerAudio.volume = (mufflerPitchRatio - 0.75f)/2f;
                if(!mufflerAudio.isPlaying) mufflerAudio.Play();
            } else if(mufflerAudio.isPlaying) {
                mufflerAudio.Stop();
            }
        }
    }

    public void MufflerCheck() {
        // mufflerAudio.volume += 0.25f;
    }
}