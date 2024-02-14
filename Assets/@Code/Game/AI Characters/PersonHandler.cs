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
    // [SerializeField] private float rotationTime;
    [SerializeField] private float yUp;

    [Space(10)]
    [Header("LAYERS")]
    [SerializeField] private int dropAreaLayer;
    [SerializeField] private int spawnAreaLayer;

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
        else if(state == "Wandering") Wander();
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
                    if(payPoint.GetComponent<StorageHandler>().value > 0) StartPayTimer(); //reset pay if money still on paymat
                    else PayFare();
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

    #region LONG FUNCTIONS ==========================================================================================

    private void Wait() {
        if(Time.time >= waitTime) MakeWander();

        if(!player.isTakingPassengers || !isPassenger || !player.carCon.HasFreeSeats()) return;
        // else if(!player.isTakingPassengers && ) return;
        // else if(!DestinationsManager.current.destinations.Contains(to)) return;

        //[Call jeep when close]
        distToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if(distToPlayer <= callDist && player.isDriving && player.isTakingPassengers && player.carCon.HasFreeSeats()) {
            // popup.Say(sayHandler.GetSay("Call"), false);
            // transform.LookAt(player.transform, Vector3.up);
            FacePos(player.transform.position);
            //[HAIL]
            //GetComponent<Test_script>().Hail();
            carCon = player.carCon;

            print("Car mag: " + carCon.GetComponent<Rigidbody>().velocity.magnitude + " mag thresh: " + magnitudeThresh);
            if(carCon.GetComponent<Rigidbody>().velocity.magnitude <= magnitudeThresh) {
                print("Past magnitude thresh");
                MakeMoveToVehicle();
            }
        } else {
            //face player
            //hail anim
        }
    }

    private void Wander() {
        MoveToNextPos();

        if(posDestinations.Count == 0) MakeWait();
    }

    private void MoveToNextPos() {
        FacePos(posDestinations[0]);
        float originalYRotation = transform.rotation.eulerAngles.y;

        //if destination reached
        if(Vector3.Distance(transform.position, posDestinations[0]) < reachDist) {
            posDestinations.RemoveAt(0);
            transform.rotation = Quaternion.Euler(0f, originalYRotation, 0f);
            return;
        }

        Vector3 newPos = new Vector3(posDestinations[0].x, transform.position.y + yUp, posDestinations[0].z);
        transform.position = Vector3.MoveTowards(transform.position, newPos, moveSpeed * Time.fixedDeltaTime);
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
    }
    
    private void MoveToNextDest() {
        FacePos(nextNode.position);
        
        //if destination reached
        if(Vector3.Distance(transform.position, nextNode.position) < reachDist) {
            //Check if entrance
            if(nextNode.name.Contains("Entrance")) EnterVehicle();
            else if(nextNode.GetComponent<MoveNode>()) nextNode = nextNode.GetComponent<MoveNode>().nextNode;
            // else destinations.RemoveAt(0);
            else nextNode = null;
            return;
        }

        Vector3 newPos = new Vector3(nextNode.position.x, transform.position.y + yUp, nextNode.position.z);
        transform.position = Vector3.MoveTowards(transform.position, newPos, moveSpeed * Time.fixedDeltaTime);
    }

    #endregion

    #region SINGLE FRAME FUNCTIONS =================================================================================================

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
        SetState("Moving to vehicle");
    }

    // private void Face(Transform target) {
    //     // print("Facing: " + target.name);
    //     Vector3 newLook = new Vector3(target.position.x, transform.position.y, target.position.z);
    //     transform.LookAt(newLook, Vector3.up);
    // }

    private void FacePos(Vector3 targetPos) {
        // print("Facing: " + target.name);
        // Vector3 newLook = new Vector3(targetPos.x, transform.position.y, targetPos.z);
        // transform.LookAt(newLook, Vector3.up);

        // Quaternion targetRotation = Quaternion.LookRotation(newLook - transform.position, Vector3.up);
        // LeanTween.rotate(transform.gameObject, targetRotation.eulerAngles, rotationTime).setEase(LeanTweenType.easeOutSine);

        // Vector3 direction = (targetPos - transform.position).normalized;
        // Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    
        // Calculate direction to target ignoring y-axis
        Vector3 direction = (new Vector3(targetPos.x, transform.position.y, targetPos.z) - transform.position).normalized;

        // Calculate target rotation only around y-axis
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        // Preserve current x and z rotations
        float currentXRotation = transform.rotation.eulerAngles.x;
        float currentZRotation = transform.rotation.eulerAngles.z;

        // Create new rotation with preserved x and z rotations and only change in y rotation
        Quaternion newYRotation = Quaternion.Euler(currentXRotation, targetRotation.eulerAngles.y, currentZRotation);

        // Smoothly rotate only around y-axis
        transform.rotation = Quaternion.Slerp(transform.rotation, newYRotation, rotationSpeed * Time.fixedDeltaTime);
    }

    private void EnterVehicle() {
        print("ENTER VEHICLE");
        carCon.TakeSeat(transform);

        GetComponent<CapsuleCollider>().isTrigger = true;
        GetComponent<Rigidbody>().isKinematic = true;
        // popup.SayConstant(to);
        // popup.SayChange("(Unpaid)");
        destinations.Clear();

        StartPayTimer();
        SetState("Waiting to pay");
        
        AddDestination();
    }

    public void AddDestination() {
        DestinationsUIManager.current.AddDestination(to);
    }

    #endregion

    #region MONEY

    private void GetChange(int amount) {
        change -= amount;
        if(change <= 0) {
            // popup.SayChange("");
            state = "Waiting to arrive";
            changePoint.GetComponent<ChangeHandler>().RemoveChangee(this);
        } else {
            // popup.SayChange("P" + change); 
        }
    }

    private void StartPayTimer() {
        payTime = Mathf.RoundToInt(Time.time) + Random.Range(payTimeRange.x, payTimeRange.y);
    }

    private void PayFare() {
        //[+] voiceHandler.Say("Pay");

        // popup.Say(sayHandler.GetSay("Pay"), true);

        int paid = 0;

        for(int i = money.Count; i > 0; i--) {
            while(paid < fare) {
                int spawnRoll = Random.Range(0, 101);

                if(spawnRoll <= moneySpawnProbabilities[i-1]) {
                    GameObject newMoney = Instantiate(money[i-1]);
                    Vector3 setpos = new Vector3(transform.parent.position.x, transform.parent.position.y + 1f, transform.parent.position.z);
                    newMoney.transform.position = setpos;
                    print("Trying to add item");
                    payPoint.GetComponent<StorageHandler>().AddItem(newMoney);
                    print("new money");
                    newMoney.name = "Money - P" + newMoney.GetComponent<Value>().value;

                    paid += newMoney.GetComponent<Value>().value;
                } else break;
            }
        }

        change = paid - fare;
        if(change > 0) {
            // popup.SayChange("P" + change);
            state = "Waiting for change"; //waitingForChange = true;

            //Add changee to changehandler
            changePoint.GetComponent<ChangeHandler>().AddChangee(this);
        } else {
            // popup.SayChange("");
            state = "Waiting to arrive";
        }
    }

    #endregion

    #region STATE SETTERS ==================================================================================================================

    private void MakeWait() {
        waitTime = Time.time + Random.Range(waitTimeRange.x, waitTimeRange.y + 1);

        SetState("Waiting");
    }

    private void MakeWander() {
        print("wandering");
        if(currentSpot == null) return;

        float moveX = Random.Range(currentSpot.position.x - (currentSpot.localScale.x/2), currentSpot.position.x + (currentSpot.localScale.x/2));
        float moveZ = Random.Range(currentSpot.position.z - (currentSpot.localScale.z/2), currentSpot.position.z + (currentSpot.localScale.z/2));

        posDestinations.Add(new Vector3(moveX, currentSpot.position.y + yUp, moveZ));
        SetState("Wandering");
    }

    private void SetState(string newState) {
        print("setting state to: " + newState);
        state = newState;

        //ANIMATION
        int anim = 0;
        
        //Stand
        if(state == "Idle" || state == "Waiting") anim = Random.Range(0, 8);
        //Walk
        else if(state == "Walking" || state == "Moving to vehicle" || state == "Wandering") anim = Random.Range(14, 20);
        //Sit
        else if(state == "Waiting to pay") anim = Random.Range(20, 30);

        print("Anim: " + anim);
        ani.SetInteger("State", anim);
    }

    #endregion

    #region TRIGGERS ======================================================================================================
  
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == dropAreaLayer && other.name.Contains(to) && state == "Waiting to arrive") {
            // voiceHandler.Say("Stop");
        }
        else if(other.gameObject.layer == spawnAreaLayer) currentSpot = other.transform;
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.layer == dropAreaLayer && other.name.Contains(to)) Arrived(); //Drop;
        else if(other.gameObject.layer == 8 && other.name.Contains(to)) { //Area
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.layer == dropAreaLayer && other.name.Contains(to) && state == "Waiting to drop") state = "Waiting to arrive";
        else if(other.gameObject.layer == spawnAreaLayer) currentSpot = null;
    }

    #endregion
}