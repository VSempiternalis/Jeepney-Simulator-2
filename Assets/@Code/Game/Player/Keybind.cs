using UnityEngine;
using TMPro;
using System;

public class Keybind : MonoBehaviour {
    [SerializeField] private string keyName;
    [SerializeField] private string defaultValue;
    [SerializeField] private TextMeshProUGUI buttonText;

    [SerializeField] private string query;

    private void Start() {
        buttonText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        buttonText.text = PlayerPrefs.GetString(keyName, defaultValue);
    }

    private void Update() {
        if(buttonText.text == query) {
            foreach(KeyCode keycode in Enum.GetValues(typeof(KeyCode))) {
                if(Input.GetKey(keycode)) {
                    string key = keycode.ToString();
                    buttonText.text = key;
                    PlayerPrefs.SetString(keyName, key);
                    Keybinds.current.KeyChanged();
                }
            }
        }
    }

    public void SetKey() {
        buttonText.text = query;
    }
}