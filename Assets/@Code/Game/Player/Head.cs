using JetBrains.Annotations;
using UnityEngine;

public class Head : MonoBehaviour {
    [SerializeField] private PlayerDriveInput pdi;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Crouch crouch;
    [SerializeField] private PlayerAnimator pAni;

    [SerializeField] private Vector3 idleCamPos;
    [SerializeField] private Vector3 runCamPos;
    [SerializeField] private Vector3 crouchCamPos;
    [SerializeField] private Vector3 driveCamPos;

    public string state;
    private float clamp;
    public bool isDriving;

    private void Start() {
        
    }

    // public void Update() {
    //     if(isDriving) {
    //         if(state == "Driving") return;
    //         state = "Driving";
    //         transform.localPosition = driveCamPos;
    //         pAni.SetState("Driving");
    //         clamp = 60;
    //     } else if(state == "Crouching") {
    //         if(state == "Crouching") return;

    //         //Crouch Walk
    //         if(rb.velocity.magnitude >= 0.1f) {
    //             state = "Crouching walking";
    //             transform.localPosition = crouchCamPos;
    //             pAni.SetState("Crouching walking");
    //             clamp = 55;
    //         } else {
    //             state = "Crouching";
    //             transform.localPosition = crouchCamPos;
    //             pAni.SetState("Crouching");
    //             clamp = 55;
    //         }
    //     } else if(rb.velocity.magnitude >= 5f) {
    //         if(state == "Running") return;
    //         state = "Running";
    //         transform.localPosition = runCamPos;
    //         pAni.SetState("Running");
    //         clamp = 10;
    //     } else if(rb.velocity.magnitude >= 0.1f) {
    //         if(state == "Walking") return;
    //         state = "Walking";
    //         transform.localPosition = idleCamPos;
    //         pAni.SetState("Walking");
    //         clamp = 10;
    //     } else {
    //         if(state == "Idle") return;
    //         state = "Idle";
    //         transform.localPosition = idleCamPos;
    //         pAni.SetState("Idle");
    //         clamp = 55;
    //     }
    // }

    public void CheckState() {
        if(isDriving && state != "Driving") {
            state = "Driving";
            transform.localPosition = driveCamPos;
            pAni.SetState("Driving");
            clamp = 60;
        } else if(state == "Crouching") {
            state = "Crouching";
            transform.localPosition = crouchCamPos;
            pAni.SetState("Crouching");
            clamp = 55;
        } else if(state == "Crouch walking") {
            state = "Crouch walking";
            transform.localPosition = crouchCamPos;
            pAni.SetState("Crouch walking");
            clamp = 55;
        } else if(state == "Running") {
            state = "Running";
            transform.localPosition = runCamPos;
            pAni.SetState("Running");
            clamp = 10;
        } else if(state == "Walking") {
            state = "Walking";
            transform.localPosition = idleCamPos;
            pAni.SetState("Walking");
            clamp = 55;
        } else if(state == "Walking back") {
            state = "Walking back";
            transform.localPosition = idleCamPos;
            pAni.SetState("Walking back");
            clamp = 55;
        } else if(state == "Walking left") {
            state = "Walking left";
            transform.localPosition = idleCamPos;
            pAni.SetState("Walking left");
            clamp = 55;
        } else if(state == "Walking right") {
            state = "Walking right";
            transform.localPosition = idleCamPos;
            pAni.SetState("Walking right");
            clamp = 55;
        } else if(state == "Idle") {
            state = "Idle";
            transform.localPosition = idleCamPos;
            pAni.SetState("Idle");
            clamp = 55;
        }
    }

    public float GetClamp() {
        return clamp;
    }
}
