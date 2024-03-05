using UnityEngine;
using System.Collections;

public class SKYPRO_Sun_Rotator : MonoBehaviour {
    public static SKYPRO_Sun_Rotator current;
    // [SerializeField] private float rotationSpeed = 0.5f;
    public float time;
    [SerializeField] private bool isMainMenuRotator;

    private void Awake() {
        current = this;
    }

    private void Start() {
        // StartCoroutine(UpdateRotation());
    }

    private void Update() {
        if(isMainMenuRotator) time += Time.deltaTime;
    }

    void FixedUpdate() {
        Rotate();
    }

    //runs every second
    private IEnumerator UpdateRotation() {
        while(true) {
            yield return new WaitForSeconds(1f);
            time ++;
            Rotate();
        }
    }

    void Rotate() {
        //one day = 24 real mins = 1440 secs
        float value = (time*0.25f) - 90f; //-90 to 270 
        //transform.localEulerAngles.x + ((rotationSpeed / 10) * Time.deltaTime)
        // transform.localEulerAngles = new Vector3(Time.time * rotationSpeed, 20, 0);
        transform.localEulerAngles = new Vector3(value, 20, 0);
    }

    //realsec/0.666 - 320
    // -90 midnight, 12am, 0 realsecs
    // 0 sunrise, 6am, 360
    // 90 midday, 12pm, 720
    // 180 sunset, 6pm, 1080 
    // 270 midnight, 12am, 1440 realsecs
}
