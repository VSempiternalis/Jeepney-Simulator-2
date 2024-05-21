using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class Notification : MonoBehaviour {
    public TMP_Text headerText;
    [SerializeField] private Image headerImg;
    [SerializeField] private TMP_Text descText;
    [SerializeField] private GameObject descImg;
    [SerializeField] private bool isPermanent;
    [SerializeField] private float duration;

    [SerializeField] private Color whiteHeaderText;

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

    public void SetHeaderColor(Color newColor) {
        headerImg.color = newColor;
        headerText.color = whiteHeaderText;
    }

    public void Setup(string header, string desc) {
        // print("Setup: " + header);
        // descImg.SetActive(false);
        // descImg.SetActive(true);
        
        gameObject.SetActive(true);
        // transform.parent.gameObject.SetActive(true);

        headerText.text = header;
        descText.text = desc;
        
        // gameObject.SetActive(false);
        // transform.parent.gameObject.SetActive(false);

        // transform.localPosition = new Vector3(-125, -435.3f, 0);
        transform.localPosition = new Vector3(-125, -435.3f, 0);
        LeanTween.moveLocalX(gameObject, 125, 0.5f).setEaseOutQuart();
        
        // gameObject.SetActive(true);
        // transform.parent.gameObject.SetActive(true);

        if(!isPermanent) StartCoroutine(TimerCoroutine());
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
