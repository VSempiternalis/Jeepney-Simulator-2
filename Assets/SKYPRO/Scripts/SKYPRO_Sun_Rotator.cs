using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKYPRO_Sun_Rotator : MonoBehaviour {
    public static SKYPRO_Sun_Rotator current;
    [SerializeField] private float rotationSpeed = 0.5f;
    public float time;

    private void Awake() {
        current = this;
    }

    private void Update() {
        // time += Time.deltaTime;
    }

    void FixedUpdate() {
        Rotate();
    }

    void Rotate() {
        //transform.localEulerAngles.x + ((rotationSpeed / 10) * Time.deltaTime)
        // transform.localEulerAngles = new Vector3(Time.time * rotationSpeed, 20, 0);
        transform.localEulerAngles = new Vector3(time * rotationSpeed, 20, 0);
    }
}
