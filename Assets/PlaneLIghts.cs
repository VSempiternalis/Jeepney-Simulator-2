using UnityEngine;

public class PlaneLIghts : MonoBehaviour {
    public GameObject lightsParent;
    public float onDuration = 0.5f;
    public float offDuration = 0.5f;

    private float timer;
    private bool lightsOn = false;

    private void Start() {
        if(lightsParent == null) {
            Debug.LogError("LightsParent is not assigned. Please assign it in the inspector.");
            return;
        }

        timer = 0.0f;
        lightsParent.SetActive(false); // Start with lights off
    }

    private void Update() {
        if(lightsParent == null) return;

        timer += Time.deltaTime;

        if(lightsOn) {
            if(timer >= onDuration) {
                lightsParent.SetActive(false);
                lightsOn = false;
                timer = 0.0f; // Reset the timer
            }
        } else {
            if(timer >= offDuration) {
                lightsParent.SetActive(true);
                lightsOn = true;
                timer = 0.0f; // Reset the timer
            }
        }
    }
}
