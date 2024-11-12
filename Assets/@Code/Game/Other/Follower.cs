using UnityEngine;

public class Follower : MonoBehaviour {
    [SerializeField] private Transform toFollow;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float frequency;
    [SerializeField] private bool isFollowingRotation;

    private GameObject image;

    private void Start() {
        if(transform.childCount > 0) image = transform.GetChild(0).gameObject;

        if(toFollow == null) return;

        InvokeRepeating("UpdatePosition", 0f, frequency);
    }

    private void Update() {
        if(image == null || toFollow == null) return;
        
        if(!toFollow.gameObject.activeSelf && image.activeSelf) image.SetActive(false);
        else if(toFollow.gameObject.activeSelf && !image.activeSelf) image.SetActive(true);
    }

    private void UpdatePosition() {
        transform.position = toFollow.position + offset;

        if(isFollowingRotation) {
            float y = toFollow.rotation.eulerAngles.y;
            Quaternion rot = Quaternion.Euler(0, y, 0);
            transform.rotation = rot;
        }
    }
}
