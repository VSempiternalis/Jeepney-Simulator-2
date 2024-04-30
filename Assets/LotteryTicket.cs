using TMPro;
using UnityEngine;

public class LotteryTicket : MonoBehaviour {
    public int tensDigit;
    public int secondsDigit;
    [SerializeField] private TMP_Text numberText;

    private void Start() {
        
    }

    private void Update() {
        
    }

    public string GetNum() {
        return numberText.text;
    }

    public void SetNumber(int tens, int seconds) {
        tensDigit = tens;
        secondsDigit = seconds;

        numberText.text = tensDigit + "" + secondsDigit;
    }
}
