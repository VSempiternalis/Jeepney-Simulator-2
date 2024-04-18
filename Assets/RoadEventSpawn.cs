using UnityEngine;

public class RoadEventSpawn : MonoBehaviour {
    [SerializeField] private LayerMask triggerLayer;

    // public int positionInRoad; //0 - left, 1 - middle, 2 - right
    public int laneSize; //1-3
    public int triggerCount;

    private void Start() {
        
    }

    private void Update() {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(triggerLayer == (triggerLayer | (1<<other.gameObject.layer))) triggerCount ++;//isSpawnable = false;
    }

    private void OnTriggerExit(Collider other) {
        if(triggerLayer == (triggerLayer | (1<<other.gameObject.layer))) triggerCount --;//isSpawnable = false;
    }
}
