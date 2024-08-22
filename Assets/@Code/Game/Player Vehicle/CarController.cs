using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CarController : MonoBehaviour {
    [SerializeField] private GameObject headlights;
    private TimeManager tm;

    [Space(10)]
    [Header("FUEL")]
    [SerializeField] private CarEngineButton carEngineButton;
    [SerializeField] private Transform fuelNeedle;
    [SerializeField] private TMP_Text fuelText;
    public int fuelAmount;
    public int fuelCapacity;
    // public bool hasFuel;
    // public bool engineIsOn;
    public int fuelLoss; //fuel lost per fixed frame
    // public bool isFuelLossActive;
    [SerializeField] private float minFuelNeedleRotation;
    [SerializeField] private float maxFuelNeedleRotation;
    public bool isPumpingFuel;

    [Space(10)]
    [Header("NAME")]
    public string jeepName;
    [SerializeField] private List<TMP_Text> namePlates;

    [Space(10)]
    [Header("VARIABLES")]
    public int health;
    public float healthFactor = 1;
    public int maxHealth;
    public bool isEngineOn;
    public float maxAcceleration = 30.0f;
    public float brakeAcceleration = 50.0f;

    public int damage;

    public float turnSens = 1.0f;
    public float maxSteerAngle = 30.0f;

    public Vector3 centerOfMass;

    public float moveInput;
    public float steerInput;
    private bool brakeInput;

    private Rigidbody carRb;
    public float freeDrag;

    [Space(10)]
    [Header("HEALTH PARTICLES")]
    [SerializeField] private GameObject flamesSmall;
    [SerializeField] private GameObject flamesMedium;
    [SerializeField] private GameObject flamesLarge;

    [Space(10)]
    [Header("PASSENGERS")]
    public int passengerCount;
    public List<Transform> seatSpots;
    private List<int> seatsTaken = new List<int>();
    [SerializeField] private LayerMask collisionLayer;
    // [SerializeField] private List<Transform> leftSeats; //[!] left (perspective of passenger entering from rear)

    [Space(10)]
    [Header("POINTS")]
    [SerializeField] private Transform driverPos;
    [SerializeField] private Transform pointDriverExit;
    public List<Transform> points;
    public Transform payPoint;
    public ChangeHandler changePoint;
    public Transform pointPassengerEntrance;
    public Transform pointPassengerExitLeft;
    public Transform pointPassengerExitRight;

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
    public int maxGear;
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
    [SerializeField] private AudioSource metalAudioSource;
    [SerializeField] private AudioClip audioGearChange;
    [SerializeField] private AudioSource audioHazard;
    [SerializeField] private Vroomer vroomer;
    [SerializeField] private AudioHandler ah;
    [SerializeField] private AudioClip hornAudio;
    [SerializeField] private AudioClip metalAudio;

    [Space(10)]
    [Header("LIGHTS")]
    [SerializeField] private GameObject indicatorArrowLeft;
    [SerializeField] private GameObject indicatorArrowRight;
    private bool hazardLightsActive;
    [SerializeField] private float blinkTime;

    [Space(10)]
    [Header("KEYBINDS")]
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
    private KeyCode Key_Map;

    [Space(10)]
    [Header("TUTORIAL UI")]
    [SerializeField] private TMP_Text rpmTextUI;
    [SerializeField] private TMP_Text speedTextUI;
    [SerializeField] private TMP_Text fuelTextUI;
    [SerializeField] private TMP_Text gearTextUI;
    [SerializeField] private Color red;
    [SerializeField] private Color white;

    [Space(10)]
    [Header("AUTO TRANS")]
    public bool isAutoTrans;

    [Space(10)]
    [Header("OTHERS")]
    [SerializeField] private ParticleSystem smokeParticles;
    [SerializeField] private Tablet tablet;
    public MusicPlayer mp;

    [Space(10)]
    [Header("ACHIEVEMENTS")]
    private bool isSmoothRide;
    private bool isPerfectionist;
    private bool isFullyLoaded;

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
        ah = GetComponent<AudioHandler>();
        carRb = GetComponent<Rigidbody>();
        tm = TimeManager.current;
        carRb.centerOfMass = centerOfMass;
        // swStandardRot = steeringWheel.transform.rotation.eulerAngles;
        swStandardRot.x = -34.624f;
        swStandardRot.y = 180;
        swStandardRot.z = 0;

        for(int i = 0; i < seatSpots.Count; i++){
            seatsTaken.Add(0);
        }
        UpdateSeatsTaken();

        if(Keybinds.current) Keybinds.current.onKeyChangeEvent += OnKeyChangeEvent;
        OnKeyChangeEvent();

        UpdateGearText();
    }

    private void Update() {
        if(isAutoTrans) AutoTrans();
        AnimateWheels();
        AnimateDashboard();
    }

    private void FixedUpdate() {
        Move();
        Steer();
        Brake();
        FuelDrain();

        if(fuelAmount < 5000 && fuelAmount > 4900) NotificationManager.current.NewNotifColor("LOW FUEL!", "You're low on fuel! Visit the nearest EZ Gas and refill!", 2);
        else if(fuelAmount < 2500 && fuelAmount > 2400) NotificationManager.current.NewNotifColor("VERY LOW FUEL!", "You're low on fuel! Visit the nearest EZ Gas and refill!", 2);
        else if(fuelAmount < 10 && fuelAmount >= 0 ) NotificationManager.current.NewNotifColor("NO FUEL!", "You have run out of fuel! Call a tow truck.", 3);
    }

    // CHECKS ======================================================================

    public bool HasFreeSeats() {
        // bool returnBool = false;
        // if(passengerCount < seatSpots.Count) returnBool = true; 

        // return returnBool;
        return (passengerCount < seatSpots.Count);
    }

    public void NewDay() {
        changePoint.ClearChangees();

        foreach(Transform seat in seatSpots) {
            if(seat.childCount > 0) {
                Transform passenger = seat.GetChild(0);
                // passenger.GetComponent<PersonHandler>().patience = 0;
                passenger.GetComponent<PersonHandler>().ExitVehicle();
                passenger.GetComponent<PersonHandler>().state = "Idle";
                passenger.GetComponent<Despawner>().Despawn();

                isPerfectionist = false;
            }
        }

        //reset fuel to full
        // fuelAmount = fuelCapacity;

        //STEAM ACH
        if(isSmoothRide) SteamAchievements.current.UnlockAchievement("ACH_SMOOTH_RIDE");
        if(isPerfectionist) SteamAchievements.current.UnlockAchievement("ACH_PERFECTIONIST");
        // if(isFullyLoaded) SteamAchievements.current.UnlockAchievement("ACH_FULLY_LOADED");

        //RESET ACH
        isSmoothRide = true;
        isPerfectionist = true;
        // isFullyLoaded = false;
    }

    #region INPUTS ======================================================================

    private void OnKeyChangeEvent() {
        //Set keys
        Key_DriveForward = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_DriveForward", "W"));
        Key_DriveBackward = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_DriveBackward", "S"));
        Key_SteerLeft = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_SteerLeft", "A"));
        Key_SteerRight = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_SteerRight", "D"));
        Key_Headlights = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_Headlights", "L"));
        Key_Horn = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_Horn", "H"));
        Key_Brake = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_Brake", "Space"));
        Key_GearUp = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_GearUp", "LeftShift"));
        Key_GearDown = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_GearDown", "LeftControl"));
        Key_TowTruck = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_TowTruck", "T"));
        Key_Map = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_Map", "Tab"));
    }

    public void GetInput() {
        moveInput = 0;
        steerInput = 0;
        brakeInput = false;

        //movement
        if(Input.GetKey(Key_DriveForward) && !isEngineOn && !carEngineButton.isOn) {
            carEngineButton.Interact(gameObject);
        }
        
        if(isEngineOn && fuelAmount > 0) {
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
        }

        //Braking
        brakeInput = Input.GetKey(Key_Brake);

        //Map
        if(Input.GetKeyDown(Key_Map)) {
            tablet.Toggle();
        }
        
        //Lights
        if(Input.GetKeyDown(Key_Headlights)) {
            headlights.SetActive(!headlights.activeSelf);
            AudioManager.current.PlayUI(0);            
        }

        //Steering
        if(Input.GetKey(Key_SteerLeft)) steerInput = -1;
        else if(Input.GetKey(Key_SteerRight)) steerInput = 1;

        if(steerInput != 0) {
            Vector3 newRot = swStandardRot;
            newRot.z = steerInput * 70;

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

        if(Input.GetKeyDown(Key_Horn)) Horn();
    }

    #endregion

    #region PASSENGERS ======================================================================

    public void TakeSeat(Transform passenger) {
        // print("Taking seat");
        UpdateSeatsTaken();

        //Get free seats
        List<int> freeSeats = new List<int>();
        for(int i = 0; i < seatsTaken.Count; i++){
            if(seatsTaken[i] == 0) freeSeats.Add(i);
        }

        //NO SEATS
        if(freeSeats.Count == 0) return;
        else if(freeSeats.Count == 1) SteamAchievements.current.UnlockAchievement("ACH_FULLY_LOADED");

        //Pick random from free seats
        int randInt = UnityEngine.Random.Range(0, freeSeats.Count);
        int seatIndex = freeSeats[randInt];
        Transform seatSpot = seatSpots[seatIndex];

        //Setup
        // print("seatspot: " + seatSpot.name);
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
        passenger.GetComponent<PersonHandler>().payPoint = payPoint.GetComponent<StorageHandler>();
        passenger.GetComponent<PersonHandler>().changePoint = changePoint;
        passenger.GetComponent<PersonHandler>().changePointStorage = changePoint.GetComponent<StorageHandler>();
    }

    private void UpdateSeatsTaken() {
        // print("Updating seats taken");
        for(int i = 0; i < seatSpots.Count; i++) {
            if(seatSpots[i].childCount == 0) seatsTaken[i] = 0;
            else seatsTaken[i] = 1;
        }
    }

    public void PassengerExit(Transform passenger) {
        Transform seatSpot = passenger.parent;

        passenger.parent = GameObject.Find("WORLD").transform;
        Vector3 dropPos = pointPassengerEntrance.position;
        dropPos.y += 0.5f;
        passenger.position = dropPos;

        passengerCount --;
    }

    #endregion

    #region FUEL ======================================================================

    private void FuelDrain() {
        if(!isEngineOn || tm.isPauseShift) return;

        AddFuel(-fuelLoss);
    }

    public void AddFuel(int addAmount) {
        if(addAmount > 0 && fuelAmount >= fuelCapacity) return;

        fuelAmount += addAmount;

        if(fuelAmount <= 0) {
            fuelAmount = 0;
            
            moveInput = 0;
            // steerInput = 0;
            // brakeInput = false;

            if(isEngineOn) carEngineButton.Interact(gameObject);
        } else if(fuelAmount > fuelCapacity) {
            fuelAmount = fuelCapacity;
        }
    }

    public void PumpingFuel(bool newVal) {
        if(isPumpingFuel == newVal) return;
        
        isPumpingFuel = newVal;

        AudioManager.current.PlayFuelPump(newVal);

        // if(isPumpingFuel && isEngineOn) {
        //     carEngineButton.Interact(gameObject);
        // }
    }

    #endregion

    #region DRIVING ======================================================================

    private void Horn() {
        audioHazard.PlayOneShot(hornAudio);
        // ah.PlayOneShot(0);
    }

    public void DriverExit() {
        tablet.Out();
    }

    public void SetEngine(bool newIsOn) {
        // print("Sett Engine. newIsOn: " + newIsOn + " isPumpingFuel: " + isPumpingFuel);
        // if(isPumpingFuel && !isEngineOn) return;

        isEngineOn = newIsOn;
        if(isEngineOn) smokeParticles.Play();
        else smokeParticles.Stop();
        // smokeParticles.SetActive(isEngineOn);

        if(newIsOn) audioSource.Play();
        else audioSource.Stop();

        GearAnimAudio(); // not sure if this belongs here, but it fixes some issues.
    }

    private void Move() {
        if(isPumpingFuel) return;

        // torque = CalculateTorque();
        torque = 0;
        foreach(Wheel wheel in wheels) {
            if(wheel.axle == Axle.Rear) {
                // if(RPM < idleRPM + 200 )
                wheelRPM = Mathf.Abs(wheel.wheelCollider.rpm) * gearRatios[gear] * differentialRatio;
                RPM = Mathf.Lerp(RPM, Mathf.Max(idleRPM, wheelRPM), Time.deltaTime * 3f);
                torque = (horsePowerToRPMCurve.Evaluate(RPM / redLine) * motorPower / RPM) * gearRatios[gear] * differentialRatio * 5252f;//*clutch
                wheel.wheelCollider.motorTorque = moveInput * torque * healthFactor;
            }
            // if(wheel.axle == Axle.Rear) wheel.wheelCollider.motorTorque = moveInput * maxAcceleration;
        }
    }

    private void Steer() {
        foreach(Wheel wheel in wheels) {
            if(wheel.axle == Axle.Front) {
                float steerAngle = steerInput * turnSens * maxSteerAngle;
                // wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 1.0f);
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, swRotSpeed);
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
        if(gearAdd > 0 && gear == maxGear) return;

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
    
    private void AutoTrans() {
        if(gear != 0 && RPM > 6800) ShiftGear(1);
        else if(gear > 2 && RPM < 5000) ShiftGear(-1);
    }

    #endregion

    #region ANIMS ======================================================================

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
        string returnText  = (gear-1) + "/" + (maxGear-1);
        if(gear == 0) returnText = "R";
        else if(gear == 1) returnText = "N";
        gearText.text = returnText;

        //TUTORIAL UI
        gearTextUI.text = returnText;
    }

    private void AnimateDashboard() {
        //RPM
        rpmNeedle.localRotation = Quaternion.Euler(Mathf.Lerp(minRPMNeedleRotation, maxRPMNeedleRotation, RPM/redLine), 0, 0);
        Color rpmColor = white;
        if(RPM > 6800) {
            rpmColor = red;
            if(gear != maxGear && !metalAudioSource.isPlaying) metalAudioSource.Play();
        }
        rpmText.text = RPM.ToString("0 000");
        if(rpmText.color != rpmColor) rpmText.color = rpmColor;

        //SPEED
        float speedMpS = carRb.velocity.magnitude;
        float speedKpH = speedMpS*3.6f;
        speedNeedle.localRotation = Quaternion.Euler(Mathf.Lerp(minSpeedNeedleRotation, maxSpeedNeedleRotation, speedKpH/maxSpeed), 0, 0);
        speedText.text = speedKpH.ToString("000");
        
        //FUEL
        fuelNeedle.localRotation = Quaternion.Euler(Mathf.Lerp(minFuelNeedleRotation, maxFuelNeedleRotation, (float)fuelAmount/(float)fuelCapacity), 0, 0);
        Color fuelColor = white;
        // if(fuelAmount < 5000 && fuelAmount > 4997) {
        //     rpmColor = red;
        // }
        fuelText.text = Mathf.Round((float)fuelAmount/1000f) + "/" + Mathf.Round((float)fuelCapacity/1000f);
        if(fuelText.color != fuelColor) fuelText.color = fuelColor;

        //TUTORIAL UI
        rpmTextUI.text = RPM.ToString("0 000");
        if(rpmTextUI.color != rpmColor) rpmTextUI.color = rpmColor;
        speedTextUI.text = speedKpH.ToString("000");
        fuelTextUI.text = Mathf.Round((float)fuelAmount/1000f) + "/" + Mathf.Round((float)fuelCapacity/1000f) + "L";
        if(fuelTextUI.color != fuelColor) fuelTextUI.color = fuelColor;
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

    #endregion

    #region OTHERS ======================================================================

    public void AddHealth(int mod) {
        health += mod;

        UpdateFlameFX();
    }

    public void SetHealth(int newVal) {
        health = newVal;

        UpdateFlameFX();
    }

    public void SetMaxGear(int newVal) {
        maxGear = newVal;

        UpdateGearText();
    }

    private void UpdateFlameFX() {
        if(health <= 0) {
            health = 0;

            healthFactor = 0.2f;
        } else if(health < 25) {
            flamesSmall.SetActive(false);
            flamesMedium.SetActive(false);

            flamesLarge.SetActive(true);

            healthFactor = 0.4f;
        } else if(health < 50) {
            flamesSmall.SetActive(false);
            flamesLarge.SetActive(false);

            flamesMedium.SetActive(true);

            healthFactor = 0.6f;
        } else if(health < 75) {
            flamesMedium.SetActive(false);
            flamesLarge.SetActive(false);

            flamesSmall.SetActive(true);

            healthFactor = 0.8f;
        } else {
            flamesSmall.SetActive(false);
            flamesMedium.SetActive(false);
            flamesLarge.SetActive(false);

            healthFactor = 1f;
        }
    }

    public void Rename(string newName) {
        jeepName = newName;
        foreach(TMP_Text namePlate in namePlates) {
            namePlate.text = jeepName;
        }
    }

    #endregion

    #region COLLISIONS ======================================================================

    private void OnCollisionEnter(Collision other) {
        // print("car collision");
        if((collisionLayer.value & (1 << other.gameObject.layer)) != 0) {
            // print("in layer");
            // Calculate the relative velocity between the two colliding objects
            float relativeVelocity = other.relativeVelocity.magnitude;
            // print("COLLISION: relvel:" + relativeVelocity);

            if(relativeVelocity > 7) { //tolerance
                // damage vehicle
                AddHealth(-(int)(relativeVelocity/2));
                NotificationManager.current.NewNotifColor("VEHICLE DAMAGED!", "Jeepney health: " + health, 3);

                //damage other health
                if(other.gameObject.GetComponent<IHealth>() != null) other.gameObject.GetComponent<IHealth>().AddHealth(-(int)(damage + relativeVelocity));

                AudioManager.current.PlayUI(14);

                //STEAM ACH
                isSmoothRide = false;

                if(other.gameObject.layer == 18) SteamAchievements.current.AddKill();
            }
        }
    }

    #endregion
}