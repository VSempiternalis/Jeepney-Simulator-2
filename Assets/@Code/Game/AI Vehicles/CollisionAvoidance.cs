using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionAvoidance : MonoBehaviour {
    // [SerializeField] private aiCarInput ai;
    public int triggerCount;
    private int emptyCheckTime = 5;
    [SerializeField] private List<GameObject> triggers = new List<GameObject>();
    [SerializeField] private int checkCount;

    //STATE
    // public string state;

    // private float nextSecUpdate;

    private void Start() {
        // nextSecUpdate = Time.time + 1;
        // StartCoroutine(EmptyCheck());
        triggerCount = 0;
        InvokeRepeating("EmptyCheck", 0f, emptyCheckTime);
    }

    // private void FixedUpdate() {
    //     // check every second if clear
    //     if(Time.time >= (nextSecUpdate)) {
    //         nextSecUpdate ++;

    //         if(triggerCount > 0) EmptyCheck();
    //     }
    // }

    private void EmptyCheck() {
        checkCount ++;
        triggerCount = 0;
        gameObject.SetActive(false);
        gameObject.SetActive(true);
        // LeanTween.moveLocalY(gameObject, 2f, 1f).setEase(LeanTweenType.punch);
        // if(triggerCount > 0) {
        //     checkCount ++;
        //     triggerCount = 0;
        //     LeanTween.moveLocalY(gameObject, 2f, 1f).setEase(LeanTweenType.punch);
        // }
    }

    // private IEnumerator EmptyCheck() {
    //     while(true) {
    //         yield return new WaitForSeconds(emptyCheckTime);
    //     }
    // }

    // private void EmptyCheck() {
    //     state = "Exit";
    // }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == 6 || other.gameObject.layer == 13 || other.gameObject.layer == 17 || other.gameObject.layer == 21) {
            triggerCount ++;//state = "Enter"; //Vehicles, Person, Traffic Stop layers
            // triggers.Add(other.name);
            // EmptyCheck();
        }
    }

    // private void OnTriggerStay(Collider other) {
    //     if(other.gameObject.layer == 6 || other.gameObject.layer == 13 || other.gameObject.layer == 17 || other.gameObject.layer == 21) state = "Stay"; //Vehicles, Person, Traffic Stop layers
    // }

    private void OnTriggerExit(Collider other) {
        if((other.gameObject.layer == 6 || other.gameObject.layer == 13 || other.gameObject.layer == 17 || other.gameObject.layer == 21) && triggerCount > 0) {
            triggerCount --;//state = "Exit"; //Vehicles, Person, Traffic Stop layers
        }
    }
}