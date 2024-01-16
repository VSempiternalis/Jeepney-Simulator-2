using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;

public class CarController : MonoBehaviour {
    [SerializeField] private GameObject headlights;

    [Space(10)]
    [Header("VARIABLES")]
    public bool isEngineOn;
    public float maxAcceleration = 30.0f;
    public float brakeAcceleration = 50.0f;

    public float turnSens = 1.0f;
    public float maxSteerAngle = 30.0f;

    public Vector3 centerOfMass;

    private float moveInput;
    private float steerInput;
    private bool brakeInput;

    private Rigidbody carRb;
    public float freeDrag;

    [Space(10)]
    [Header("STEERING")]
    [SerializeField] private Transform steeringWheel;
    [SerializeField] private Vector3 swStandardRot;
    [SerializeField] private float swRotSpeed;
    private int rotationTweenId = -1; // Store the ID of the current rotation Tween

    [Space(10)]
    [Header("POINTS")]
    [SerializeField] private Transform driverPos;
    [SerializeField] private Transform pointDriverExit;

    [Space(10)]
    [Header("GEAR SHIFTING")]
    [SerializeField] private float motorPower;
    private float RPM;
    [SerializeField] private float redLine;
    [SerializeField] private float idleRPM;
    [SerializeField] private TMP_Text rpmText;
    [SerializeField] private TMP_Text gearText;
    [SerializeField] private Transform rpmNeedle;
    [SerializeField] private float minNeedleRotation;
    [SerializeField] private float maxNeedleRotation;
    [SerializeField] private int gear;
    private float wheelRPM;
    [SerializeField] private AnimationCurve horsePowerToRPMCurve;

    [SerializeField] private float[] gearRatios;
    [SerializeField] private float differentialRatio;
    private float torque;
    private float clutch;
    private GearState gearState;
    // [SerializeField] private float increaseGearRPM;
    // [SerializeField] private float decreaseGearRPM;
    // [SerializeField] private float changeGearTime = 0.5f;

    [Space(10)]
    [Header("Keybinds")]
    private KeyCode Key_DriveForward;
    private KeyCode Key_DriveBackward;
    private KeyCode Key_SteerLeft;
    private KeyCode Key_SteerRight;
    private KeyCode Key_Headlights;
    private KeyCode Key_Horn;
    private KeyCode Key_Brake;
    private KeyCode Key_GearUp;
    private KeyCode Key_GearDown;
    private KeyCode Key_TowTruck;

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
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = centerOfMass;
        // swStandardRot = steeringWheel.transform.rotation.eulerAngles;
        swStandardRot.x = -34.624f;
        swStandardRot.y = 180;
        swStandardRot.z = 0;

        // KeybindsManager.current.onKeyChangeEvent += OnKeyChangeEvent;
        OnKeyChangeEvent();
    }

    private void Update() {
        AnimateWheels();
        AnimateDashboard();
    }

    private void FixedUpdate() {
        Move();
        Steer();
        Brake();
    }

    private void OnKeyChangeEvent() {
        //Set keys
        Key_DriveForward = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_DriveForward", "W"));;
        Key_DriveBackward = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_DriveBackward", "S"));;
        Key_SteerLeft = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_SteerLeft", "A"));;
        Key_SteerRight = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_SteerRight", "D"));;
        Key_Headlights = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_Headlights", "R"));;
        Key_Horn = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_Horn", "F"));;
        Key_Brake = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_Brake", "Space"));;
        Key_GearUp = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_GearUp", "LeftShift"));;
        Key_GearDown = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_GearDown", "LeftControl"));;
        Key_TowTruck = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_TowTruck", "T"));;
    }

    public void GetInput() {
        //movement
        if(Input.GetKey(Key_DriveForward)) {
            //if R or N, set to gear 1
            // if()
            moveInput = 1;
        } else if(Input.GetKey(Key_DriveBackward)) {
            //if forward, set to R
            // if()
            moveInput = 1;
        }
        // moveInput = Input.GetAxis("Vertical");

        //Braking
        brakeInput = Input.GetKey(Key_Brake);

        //Steering
        steerInput = Input.GetAxis("Horizontal");
        if(steerInput != 0) {
            Vector3 newRot = swStandardRot;
            newRot.z = steerInput * 90;

            if(rotationTweenId != -1) LeanTween.cancel(rotationTweenId);

            // Start the new rotation animation
            rotationTweenId = LeanTween.rotateLocal(steeringWheel.gameObject, newRot, swRotSpeed).id;
        } else {
            if(rotationTweenId != -1) LeanTween.cancel(rotationTweenId);

            rotationTweenId = LeanTween.rotateLocal(steeringWheel.gameObject, swStandardRot, swRotSpeed).id;
        }

        //Shifting

    }

    public void ToggleDriverSeat(Transform driver) {
        //exit
        if(driverPos.childCount > 0 && driverPos.GetChild(0) == driver) {
            //cameras

            driver.position = pointDriverExit.position;
            driver.rotation = driverPos.rotation;
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
        if(!isEngineOn) return;
        // torque = CalculateTorque();
        torque = 0;
        foreach(Wheel wheel in wheels) {
            if(wheel.axle == Axle.Rear) {
                wheelRPM = Mathf.Abs(wheel.wheelCollider.rpm) * gearRatios[gear] * differentialRatio;
                RPM = Mathf.Lerp(RPM, Mathf.Max(idleRPM-100, wheelRPM), Time.deltaTime * 3f);
                torque = (horsePowerToRPMCurve.Evaluate(RPM/redLine)*motorPower/RPM)*gearRatios[gear]*differentialRatio*5252f;//*clutch
                wheel.wheelCollider.motorTorque = moveInput * torque;
            }
            // if(wheel.axle == Axle.Rear) wheel.wheelCollider.motorTorque = moveInput * maxAcceleration;
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
        if(brakeInput) {
            foreach(Wheel wheel in wheels) {
                wheel.wheelCollider.brakeTorque = brakeAcceleration;
            }
        } else {
            foreach(Wheel wheel in wheels) {
                wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }

    // private float CalculateTorque() {
    //     float torque = 0;
    //     if(isEngineOn) {
    //         //cluch
    //         wheelRPM = Mathf.Abs(Axle.Rear.)
    //     }
    // }

    private void AnimateDashboard() {
        //needle rotation
        rpmNeedle.rotation = Quaternion.Euler(0, Mathf.Lerp(minNeedleRotation, maxNeedleRotation, RPM/redLine), 0);
        rpmText.text = RPM.ToString("0, 000");
        gearText.text = (gear+1).ToString();
        //speed
        //speedClamped
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

    // IEnumerator ChangeGear(int gearChange) {
    //     gearState = GearState.CheckingChange;
    //     if(gear + gearChange >= 0) {
    //         if(gearChange > 0) {
    //             yield return new WaitForSeconds(0.7f);
    //             if(RPM < increaseGearRPM || gear >= gearRatios.Length-1) {
    //                 gearState = GearState.Running;
    //                 yield break;
    //             }
    //         } else if(gearChange < 0) {
    //             yield return new WaitForSeconds(0.1f);

    //             if(RPM > decreaseGearRPM || gear <= 0) {
    //                 gearState = GearState.Running;
    //                 yield break;
    //             }
    //         }
    //         gearState = GearState.Changing;
    //         yield return new WaitForSeconds(changeGearTime);
    //     }
    //     gear += gearChange;
    //     gearState = GearState.Running;
    // }
}

public enum GearState {
    Neutral,
    Running,
    CheckingChange,
    Changing
}