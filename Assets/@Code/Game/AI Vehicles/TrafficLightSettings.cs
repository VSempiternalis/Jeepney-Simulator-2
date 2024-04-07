using UnityEngine;

public class TrafficLightSettings : MonoBehaviour {
    public static TrafficLightSettings current;

    public float roadActiveFor; //How long a road is green
    public float yellowActiveFor; //Time between yellow and green

    private void Awake() {
        current = this;
    }
}
