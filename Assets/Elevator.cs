using UnityEngine;
using System.Collections.Generic;

public class Elevator : MonoBehaviour {
    private bool isBeingUsed;

    private Transform player;
    [SerializeField] private float elevateDelay;
    [SerializeField] private float elevationDiff;

    [SerializeField] private List<SlidingDoor> doors;

    private void Start() {
        player = PlayerDriveInput.current.transform;
    }

    private void Update() {
        
    }

    public void OpenDoors(int level) {
        doors[level].OpenDoors();
    }

    public void CloseDoors(int level) {
        doors[level].CloseDoors();
    }

    public void ElevatePlayer(float meters) {
        LeanTween.delayedCall(elevateDelay, () => {
            player.position += new Vector3(0, meters, 0);
        });
    }

    public void CloseAndElevate(int originLevel) {
        if(isBeingUsed) return;
        isBeingUsed = true;

        CloseDoors(originLevel);

        if(originLevel == 0) {
            LeanTween.delayedCall(elevateDelay, () => {
                player.position += new Vector3(0, elevationDiff, 0);
            });
            LeanTween.delayedCall(elevateDelay*2, () => {
                OpenDoors(1);
            });
            LeanTween.delayedCall(elevateDelay*2+1, () => {
                isBeingUsed = false;
            });
        } else {
            LeanTween.delayedCall(elevateDelay, () => {
                player.position += new Vector3(0, -elevationDiff, 0);
            });
            LeanTween.delayedCall(elevateDelay*2, () => {
                OpenDoors(0);
            });
            LeanTween.delayedCall(elevateDelay*2+1, () => {
                isBeingUsed = false;
            });
        }
    }
}
