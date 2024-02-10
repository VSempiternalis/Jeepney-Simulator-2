using UnityEngine;
using System.Collections.Generic;

public class PersonHandler : MonoBehaviour {
    [Header("COMPONENTS")]
    // [SerializeField] private PopupHandler popup;
    // private SayHandler sayHandler;
    private Transform currentSpot;
    [SerializeField] private Transform voices;
    [SerializeField] private Transform voicesMale;
    [SerializeField] private Transform voicesFemale;
    [SerializeField] private Transform voicesChild;
    // private VoiceHandler voiceHandler;

    [Space(10)]
    [Header("STATS")]
    public string state;
    private float dropStartTime;
    private float reachDist = 0.35f;
    public string personType;

    [Space(10)]
    [Header("VARIABLES")]
    [SerializeField] private Vector2 moveSpeedRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector2 waitTimeRange;
    private float waitTime;
    [SerializeField] private float callDist;
    [SerializeField] private float magnitudeThresh;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Vector2 payTimeRange;
    private float payTime;
    [SerializeField] private bool isPassenger;

    [Space(10)]
    [Header("LAYERS")]
    [SerializeField] private int dropAreaLayer;

    [Space(10)]
    [Header("DESTINATION")]
    private Transform nextNode;
    private List<Transform> destinations = new List<Transform>();
    private List<Vector3> posDestinations = new List<Vector3>();
    public string from;
    public string to;
    private bool isInTo;

    [Space(10)]
    [Header("PAYMENT")]
    public Transform payPoint;
    public Transform changePoint;
    [SerializeField] private int fare = 12;
    [SerializeField] private GameObject coin1PF;
    [SerializeField] private GameObject coin5PF;
    [SerializeField] private GameObject coin10PF;
    [SerializeField] private GameObject cash20PF;
    [SerializeField] private GameObject cash50PF;
    private List<GameObject> money = new List<GameObject>();
    [SerializeField] private List<int> moneySpawnProbabilities = new List<int>();
    private bool waitingForChange;
    public int change;

    [Space(10)]
    [Header("ANIMATION")]
    private Animator ani;

    private float nextSecUpdate;
    private PlayerDriveInput player;
    [SerializeField] private float distToPlayer;
    public CarController carCon;

    private void Start() {
        //[+] sayHandler = GetComponent<SayHandler>();
        player = GameObject.Find("PLAYER").GetComponent<PlayerDriveInput>();
        ani = GetComponent<Animator>();

        //Populate money
        money.Add(coin1PF);
        money.Add(coin5PF);
        money.Add(coin10PF);
        money.Add(cash20PF);
        money.Add(cash50PF);

        MakeWait();

        //SET MOVESPEED
        moveSpeed = Random.Range(moveSpeedRange.x, moveSpeedRange.y);

        //[+] GET VOICES
        // if(personType == "Male") voices = voicesMale;
        // else if(personType == "Female") voices = voicesFemale;
        // else if(personType == "Child") {
        //     voices = voicesChild;
        //     //Make children immortal beings
        //     Destroy(GetComponent<Ragdoll>());
        // }
        // else print("VOICE ERROR GODDAMMIT");
        // voiceHandler = GetComponent<VoiceHandler>();
        // int voiceIndex = Random.Range(0, voices.childCount);
        // VoiceType voiceType = voices.GetChild(voiceIndex).GetComponent<VoiceType>();
        // voiceHandler.SetAudioClips(voiceType.payAudios, voiceType.stopAudios, voiceType.deathAudios);
        // patience = maxPatience;

        nextSecUpdate = Time.time + 1;
    }

    private void FixedUpdate() {
        
        // transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed);
        // if(nextSecUpdate > 30) SetState("Walking");
        // print("nextsecupdate: " + nextSecUpdate);
        if(state == "Idle") return;

        // else if(state == "Paying") PayFare();
        // else if(state == "Wandering") Wander();
        else if(state == "Moving to vehicle") MoveToVehicle();
        // else if(state == "Waiting to drop") WaitToDrop();
        // else if(state == "Dropping") Dropping();

        if(Time.time >= (nextSecUpdate)) {
            nextSecUpdate ++;
        //     PatienceCheck();
            
            if(state == "Waiting") {
                Wait();
            } 
            else if(state == "Waiting to pay") {
                if(Time.time >= payTime) {
                    // if(payPoint.GetComponent<StorageHandler>().value > 0) StartPayTimer();
                    // else PayFare();
                }
        //     } else if(state == "Waiting for change") {
        //         // if(changePoint.GetComponent<StorageHandler>().value > 0 && changePoint.GetComponent<StorageHandler>().value <= change) {
        //         if(changePoint.GetComponent<StorageHandler>().value > 0 && changePoint.GetComponent<ChangeHandler>().currentChangee == this) {
        //             // GetChange(changePoint.GetComponent<StorageHandler>().value);
        //             // changePoint.GetComponent<StorageHandler>().Clear();
        //             List<GameObject> removeList = new List<GameObject>();
        //             foreach(GameObject item in changePoint.GetComponent<StorageHandler>().items) {
        //                 if(item.GetComponent<Value>() && item.GetComponent<Value>().value <= change) {
        //                     GetChange(item.GetComponent<Value>().value);
        //                     removeList.Add(item);
        //                 }
        //             }

        //             //[] Remove items taken
        //             foreach(GameObject item in removeList) {
        //                 changePoint.GetComponent<StorageHandler>().RemoveItem(item);
        //                 Destroy(item);
        //             }
        //             changePoint.GetComponent<ChangeHandler>().UpdateText();
        //         }
            }
        }
    }

    // LONG FUNCTIONS ==========================================================================================

    private void Wait() {
        // if(Time.time >= waitTime) MakeWander();

        // if(!player.isTakingPassengers || !isPassenger) return;
        // else if(!player.isTakingPassengers && !player.carCon.HasFreeSeats()) return;
        // else if(!player.destinations.Contains(to)) return;

        //[Call jeep when close]
        distToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if(distToPlayer <= callDist && player.isDriving && player.isTakingPassengers && player.carCon.HasFreeSeats()) {
            // popup.Say(sayHandler.GetSay("Call"), false);
            transform.LookAt(player.transform, Vector3.up);
            //[HAIL]
            //GetComponent<Test_script>().Hail();
            carCon = player.carCon;

            print("Car mag: " + carCon.GetComponent<Rigidbody>().velocity.magnitude + " mag thresh: " + magnitudeThresh);
            if(carCon.GetComponent<Rigidbody>().velocity.magnitude <= magnitudeThresh) {
                print("Past magnitude thresh");
                MakeMoveToVehicle();
            }
        }
    }

    private void MoveToVehicle() {
        if(destinations.Count > 3) destinations.RemoveRange(3, destinations.Count-4);

        if(!carCon.HasFreeSeats()) {
            SetState("Waiting");
            return;
        } else if(Vector3.Distance(transform.position, carCon.transform.position) > callDist) {
            SetState("Waiting");
            return;
        }

        if(nextNode) MoveToNextDest();

        // if(destinations.Count == 0) {
        //     SetState("Waiting");
        //     return;
        //     // EnterVehicle();
        // }

        // check if near entrance
        // foreach(Transform point in carCon.points) {
        //     if(point.name.Contains("Entrance") && Vector3.Distance(transform.position, point.position) < reachDist) {
        //         EnterVehicle();
        //     }
        // }
    }
    
    private void MoveToNextDest() {
        print("MOVE TO NEXT DEST: " + nextNode + " " + Time.time);
        // Face(destinations[0]);
        Face(nextNode);
        
        //if destination reached
        if(Vector3.Distance(transform.position, nextNode.position) < reachDist) {
            //Check if entrance
            if(nextNode.name.Contains("Entrance")) EnterVehicle();
            else if(nextNode.GetComponent<MoveNode>()) nextNode = nextNode.GetComponent<MoveNode>().nextNode;
            // else destinations.RemoveAt(0);
            else nextNode = null;
            return;
        }
        // if(Vector3.Distance(transform.position, destinations[0].position) < reachDist) {
        //     if(destinations[0].GetComponent<MoveNode>()) destinations[0] = destinations[0].GetComponent<MoveNode>().nextNode;
        //     else destinations.RemoveAt(0);
        //     return;
        // }

        Vector3 newPos = new Vector3(nextNode.position.x, transform.position.y, nextNode.position.z);
        transform.position = Vector3.MoveTowards(transform.position, newPos, moveSpeed * Time.fixedDeltaTime);
        // Vector3 newPos = new Vector3(destinations[0].position.x, transform.position.y, destinations[0].position.z);
        // transform.position = Vector3.MoveTowards(transform.position, newPos, moveSpeed);
    }

    // SINGLE FRAME FUNCTIONS =================================================================================================

    private void Arrived() {
        if(state == "Dropping" || state == "Waiting for change") return;
        else if(state == "Waiting to pay") {
            //TESTING
            // PayFare();
            return;
        } else if(state == "Waiting to arrive") state = "Waiting to drop";
    }

    private void MakeMoveToVehicle() {
        //Move to closest carpoint
        Transform closest = null;
        float dist = 100;
        foreach(Transform point in carCon.points) {
            if(Vector3.Distance(transform.position, point.position) < dist) {
                dist = Vector3.Distance(transform.position, point.position);
                closest = point;
            }
        }

        if(closest != null) nextNode = closest;
        //! if(closest != null) destinations.Add(closest);

        //Disable collider when moving to vehicle
        // GetComponent<CapsuleCollider>().isTrigger = false;
        // GetComponent<Rigidbody>().isKinematic = false;
        // state = "Moving to vehicle";
        SetState("Moving to vehicle");
    }

    private void Face(Transform target) {
        print("Facing: " + target.name);
        Vector3 newLook = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(newLook, Vector3.up);
    }

    private void EnterVehicle() {
        print("ENTER VEHICLE");
        carCon.TakeSeat(transform);

        GetComponent<CapsuleCollider>().isTrigger = true;
        GetComponent<Rigidbody>().isKinematic = true;
        // popup.SayConstant(to);
        // popup.SayChange("(Unpaid)");
        destinations.Clear();

        // StartPayTimer();
        SetState("Waiting to pay");
        
        // AddDestination();
    }

    // STATE SETTERS ==================================================================================================================

    private void MakeWait() {
        waitTime = Time.time + Random.Range(waitTimeRange.x, waitTimeRange.y + 1);

        SetState("Waiting");
    }

    private void MakeWander() {
        if(currentSpot == null) return;

        float moveX = Random.Range(currentSpot.position.x - (currentSpot.localScale.x/2), currentSpot.position.x + (currentSpot.localScale.x/2));
        float moveZ = Random.Range(currentSpot.position.z - (currentSpot.localScale.z/2), currentSpot.position.z + (currentSpot.localScale.z/2));

        posDestinations.Add(new Vector3(moveX, currentSpot.position.y-0.5f, moveZ));
        SetState("Wandering");
    }

    private void SetState(string newState) {
        print("setting state to: " + newState);
        state = newState;

        //ANIMATION
        //Stand
        if(state == "Idle" || state == "Waiting") ani.SetInteger("State", 0);
        //Walk
        else if(state == "Walking" || state == "Moving to vehicle" || state == "Wandering") ani.SetInteger("State", 1);
        //Sit
        else if(state == "Waiting to pay") ani.SetInteger("State", 2);
    }

    // TRIGGERS ======================================================================================================

    
    
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == dropAreaLayer && other.name.Contains(to) && state == "Waiting to arrive") {
            // voiceHandler.Say("Stop");
        }
        else if(other.gameObject.layer == 10) currentSpot = other.transform;
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.layer == dropAreaLayer && other.name.Contains(to)) Arrived(); //Drop;
        else if(other.gameObject.layer == 8 && other.name.Contains(to)) { //Area
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.layer == dropAreaLayer && other.name.Contains(to) && state == "Waiting to drop") state = "Waiting to arrive";
        else if(other.gameObject.layer == 10) currentSpot = null;
    }
}