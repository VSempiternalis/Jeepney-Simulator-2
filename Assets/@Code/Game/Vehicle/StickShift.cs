using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class StickShift : MonoBehaviour {
    [SerializeField] private List<GearPoint> points;
    [SerializeField] private List<GearPoint> moveToPoints;
    [SerializeField] private GearPoint currentPoint;
    [SerializeField] private GearPoint targetPoint;
    [SerializeField] private string state;
    [SerializeField] private float moveTime;

    private void Start() {
        
    }

    private void Update() {
        if(state == "Moving" && moveToPoints.Count > 0) {
            float distToNext = 1;
            Vector3 thisPos = transform.position;
            Vector3 targetPos = targetPoint.transform.position;
            Vector3 nextPos = moveToPoints[0].transform.position;

            if(targetPoint) distToNext = Vector3.Distance(transform.position, moveToPoints[0].transform.position);

            if(distToNext <= 0.001f) {
                moveToPoints.RemoveAt(0);
                if(moveToPoints.Count == 0) {
                    transform.position = targetPoint.transform.position;
                    currentPoint = targetPoint;
                    state = "Idle";
                }
            } else {
                // print(Time.time + "> [StickShift] Moving to: " + moveToPoints[0].pointName);
                //move to next point
                transform.position = Vector3.Lerp(transform.position, moveToPoints[0].transform.position, moveTime);
            }
        }
    }

    public void MoveToGear(int gearIndex) {
        // GearPoint newTargetPoint = points[gearIndex];
        if(targetPoint == points[gearIndex]) return;
        targetPoint = points[gearIndex];

        //[] If already moving, teleport to newpoint
        if(state == "Moving") {
        // if(state == "Moving" || targetPoint == newTargetPoint) {
            currentPoint = targetPoint;
            transform.position = targetPoint.transform.position;
            moveToPoints.Clear();
            state = "Idle";
            return;
        }

        //[] Clear movetopoints
        // targetPoint = newTargetPoint;
        moveToPoints.Clear();

        //[] get exit points
        if(!currentPoint.isMidPoint) moveToPoints.Add(currentPoint.midPoint);
        //[] get entrance points
        if(!targetPoint.isMidPoint) moveToPoints.Add(targetPoint.midPoint);
        moveToPoints.Add(targetPoint);

        state = "Moving";
    }
}
