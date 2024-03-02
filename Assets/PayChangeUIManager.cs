using UnityEngine;
using TMPro;

public class PayChangeUIManager : MonoBehaviour {
    public static PayChangeUIManager current;
    [SerializeField] private uiAnimGroup payChangePanel;
    [SerializeField] private TMP_Text changePointText;
    [SerializeField] private TMP_Text payPointText;
    private string changeText;
    private int payValue;

    private void Awake() {
        current = this;
    }

    private void Start() {
        
    }

    private void Update() {
        
    }

    public void SetChangeText(string newText) {
        print("SetChangeText: " + newText);
        changePointText.text = newText;
        changeText = newText;

        Check();
    }

    public void SetStorageValue(int newVal) {
        if(newVal == 0) payPointText.text = "PAY";
        else payPointText.text = "P" + newVal;
        payValue = newVal;

        Check();
    }

    private void Check() {
        // if(changeText == "CHANGE" && payValue == 0 && payChangePanel.isIn) payChangePanel.Out();
        // else if(!payChangePanel.isIn) payChangePanel.In();

        if(!payChangePanel.isIn && (changeText != "CHANGE" || payValue > 0)) payChangePanel.In();
        // if((payValue > 0) && !payChangePanel.isIn) payChangePanel.In();
        else if(payChangePanel.isIn && (changeText == "CHANGE" && payValue <= 0)) payChangePanel.Out();
    }
}
