using UnityEngine;

public class aiCarInput : MonoBehaviour {
    //COMPONENTS
    public aiCarController carCon;

    //VARIABLES
    private bool isBraking;
    [SerializeField] private float distToNodeThresh = 1f;

    //COLLISION AVOIDANCE
    public CollisionAvoidance CA_frontMed;
    [SerializeField] private float distToNode;
    //public CollisionAvoidance CA_frontClose;

    // [Space(10)]
    // [Header("NODES")]
    // [SerializeField] private NodeHandler nextNode;
    // [SerializeField] private NodeHandler currentNode;

    private void Start() {
        carCon = GetComponent<aiCarController>();
    }

    private void Update() {
        if(CA_frontMed == null) return;

        //MoveInput
        Vector2 moveInput = new Vector2(0, 0);
        //if(!isBraking) moveInput.y = 1;

        //COLLISION AVOIDANCE
        // if(CA_frontMed.state == "Empty") {
        //     moveInput.y = 1;
        // } else if(CA_frontMed.state == "Stay") {
        //     carCon.Brake();
        // } else if(CA_frontMed.state == "Exit") {
        //     carCon.StopBrake();
        //     CA_frontMed.state = "Empty";
        // }

        //COLLISION AVOIDANCE
        if(CA_frontMed.triggerCount == 0) {
            if(carCon.isBraking) carCon.StopBrake();
            moveInput.y = 1;
        } else if(CA_frontMed.triggerCount > 0) {
            carCon.Brake();
        }

        //CHECK IF NEAR NEXT NODE
        if(carCon.nextNode) {
            distToNode = (Vector3.Distance(transform.position, carCon.nextNode.transform.position));
            if(Vector3.Distance(transform.position, carCon.nextNode.transform.position) <= distToNodeThresh) {
                NextNode();
            }
        }

        //moveInput.x = 1;

        carCon.GetInput(moveInput);
    }

    public void NextNode() {
        carCon.currentNode = carCon.nextNode;
        carCon.nextNode = carCon.currentNode.GetRandomNode();

        if(carCon.nextNode == null && GetComponent<Despawner>()) GetComponent<Despawner>().Despawn();

        //Snap to current node
        //transform.position = carCon.currentNode.transform.position;
        //transform.LookAt(carCon.nextNode.transform, Vector3.up);
    }
}
