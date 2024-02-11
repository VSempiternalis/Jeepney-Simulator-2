﻿using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;

    Rigidbody rb;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    [SerializeField, Tooltip("Prevents jumping when the transform is in mid-air.")]
    GroundCheck groundCheck;

    [Header("Keybinds")]
    public KeyCode Key_Forward;
    public KeyCode Key_Left;
    public KeyCode Key_Right;
    public KeyCode Key_Back;
    public KeyCode Key_Run;
    public KeyCode Key_Crouch;

    [SerializeField] private Head head;

    void Awake() {
        // Get the rb on this.
        rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        Keybinds.current.onKeyChangeEvent += OnKeyChangeEvent;

        OnKeyChangeEvent();
    }

    void FixedUpdate() {
        if((!groundCheck || groundCheck.isGrounded)) {
            // Update IsRunning from input.
            IsRunning = canRun && Input.GetKey(Key_Run) && Input.GetKey(Key_Forward);

            // Get targetMovingSpeed.
            float targetMovingSpeed = IsRunning ? runSpeed : speed;
            if (speedOverrides.Count > 0)
            {
                targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
            }

            // Get targetVelocity from input.
            Vector2 targetVelocity =new Vector2( Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

            // Apply movement.
            rb.velocity = transform.rotation * new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.y);

            //Animation
            if(Input.GetKey(Key_Crouch) && Input.GetKey(Key_Forward)) {
                if(head.state == "Crouch walking") return;
                head.state = "Crouch walking";
                head.CheckState();
            } else if(Input.GetKey(Key_Crouch)) {
                if(head.state == "Crouching") return;
                head.state = "Crouching";
                head.CheckState();
            } else if(Input.GetKey(Key_Run) && Input.GetKey(Key_Forward)) {
                if(head.state == "Running") return;
                head.state = "Running";
                head.CheckState();
            } else if(Input.GetKey(Key_Forward)) {
                if(head.state == "Walking") return;
                head.state = "Walking";
                head.CheckState();
            } else if(Input.GetKey(Key_Back)) {
                if(head.state == "Walking back") return;
                head.state = "Walking back";
                head.CheckState();
            } else if(Input.GetKey(Key_Left)) {
                if(head.state == "Walking left") return;
                head.state = "Walking left";
                head.CheckState();
            } else if(Input.GetKey(Key_Right)) {
                if(head.state == "Walking right") return;
                head.state = "Walking right";
                head.CheckState();
            } else if(head.state != "Idle"){
                head.state = "Idle";
                head.CheckState();
            }
        }
    }

    private void OnKeyChangeEvent() {
        Key_Forward = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_Forward", "W"));
        Key_Back = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_Back", "S"));
        Key_Left = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_Left", "A"));
        Key_Right = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_Right", "D"));
        Key_Run = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_Run", "LeftShift"));
        Key_Crouch = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_Crouch", "LeftControl"));
        // Key_Horn = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_Horn", "F"));;
        // Key_Brake = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_Brake", "Space"));;
        // Key_GearUp = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_GearUp", "LeftShift"));;
        // Key_GearDown = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_GearDown", "LeftControl"));;
        // Key_TowTruck = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_TowTruck", "T"));;
    }
}