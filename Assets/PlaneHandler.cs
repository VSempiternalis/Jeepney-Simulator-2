using UnityEngine;
using System.Collections.Generic;

public class PlaneHandler : MonoBehaviour {
    [SerializeField] private List<Transform> nodes;
    private int currentNodeInt;
    private Transform currentNode;
    [SerializeField] private int speed;
    [SerializeField] private int rotationSpeed;

    private void Start() {
        currentNodeInt = 0;
        currentNode = nodes[0];
    }

    private void Update() {
        //IF CLOSE TO NODE
        if(Vector3.Distance(transform.position, currentNode.position) < 1f) {
            if(currentNodeInt == nodes.Count - 1) {
                currentNodeInt = 0;
                currentNode = nodes[0];
            } else {
                currentNodeInt++;
                currentNode = nodes[currentNodeInt];
            }
        }

        //MOVE TO NODE
        else {
            //MOVE
            transform.position = Vector3.MoveTowards(transform.position, currentNode.position, speed * Time.deltaTime);

            //FACE
            // transform.LookAt(currentNode);

            //USE LEANTWEEN TO SMOOTHLY ROTATE TO FACE THE NODE
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(currentNode.position - transform.position), rotationSpeed * Time.deltaTime);
        }
    }
}
