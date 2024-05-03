using UnityEngine;

[ExecuteInEditMode]
public class VehicleSpawn : MonoBehaviour {
    [SerializeField] private LayerMask triggerLayer;
    // [SerializeField] private LayerMask vehicleLayer;
    // [SerializeField] private LayerMask playerLayer;

    public NodeHandler node;
    public bool isSpawnable;
    public bool onlySpawnSmallVics;
    public int triggerCount;

    private void Start() {
        
    }

    private void Update() {
        // DrawLines();
    }

    private void DrawLines() {
        if(node != null) {
            Debug.DrawLine(transform.position, node.transform.position, new Color(0, 0, 1, 50f/255f));
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(triggerLayer == (triggerLayer | (1<<other.gameObject.layer))) triggerCount ++;//isSpawnable = false;
    }

    private void OnTriggerExit(Collider other) {
        if(triggerLayer == (triggerLayer | (1<<other.gameObject.layer))) triggerCount --;//isSpawnable = false;
    }
}
