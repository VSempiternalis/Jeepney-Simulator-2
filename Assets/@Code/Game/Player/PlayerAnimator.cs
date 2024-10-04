using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    [SerializeField] private Animator ani;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private PlayerDriveInput pdi;
    // [SerializeField] private Head head;

    private void Start() {
        
    }

    private void Update() {
        // float speed = rb.velocity.magnitude;

        // if(pdi.isDriving) ani.SetInteger("State", 5);
        // else if(speed >= 4f) ani.SetInteger("State", 4);
        // else if(speed >= 0.1f) ani.SetInteger("State", 1);
        // else ani.SetInteger("State", 0);
    }

    public void SetState(string newState) {
        // float speed = rb.velocity.magnitude;

        if(newState == "Driving") ani.SetInteger("State", 101);
        else if(newState == "Sitting") ani.SetInteger("State", 20);
        else if(newState == "Crouching") ani.SetInteger("State", 102);
        else if(newState == "Crouch walking") ani.SetInteger("State", 103);
        else if(newState == "Running") ani.SetInteger("State", 100);
        else if(newState == "Walking") ani.SetInteger("State", 10);
        else if(newState == "Walking left") ani.SetInteger("State", 11);
        else if(newState == "Walking right") ani.SetInteger("State", 12);
        else if(newState == "Walking back") ani.SetInteger("State", 13);
        else ani.SetInteger("State", 0);
    }
}
