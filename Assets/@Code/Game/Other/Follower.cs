using UnityEngine;

public class Follower : MonoBehaviour {
    [SerializeField] private Transform toFollow;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float frequency;

    private void Start() {
        InvokeRepeating("UpdatePosition", 0f, frequency);
    }

    private void Update() {
        
    }

    private void UpdatePosition() {
        transform.position = toFollow.position + offset;
    }
}
