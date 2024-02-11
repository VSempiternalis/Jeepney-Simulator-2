using UnityEngine;
using System;

public class Keybinds : MonoBehaviour {
    public static Keybinds current;

    //[EVENTS]
    public event Action onKeyChangeEvent;

    private void Awake() {
        current = this;
    }

    public void KeyChanged() {
        if(onKeyChangeEvent != null) onKeyChangeEvent();
    }
}
