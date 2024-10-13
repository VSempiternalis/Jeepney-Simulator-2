using UnityEngine;
using TMPro;

public class TickerIBarelyKnowHer : MonoBehaviour {
    private TMP_Text text;

    private float timer = 0f;
    public float tickerSpeed = 0.05f;  // Time delay between each character shift

    private void Start() {
        text = GetComponent<TMP_Text>();
    }

    private void Update() {
        timer += Time.deltaTime;

        if (timer >= tickerSpeed) {
            string newText = text.text.Substring(1) + text.text[0];
            text.text = newText;

            timer = 0f;
        }
    }
}
