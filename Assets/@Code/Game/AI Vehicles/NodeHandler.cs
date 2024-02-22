using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class NodeHandler : MonoBehaviour {
    [SerializeField] private List<NodeHandler> connections;
    public float speedMod;
    [SerializeField] private int vehicleLayer;

    private void Start() {
        //Clear empty connections
        List<NodeHandler> clearList = new List<NodeHandler>();
        foreach(NodeHandler node in connections) {
            if(node == null) clearList.Add(node);
        }
        foreach(NodeHandler clear in clearList) {
            connections.Remove(clear);
        }
    }

    private void Update() {
        DrawLines();
    }

    private void DrawLines() {
        if(connections.Count > 0) {
            foreach(NodeHandler node in connections) {
                Debug.DrawLine(transform.position, node.transform.position, Color.gray);
            }
        }
    }

    public NodeHandler GetRandomNode() {
        if(connections.Count == 0) return null;

        int randomIndex = Random.Range(0, connections.Count);
        return connections[randomIndex];
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == vehicleLayer) {
            if(other.gameObject.GetComponent<aiCarInput>()) CallNextNode(other.gameObject.GetComponent<aiCarInput>()); 
            else if(other.transform.parent.GetComponent<aiCarInput>()) CallNextNode(other.transform.parent.GetComponent<aiCarInput>());
        }
    }

    private void CallNextNode(aiCarInput ai) {
        if(speedMod == 0) ai.carCon.maxMotorTorque = ai.carCon.gearFactor;
        else ai.carCon.maxMotorTorque = ai.carCon.maxMotorTorque*5;//ai.carCon.maxMotorTorque *= ai.carCon.currentNode.speedMod;

        // if(ai == null || ai.carCon == null || ai.carCon.nextNode == null) return;
        // if(ai.carCon.nextNode != this) return;

        // ai.NextNode();
    }
}
