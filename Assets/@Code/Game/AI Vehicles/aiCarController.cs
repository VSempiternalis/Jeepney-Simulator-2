using UnityEngine;
using System.Collections.Generic;
using System;


public class aiCarController : MonoBehaviour {
    [Header("VARIABLES")]
    // [SerializeField] private List<AxleInfo> axleInfos;
    [SerializeField] private Vector2 maxMotorTorqueRange;
    public float maxMotorTorque;
    [SerializeField] private float maxSteeringAngle;
    [SerializeField] private float brakeDrag;
    [SerializeField] private float freeDrag;
    private bool isActive = true;
    public bool isBraking;

    private Vector2 moveInput;
    public int gear = 1;
    public float gearFactor;

    [Space(10)]
    [Header("COMPONENTS")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioCrash;
    [SerializeField] private CollisionAvoidance ca;
    [SerializeField] private ParticleSystem smokeParticles;

    [Space(10)]
    [Header("NODES")]
    public NodeHandler nextNode;
    public NodeHandler currentNode;
    private bool turning;
    [SerializeField] private float turnSpeed;

    //COLLISION
    private float velocityThresh = 3f;

    [Space(10)]
    [Header("WHEEL AND AXLES")]
    public List<Wheel> wheels;

    [Serializable] public struct Wheel {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axle axle;
    }

    public enum Axle {
        Front,
        Rear
    }

    private void Start() {
        maxMotorTorque = UnityEngine.Random.Range(maxMotorTorqueRange.x, maxMotorTorqueRange.y + 1);
        // maxMotorTorque = gearFactor;
        GetComponent<Rigidbody>().drag = freeDrag;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        AnimateWheels();
    }

    private void FixedUpdate() {
        if(nextNode == null) {
            return;
            // Destroy(GetComponent<aiCarInput>());
            // Destroy(GetComponent<aiCarController>());
        }
        if(!isActive) return;

        //IF ENGINE ON
        float motor = 0;
        float steering = maxSteeringAngle * -moveInput.x;

        motor = maxMotorTorque * moveInput.y;

        foreach(Wheel wheel in wheels) {
            wheel.wheelCollider.motorTorque = motor;
        }

        //MOVE BY NODE
        if(moveInput.y > 0) TurnToDest();
    }

    public void Reset() {
        ca.EmptyCheck();
    }

    public void SetActive(bool newIsActive) {
        isActive = newIsActive;
        if(isActive) smokeParticles.Play();
        else smokeParticles.Stop();
    }

    private void AnimateWheels() {
        foreach(Wheel wheel in wheels) {
            if(wheel.wheelCollider && wheel.wheelModel) {
                Quaternion rot;
                Vector3 pos;

                wheel.wheelCollider.GetWorldPose(out pos, out rot);
                wheel.wheelModel.transform.position = pos;
                wheel.wheelModel.transform.rotation = rot;
            }
        }
    }

    private void TurnToDest() {
        if(nextNode == null) return;
        Vector3 thisPos = transform.position;
        Vector3 direction = nextNode.transform.position - thisPos;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.up);
        
        transform.rotation = Quaternion.Slerp(transform.rotation, q, turnSpeed);
        turning = true;
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

    // [System.Serializable]
    // public class AxleInfo {
    //     public WheelCollider leftWheel;
    //     public WheelCollider rightWheel;
    //     public bool motor;
    //     public bool steering;
    // }

    private void OnCollisionEnter(Collision other) {
        int layer = other.gameObject.layer;
        //vehicle, person, playervic
        if(layer == 6 || layer == 13 || layer == 21) {
            float relativeVelocity = other.relativeVelocity.magnitude;
            
            if (relativeVelocity > velocityThresh) 
                if(audioSource) audioSource.PlayOneShot(audioCrash);
        }
    }
}
