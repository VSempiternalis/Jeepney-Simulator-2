using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;

public class CarController : MonoBehaviour {
    public enum Axle {
        Front,
        Rear
    }

    [Serializable] public struct Wheel {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axle axle;
    }

    public float maxAcceleration = 30.0f;
    public float brakeAcceleration = 50.0f;

    public float turnSens = 1.0f;
    public float maxSteerAngle = 30.0f;

    public Vector3 centerOfMass;

    public List<Wheel> wheels;

    private float moveInput;
    private float steerInput;

    private Rigidbody carRb;

    private void Start() {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = centerOfMass;
    }

    private void Update() {
        GetInput();
        AnimateWheels();
    }

    private void FixedUpdate() {
        Move();
        Steer();
        Brake();
    }

    private void GetInput() {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
    }

    private void Move() {
        foreach(Wheel wheel in wheels) {
            wheel.wheelCollider.motorTorque = moveInput * maxAcceleration;
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
