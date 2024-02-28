using UnityEngine;
using System.Collections.Generic;

public class TrafficIntersection : MonoBehaviour {
    [SerializeField] private TrafficLight trafficLight1;
    [SerializeField] private TrafficLight trafficLight2;
    [SerializeField] private TrafficLight trafficLight3;
    [SerializeField] private TrafficLight trafficLight4;
    private List<TrafficLight> lights = new List<TrafficLight>();

    private float roadActiveFor; //How long a road is green
    private float yellowActiveFor; //Time between yellow and green
    private float switchRoadTime; //The time when the switch occurs
    private float lightYellowTime; //Time when yellow occurs
    private int currentIndex = 0;
    private bool yellowActive;

    private float nextSecUpdate;

    private void Start() {
        if(trafficLight1 != null) {
            lights.Add(trafficLight1);
        }
        if(trafficLight2 != null) {
            lights.Add(trafficLight2);
        }
        if(trafficLight3 != null) {
            lights.Add(trafficLight3);
        }
        if(trafficLight4 != null) {
            lights.Add(trafficLight4);
        }

        nextSecUpdate = Time.time + 1;

        roadActiveFor = TrafficLightSettings.current.roadActiveFor;
        yellowActiveFor = TrafficLightSettings.current.yellowActiveFor;

        SetTimes();
        lights[currentIndex].SetLight(2);
    }

    private void FixedUpdate() {
        if(Time.time >= (nextSecUpdate)) {
            nextSecUpdate ++;
            
            //check if lightyellowtime
            if(!yellowActive && Time.time >= lightYellowTime) {
                yellowActive = true;
                
                //Switch next light to yellow
                int nextIndex = currentIndex;
                if(nextIndex == lights.Count - 1) nextIndex = 0;
                else nextIndex ++;

                lights[nextIndex].SetLight(1);

                //Set current index to red
                SetOff(currentIndex);
            }

            //check if green time
            else if(Time.time >= switchRoadTime) {
                yellowActive = false;

                NextRoad();
            }
        }
    }

    private void SetTimes() {
        switchRoadTime = Time.time + roadActiveFor;
        lightYellowTime = switchRoadTime - yellowActiveFor;
    }

    private void NextRoad() {
        SetOff(currentIndex);

        if(currentIndex == lights.Count - 1) currentIndex = 0;
        else currentIndex ++;

        SetOn(currentIndex);
        SetTimes();
    }

    private void SetOff(int index) {
        lights[index].SetLight(0);
    }

    private void SetOn(int index) {
        lights[index].SetLight(2);
    }
}
