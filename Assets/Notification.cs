using UnityEngine;
using System.Collections;
using TMPro;

public class Notification : MonoBehaviour {
    [SerializeField] private TMP_Text headerText;
    [SerializeField] private TMP_Text descText;
    [SerializeField] private GameObject descImg;
    [SerializeField] private float duration;

    private void Start() {
        // gameObject.SetActive(true);
    }

    private void Update() {
        if(transform.position.x < -120) {
            descImg.SetActive(false);
        } else if(transform.position.x < -110) {
            descImg.SetActive(true);
        } else if(transform.position.x == 125) {
            descImg.SetActive(false);
            descImg.SetActive(true);
        }
        // if(!gameObject.activeSelf) gameObject.SetActive(true);
    }

    public void Setup(string header, string desc) {
        print("Setup: " + header);
        gameObject.SetActive(true);
        // descImg.SetActive(false);
        // descImg.SetActive(true);

        transform.position = new Vector3(-125, 0, 0);
        LeanTween.moveLocalX(gameObject, 125, 0.5f).setEaseOutQuart();

        headerText.text = header;
        descText.text = desc;

        StartCoroutine(TimerCoroutine());
    }

    //Runs every second
    private IEnumerator TimerCoroutine() {
        while(true) {
            yield return new WaitForSeconds(duration);
            Destroy(gameObject);
        }
    }
}
