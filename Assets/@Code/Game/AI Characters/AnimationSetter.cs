using UnityEngine;

//For NPCs that arent passengers/just posing
public class AnimationSetter : MonoBehaviour {
    [SerializeField] private Animator ani;
    [SerializeField] private int animationInt;

    private void Start() {
        ani = GetComponent<Animator>();
        InvokeRepeating(nameof(CheckAnimationState), 0f, 1f);
    }

    // Method to check animation state every second
    private void CheckAnimationState() {
        if (ani.GetInteger("State") != animationInt) {
            ani.SetInteger("State", animationInt); 
        }
    }
}

/*
ANIMATION INDEX
NPCs
0-9: Idle
10-19: Walking
20-39: Sitting
40-49: Stopping
50-59: Calling jeep/Hailing
90: Guard

PLAYER
0: Idle
10: Walking
11: Walk left
12: Walk right
13: Walk back
100: Running
101: Driving
102: Crouching
103: Crouch walking

*/