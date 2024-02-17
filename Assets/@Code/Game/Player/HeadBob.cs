using UnityEngine;

public class HeadBob : MonoBehaviour {
    [SerializeField] private Rigidbody rb;
    private Vector3 startPos;

    // [Range(0.001f, 0.01f)]
    [SerializeField] private float amount;

    // [Range(1f, 30f)]
    [SerializeField] private float freq;

    // [Range(10f, 100f)]
    [SerializeField] private float smooth;

    private void Start() {
        startPos = transform.localPosition;
    }

    private void Update() {
        // CheckForTrigger();
        smooth = rb.velocity.magnitude;
        DoHeadBob();
        // if(rb.velocity.magnitude > 0.1f) DoHeadBob();
        // else StopHeadBob();
    }

    private Vector3 DoHeadBob() {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * freq) * amount * 1.4f, smooth * Time.deltaTime);
        pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * freq/2f) * amount * 1.6f, smooth * Time.deltaTime);
        transform.localPosition += pos;

        return pos;
    }

    private void StopHeadBob() {
        if(transform.localPosition == startPos) return;
        transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, 1 * Time.deltaTime);
    }

    // private bool CheckForTrigger() {
    //     float in
    // }
}
