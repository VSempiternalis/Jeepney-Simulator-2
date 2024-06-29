using UnityEngine;
using System.Collections.Generic;

public class MapCamera : MonoBehaviour {
    [SerializeField] private List<Transform> rotatables;
    private bool isFollowRotation;

    private Quaternion initialRotation;

    [SerializeField] private Transform toFollow;

    private void Start() {
        initialRotation = transform.rotation;
    }

    private void Update() {
        if(isFollowRotation) {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, toFollow.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

            foreach(Transform rotatable in rotatables) {
                rotatable.rotation = Quaternion.Euler(rotatable.rotation.eulerAngles.x, toFollow.rotation.eulerAngles.y, rotatable.rotation.eulerAngles.z);
            }
        }
    }

    public void ToggleRotation() {
        isFollowRotation = !isFollowRotation;

        if(!isFollowRotation) {
            transform.rotation = initialRotation;

            foreach(Transform rotatable in rotatables) {
                // rotatable.rotation = Quaternion.identity;
                rotatable.rotation = Quaternion.Euler(90, 0, 0);
            }
        }
    }
}
