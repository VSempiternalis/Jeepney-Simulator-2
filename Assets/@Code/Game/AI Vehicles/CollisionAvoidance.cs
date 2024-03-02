using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionAvoidance : MonoBehaviour {
    public int triggerCount;
    private int emptyCheckTime = 5;
    [SerializeField] private List<GameObject> triggers = new List<GameObject>();
    // [SerializeField] private int checkCount;
    [SerializeField] private LayerMask layerMask;

    private void Start() {
        triggerCount = 0;
        InvokeRepeating("EmptyCheck", 0f, emptyCheckTime);
    }

    public void EmptyCheck() {
        // checkCount ++;
        triggerCount = 0;
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other) {
        if(((1 << other.gameObject.layer) & layerMask) != 0) {
            triggerCount ++;
        }
    }

    private void OnTriggerExit(Collider other) {
        if((((1 << other.gameObject.layer) & layerMask) != 0) && triggerCount > 0) {
            triggerCount --;
        }
    }
}