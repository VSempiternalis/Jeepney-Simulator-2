using UnityEngine;

public class Follower : MonoBehaviour {
    [SerializeField] private Transform toFollow;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float frequency;
    [SerializeField] private bool isFollowingRotation;

    private void Start() {
        InvokeRepeating("UpdatePosition", 0f, frequency);
    }

    private void Update() {
        
    }

    private void UpdatePosition() {
        transform.position = toFollow.position + offset;

        if(isFollowingRotation) {
            // Quaternion rot = transform.rotation;
            // rot.z = toFollow.rotation.y;
            // transform.rotation = rot;
            float y = toFollow.rotation.eulerAngles.y;
            Quaternion rot = Quaternion.Euler(0, y, 0);
            transform.rotation = rot;
        }
    }
}
