using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Runtime.InteropServices.WindowsRuntime;

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
    [Header("PASSENGERS")]
    public int passengerCount;
    public List<Transform> seatSpots;
    private List<int> seatsTaken = new List<int>();
    // [SerializeField] private List<Transform> leftSeats; //[!] left (perspective of passenger entering from rear)

    [Space(10)]
    [Header("POINTS")]
    [SerializeField] private Transform driverPos;
    [SerializeField] private Transform pointDriverExit;
    public List<Transform> points;
    [SerializeField] private Transform payPoint;
    [SerializeField] private Transform changePoint;

    [Space(10)]
    [Header("STEERING")]
    [SerializeField] private Transform steeringWheel;
    [SerializeField] private Vector3 swStandardRot;
    [SerializeField] private float swRotSpeed;
    private int rotationTweenId = -1; // Store the ID of the current rotation Tween

    [Space(10)]
    [Header("GEAR SHIFTING")]
    [SerializeField] private float motorPower;
    private float RPM;
    [SerializeField] private float redLine;
    [SerializeField] private float idleRPM;
    [SerializeField] private TMP_Text rpmText;
    [SerializeField] private TMP_Text gearText;
    [SerializeField] private TMP_Text speedText;
    // private float speed;
    [SerializeField] private float maxSpeed;
    // private float speedClamped;
    [SerializeField] private Transform rpmNeedle;
    [SerializeField] private float minRPMNeedleRotation;
    [SerializeField] private float maxRPMNeedleRotation;
    [SerializeField] private Transform speedNeedle;
    [SerializeField] private float minSpeedNeedleRotation;
    [SerializeField] private float maxSpeedNeedleRotation;
    [SerializeField] private int gear;
    private float wheelRPM;
    [SerializeField] private AnimationCurve horsePowerToRPMCurve;

    [SerializeField] private float[] gearRatios;
    [SerializeField] private float differentialRatio;
    private float torque;
    private float clutch;

    [SerializeField] private StickShift stickShift;

    [Space(10)]
    [Header("AUDIO")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioGearChange;
    [SerializeField] private AudioSource audioHazard;
    [SerializeField] private Vroomer vroomer;

    [Space(10)]
    [Header("LIGHTS")]
    [SerializeField] private GameObject indicatorArrowLeft;
    [SerializeField] private GameObject indicatorArrowRight;
    private bool hazardLightsActive;
    [SerializeField] private float blinkTime;

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

        for(int i = 0; i < seatSpots.Count; i++){
            seatsTaken.Add(0);
        }
        UpdateSeatsTaken();

        // KeybindsManager.current.onKeyChangeEvent += OnKeyChangeEvent;
        OnKeyChangeEvent();

        UpdateGearText();
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

    // CHECKS ======================================================================

    public bool HasFreeSeats() {
        // bool returnBool = false;
        // if(passengerCount < seatSpots.Count) returnBool = true; 

        // return returnBool;
        return (passengerCount < seatSpots.Count);
    }

    // INPUTS ============================================================================

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
        moveInput = 0;
        steerInput = 0;
        brakeInput = false;

        //movement
        if(Input.GetKey(Key_DriveForward)) {
            //if R or N, set to gear 1
            if(gear < 2) {
                SetGear(2);
            }
            moveInput = 1;

            //Muffler
            vroomer.MufflerCheck();
        } else if(Input.GetKey(Key_DriveBackward)) {
            //if forward, set to R
            if(gear > 0) {
                SetGear(0);
            }
            moveInput = -1;
        }

        //Braking
        brakeInput = Input.GetKey(Key_Brake);

        //Steering
        if(Input.GetKey(Key_SteerLeft)) steerInput = -1;
        else if(Input.GetKey(Key_SteerRight)) steerInput = 1;

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
        if(Input.GetKeyDown(Key_GearUp)) ShiftGear(1);
        else if(Input.GetKeyDown(Key_GearDown)) ShiftGear(-1);
    }

    // PASSENGERS ============================================================================

    public void TakeSeat(Transform passenger) {
        print("Taking seat");
        UpdateSeatsTaken();

        //Get free seats
        List<int> freeSeats = new List<int>();
        for(int i = 0; i < seatsTaken.Count; i++){
            if(seatsTaken[i] == 0) freeSeats.Add(i);
        }

        //NO SEATS
        if(freeSeats.Count == 0) return;

        //Pick random from free seats
        int randInt = UnityEngine.Random.Range(0, freeSeats.Count);
        int seatIndex = freeSeats[randInt];
        Transform seatSpot = seatSpots[seatIndex];

        //Setup
        print("seatspot: " + seatSpot.name);
        passenger.SetParent(seatSpot);
        passenger.localPosition = Vector3.zero;
        passenger.rotation = seatSpot.rotation;
        // passenger.position = seatSpot.position;
        // passenger.rotation = seatSpot.rotation;
        // passenger.SetParent(seatSpot);
        passengerCount ++;

        //KEEP IN TO AVOID CAR "HITTING SOMETHING" BUG. IDK WHY THIS HAPPENS OR WHY THIS EVEN WORKS BUT IT DOES
        seatSpot.gameObject.SetActive(false);
        seatSpot.gameObject.SetActive(true);

        //Set points
        passenger.GetComponent<PersonHandler>().payPoint = payPoint;
        passenger.GetComponent<PersonHandler>().changePoint = changePoint;
    }

    private void UpdateSeatsTaken() {
        // print("Updating seats taken");
        for(int i = 0; i < seatSpots.Count; i++) {
            if(seatSpots[i].childCount == 1) seatsTaken[i] = 0;
            else seatsTaken[i] = 1;
        }
    }

    // DRIVING ============================================================================

    public void SetEngine(bool newIsOn) {
        isEngineOn = newIsOn;

        if(newIsOn) audioSource.Play();
        else audioSource.Stop();

        GearAnimAudio(); // not sure if this belongs here, but it fixes some issues.
    }

    public void ToggleDriverSeat(Transform driver) {
        //EXIT
        if(driverPos.childCount > 0 && driverPos.GetChild(0) == driver) {
            //cameras

            driver.position = pointDriverExit.position;
            // driver.rotation = driverPos.rotation;
            // driver.SetParent(GameObject.Find("WORLD").transform);
            driver.SetParent(null);

            driver.GetComponent<PlayerDriveInput>().SetIsDriving(false, null);
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

        } 
        //ENTER
        else { 
            //cameras

            //Seating
            driver.SetParent(driverPos);
            //driver.localPosition = Vector3.zero;
            driver.localPosition = new Vector3(0f, 0f, 0.1f);
            // driver.rotation = driverPos.rotation;

            //Set player isDriving
            driver.GetComponent<PlayerDriveInput>().SetIsDriving(true, this);
        
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
                // if(RPM < idleRPM + 200 )
                wheelRPM = Mathf.Abs(wheel.wheelCollider.rpm) * gearRatios[gear] * differentialRatio;
                RPM = Mathf.Lerp(RPM, Mathf.Max(idleRPM, wheelRPM), Time.deltaTime * 3f);
                torque = (horsePowerToRPMCurve.Evaluate(RPM / redLine) * motorPower / RPM) * gearRatios[gear] * differentialRatio * 5252f;//*clutch
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

    private void ShiftGear(int gearAdd) {
        if(gearAdd > 0 && gear != gearRatios.Length - 1) {
            gear ++;
        } else if(gearAdd < 0 && gear != 0) {
            gear --;
        }

        GearAnimAudio();
    }

    private void SetGear(int newGear) {
        gear = newGear;

        GearAnimAudio();
    }

    private void GearAnimAudio() {
        UpdateGearText();
        stickShift.MoveToGear(gear);

        audioSource.PlayOneShot(audioGearChange);

        //Check for reverse
        if(gear == 0 && isEngineOn) { 
            //lights
            if(!hazardLightsActive) StartCoroutine(BlinkHazardLights());

            //audio
            audioHazard.Play();
        } else {
            //lights
            if(indicatorArrowLeft.activeSelf) indicatorArrowLeft.SetActive(false);
            if(indicatorArrowRight.activeSelf) indicatorArrowRight.SetActive(false);
            hazardLightsActive = false;

            //audio
            if (audioHazard.isPlaying) audioHazard.Stop();
        }
    }

    private IEnumerator BlinkHazardLights() {
        hazardLightsActive = true;

        while(gear == 0 && isEngineOn) { 
            // Toggle the visibility of both arrows
            indicatorArrowLeft.SetActive(!indicatorArrowLeft.activeSelf);
            indicatorArrowRight.SetActive(!indicatorArrowRight.activeSelf);

            // Wait for a short duration
            yield return new WaitForSeconds(blinkTime);
        }
    }

    private void UpdateGearText() {
        string returnText  = (gear-1).ToString();
        if(gear == 0) returnText = "R";
        else if(gear == 1) returnText = "N";
        gearText.text = returnText;
    }

    private void AnimateDashboard() {
        //needle rotation
        rpmNeedle.localRotation = Quaternion.Euler(Mathf.Lerp(minRPMNeedleRotation, maxRPMNeedleRotation, RPM/redLine), 0, 0);
        rpmText.text = RPM.ToString("0 000");
        float speedMpS = carRb.velocity.magnitude;
        float speedKpH = speedMpS*3.6f;
        speedNeedle.localRotation = Quaternion.Euler(Mathf.Lerp(minSpeedNeedleRotation, maxSpeedNeedleRotation, speedKpH/maxSpeed), 0, 0);
        speedText.text = speedKpH.ToString("000");
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
}

public enum GearState {
    Neutral,
    Running,
    CheckingChange,
    Changing
}