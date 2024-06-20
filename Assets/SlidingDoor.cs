using UnityEngine;

public class SlidingDoor : MonoBehaviour {
    private bool isSliding;
    
    [SerializeField] private GameObject door1;
    [SerializeField] private GameObject door2;

    [SerializeField] private Vector3 door1OpenPos;
    [SerializeField] private Vector3 door2OpenPos;
    [SerializeField] private Vector3 door1ClosedPos;
    [SerializeField] private Vector3 door2ClosedPos;

    [SerializeField] private float speed;

    //AUDIO
    [SerializeField] private AudioHandler ah;

    private void Start() {
        door1ClosedPos = door1.transform.localPosition;
        door2ClosedPos = door2.transform.localPosition;
    }

    private void Update() {
        
    }

    public void OpenDoors() {
        if(door1.transform.position == door1OpenPos && door2.transform.position == door2OpenPos) return;

        ah.Play(0);
        ah.PlayOneShot(1);
        LeanTween.moveLocal(door1, door1OpenPos, speed).setEaseInOutCubic();
        LeanTween.moveLocal(door2, door2OpenPos, speed).setEaseInOutCubic();
    }
    
    public void CloseDoors() {
        if(door1.transform.position == door1ClosedPos && door2.transform.position == door2ClosedPos) return;

        ah.Play(1);
        LeanTween.moveLocal(door1, door1ClosedPos, speed).setEaseInOutCubic();
        LeanTween.moveLocal(door2, door2ClosedPos, speed).setEaseInOutCubic();
    }
}
