using UnityEngine;
using System.Collections.Generic;

public class StickShift : MonoBehaviour {
    [SerializeField] private List<GearPoint> points;
    [SerializeField] private GearPoint currentPoint;
    [SerializeField] private GearPoint targetPoint;
    [SerializeField] private string state;

    private void Start() {
        
    }

    private void Update() {
        if(state == "Moving to midpoint") {
            if(currentPoint == targetPoint.midPoint) {
                state = "Moving to target point";
            } else {
                //move to targetPoint.midPoint
            }
        } else if(state == "Exiting") { //From current point to mid point

        }
    }

    public void MoveToPoint(string newPointName) {
        foreach(GearPoint point in points) {
            if(point.pointName == newPointName) targetPoint = point;

            if(currentPoint.isMidPoint) state = "Moving to midpoint";
            else state = "Exiting";
        }
    }
}
