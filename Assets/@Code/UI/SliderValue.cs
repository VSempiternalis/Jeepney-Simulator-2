using TMPro;
using UnityEngine;

public class SliderValue : MonoBehaviour {
    private TMP_Text valueText;

    private void Start() {
        valueText = GetComponent<TMP_Text>();
    }

    private void Update() {
        
    }

    public void SetValueInt(int newVal) {
        valueText.text = newVal + "";
    }

    public void SetValueFloat(float newVal) {
        valueText.text = newVal + "";
    }
}
