using UnityEngine;
using System.Collections.Generic;

public class TrafficLight : MonoBehaviour {
    [SerializeField] private List<GameObject> lights;
    [SerializeField] private List<GameObject> secondaryLights;
    [SerializeField] private Transform stopper;
    private Vector3 redStopperPos;
    private Vector3 greenStopperPos;
    private int currentIndex;

    private bool hasSecondaryLights;
    private bool hasStopper;

    private void Start() {
        if(stopper != null) hasStopper = true;

        if(hasStopper) {
            redStopperPos = stopper.localPosition;
            greenStopperPos = stopper.localPosition;
            greenStopperPos.y += 5;
        }

        if(secondaryLights.Count > 0) {
            hasSecondaryLights = true;
        }
    }

    private void Update() {
        if(!hasStopper) return;
        //Go down on red
        if(currentIndex == 0) {
            stopper.localPosition = Vector3.MoveTowards(stopper.localPosition, redStopperPos, 0.5f);
        } //Go up on green
        else if(currentIndex == 2) {
            stopper.localPosition = Vector3.MoveTowards(stopper.localPosition, greenStopperPos, 0.5f);
        }
    }
    
    public void SetLight(int index) {
        currentIndex = index;
        // lights[currentIndex].SetActive(false);

        // if(currentIndex == lights.Count - 1) currentIndex = 0;
        // else currentIndex ++;

        // lights[currentIndex].SetActive(true);

        for(int i = 0; i < lights.Count; i++) {
            if(i == index) lights[i].SetActive(true);
            else lights[i].SetActive(false);
        }

        if(hasSecondaryLights) {
            for(int i = 0; i < secondaryLights.Count; i++) {
                if(i == index) secondaryLights[i].SetActive(true);
                else secondaryLights[i].SetActive(false);
            }
        }
    }
}
