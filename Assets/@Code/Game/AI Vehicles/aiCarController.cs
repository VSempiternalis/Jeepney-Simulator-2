using UnityEngine;
using System.Collections.Generic;
using TMPro;


public class aiCarController : MonoBehaviour {
    [Header("VARIABLES")]
    [SerializeField] private List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    [SerializeField] private float maxSteeringAngle;
    [SerializeField] private float brakeDrag;
    [SerializeField] private float freeDrag;
    public bool isActive = true;
    public bool isBraking;

    private Vector2 moveInput;
    public int gear = 1;
    public float gearFactor;

    [Space(10)]
    [Header("COMPONENTS")]
    [SerializeField] private List<WheelCollider> wheelColliders;
    [SerializeField] private List<Transform> visualWheels;
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioCrash;

    [Space(10)]
    [Header("NODES")]
    public NodeHandler nextNode;
    public NodeHandler currentNode;
    private bool turning;
    [SerializeField] private float turnSpeed;

    //COLLISION
    private float velocityThresh = 3f;

    private void Start() {
        maxMotorTorque = gearFactor;
        GetComponent<Rigidbody>().drag = freeDrag;
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate() {
        if(nextNode == null) {
            Destroy(GetComponent<aiCarInput>());
            Destroy(GetComponent<aiCarController>());
        }
        if(!isActive) return;

        //IF ENGINE ON
        float motor = 0;
        float steering = maxSteeringAngle * -moveInput.x;

        motor = maxMotorTorque * moveInput.y;

        foreach(AxleInfo axleInfo in axleInfos) {
            if(axleInfo.steering) {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if(axleInfo.motor) {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }

            //TURN STEER WHEELS
            // if(!axleInfo.steering) return;
            // axleInfo.leftWheel.transform.GetChild(0).Rotate(new Vector3(axleInfo.leftWheel.steerAngle, 0, 0));
            // axleInfo.rightWheel.transform.GetChild(0).Rotate(new Vector3(0, 0, 0));
        }

        //MOVE BY NODE
        if(moveInput.y > 0) TurnToDest();
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, nextNode.transform.rotation)

        //UpdateWheels();
    }

    private void TurnToDest() {
        if(nextNode == null) return;
        //transform.LookAt(nextNode.transform, Vector3.up);
        Vector3 thisPos = transform.position;
        Vector3 direction = nextNode.transform.position - thisPos;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.up);

        //float dotProduct = Vector3.Dot(transform.TransformDirection(Vector2.up), direction);

        // float snapThresh = 0.1f; //Snap threshold
        // if(((dotProduct < snapThresh && dotProduct > 0 && dotProduct < 1) || (dotProduct > -snapThresh && dotProduct < 0 && dotProduct > -1)) && turning) {
        //     transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //     moveStartPos = transform.position;

        //     //keep canvas readable
        //     if(canvas != null) canvas.transform.rotation = Quaternion.identity;

        //     NextState();
        // }
        transform.rotation = Quaternion.Slerp(transform.rotation, q, turnSpeed);
        turning = true;
    }

    private void UpdateWheels() {
        for(int i = 0; i < wheelColliders.Count; i ++) {
            Vector3 pos;
            Quaternion rot;
            wheelColliders[i].GetWorldPose(out pos, out rot);
            visualWheels[i].position = pos;
            visualWheels[i].rotation = rot;
        }
    }

    public void Brake() {
        isBraking = true;
        moveInput = new Vector2(0, 0);

        GetComponent<Rigidbody>().drag = brakeDrag;
    }

    public void StopBrake() {
        isBraking = false;
        GetComponent<Rigidbody>().drag = freeDrag;
    }

    public void SwitchGear(int add) {
        if(add > 0) gear += add;
        else if(add < 0 && gear >= 0) gear -= add;
        
        if(gear == -1) {
            maxMotorTorque = gear*gearFactor*-1;
        } else {
            maxMotorTorque = gear*gearFactor;
        }
    }

    public void GetInput(Vector2 newMoveInput) {
        if(gear < 0) newMoveInput.y *= -1;
        moveInput = newMoveInput;
    }

    [System.Serializable]
    public class AxleInfo {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;
        public bool motor;
        public bool steering;
    }

    private void OnCollisionEnter(Collision other) {
        int layer = other.gameObject.layer;
        //vehicle, person, playervic
        if(layer == 6 || layer == 13 || layer == 21) {
            float relativeVelocity = other.relativeVelocity.magnitude;
            
            if (relativeVelocity > velocityThresh) 
                audioSource.PlayOneShot(audioCrash);
        }
    }
}
