using UnityEngine;

public class AnimationSetter : MonoBehaviour {
    [SerializeField] private Animator ani;
    [SerializeField] private int animationInt;

    private void Start() {
        ani = GetComponent<Animator>();
        ani.SetInteger("State", animationInt);       
    }
}

/*
ANIMATION INDEX
NPCs
0-9: Idle
10-19: Walking
20-29: Sitting

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