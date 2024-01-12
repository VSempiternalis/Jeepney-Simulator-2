using System;
using UnityEngine;
using System.Collections.Generic;

public class CarController : MonoBehaviour {
    [SerializeField] private Transform driverPos;

    [Space(10)]
    [Header("VARIABLES")]
    public float maxAcceleration = 30.0f;
    public float brakeAcceleration = 50.0f;

    public float turnSens = 1.0f;
    public float maxSteerAngle = 30.0f;

    public Vector3 centerOfMass;

    private float moveInput;
    private float steerInput;

    private Rigidbody carRb;
    public float freeDrag;

    [Space(10)]
    [Header("STEERING")]
    [SerializeField] private Transform steeringWheel;
    [SerializeField] private Vector3 swStandardRot;
    [SerializeField] private float swRotSpeed;
    private int rotationTweenId = -1; // Store the ID of the current rotation Tween

    [SerializeField] private GameObject headlights;

    [Space(10)]
    [Header("POINTS")]
    [SerializeField] private Transform pointDriverExit;

    public enum Axle {
        Front,
        Rear
    }

    [Serializable] public struct Wheel {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axle axle;
    }

    [Space(10)]
    [Header("WHEEL AND AXLES")]

    public List<Wheel> wheels;

    private void Start() {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = centerOfMass;
        // swStandardRot = steeringWheel.transform.rotation.eulerAngles;
        swStandardRot.x = -34.624f;
        swStandardRot.y = 180;
        swStandardRot.z = 0;
    }

    private void Update() {
        AnimateWheels();
    }

    private void FixedUpdate() {
        Move();
        Steer();
        Brake();
    }

    public void GetInput() {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");

        //Steering
        if(steerInput != 0) {
            Vector3 newRot = swStandardRot;
            newRot.z = steerInput * 90;

            if (rotationTweenId != -1) LeanTween.cancel(rotationTweenId);

            // Start the new rotation animation
            rotationTweenId = LeanTween.rotateLocal(steeringWheel.gameObject, newRot, swRotSpeed).id;
        } else {
            if (rotationTweenId != -1) LeanTween.cancel(rotationTweenId);

            rotationTweenId = LeanTween.rotateLocal(steeringWheel.gameObject, swStandardRot, swRotSpeed).id;
        }
    }

    public void ToggleDriverSeat(Transform driver) {
        //exit
        if(driverPos.childCount > 0 && driverPos.GetChild(0) == driver) {
            //cameras

            driver.position = pointDriverExit.position;
            // driver.rotation = driverPos.rotation;
            // driver.SetParent(GameObject.Find("WORLD").transform);
            driver.SetParent(null);

            driver.GetComponent<PlayerDriveInput>().SetIsDriving(false);
            driver.GetComponent<Rigidbody>().isKinematic = false;
            driver.GetComponent<CapsuleCollider>().isTrigger = false;

            //Reset rb drag (KEEP THIS IN. LITERALLY FIXES GEAR JAM)
            // GetComponent<Rigidbody>().drag = freeDrag;
            // GetComponent<Rigidbody>().drag = brakeDrag;
            //Reset move input to (hopefully) fix car moving on its own on exit
            moveInput = 0;
            steerInput = 0; 

            //destinations ui

            //gear

        } else { //enter
            //cameras

            driver.SetParent(driverPos);
            driver.localPosition = Vector3.zero;
            driver.rotation = driverPos.rotation;

            driver.GetComponent<PlayerDriveInput>().SetIsDriving(true);
            driver.GetComponent<PlayerDriveInput>().carCon = this;
            driver.GetComponent<Rigidbody>().isKinematic = true;
            driver.GetComponent<CapsuleCollider>().isTrigger = true;
        
            // GetComponent<Rigidbody>().drag = freeDrag;

            //Update destinations UI

            //Update gear
        }
    }

    private void Move() {
        foreach(Wheel wheel in wheels) {
            if(wheel.axle == Axle.Rear) wheel.wheelCollider.motorTorque = moveInput * maxAcceleration;
        }
    }

    private void Steer() {
        foreach(Wheel wheel in wheels) {
            if(wheel.axle == Axle.Front) {
                float steerAngle = steerInput * turnSens * maxSteerAngle;
                // wheel.wheelCollider.steerAngle = steerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 1.0f);
            }
        }
    }

    private void Brake() {
        if(Input.GetKey(KeyCode.Space)) {
            foreach(Wheel wheel in wheels) {
                wheel.wheelCollider.brakeTorque = brakeAcceleration;
            }
        } else {
            foreach(Wheel wheel in wheels) {
                wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }

    private void AnimateWheels() {
        foreach(Wheel wheel in wheels) {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }
}
