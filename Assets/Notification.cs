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
        if(transform.localPosition.x < -120) {
            descImg.SetActive(false);
        } else if(transform.localPosition.x < -110) {
            descImg.SetActive(true);
        } else
        if(transform.localPosition.x == 125) {
            descImg.SetActive(false);
            descImg.SetActive(true);
        }
        // if(!gameObject.activeSelf) gameObject.SetActive(true);
    }

    public void Setup(string header, string desc) {
        // print("Setup: " + header);
        // descImg.SetActive(false);
        // descImg.SetActive(true);

        transform.localPosition = new Vector3(-125, -435.3f, 0);
        LeanTween.moveLocalX(gameObject, 125, 0.5f).setEaseOutQuart();

        headerText.text = header;
        descText.text = desc;
        
        gameObject.SetActive(true);

        StartCoroutine(TimerCoroutine());
    }

    //Runs every second
    private IEnumerator TimerCoroutine() {
        while(true) {
            yield return new WaitForSeconds(duration);
            DestroySelf();
        }
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }
}
