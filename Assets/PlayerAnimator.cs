using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    [SerializeField] private Animator ani;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private PlayerDriveInput pdi;

    private void Start() {
        
    }

    private void Update() {
        float speed = rb.velocity.magnitude;

        if(pdi.isDriving) ani.SetInteger("State", 5);
        else if(speed >= 4f) ani.SetInteger("State", 4);
        else if(speed >= 0.1f) ani.SetInteger("State", 1);
        else ani.SetInteger("State", 0);
    }
}
