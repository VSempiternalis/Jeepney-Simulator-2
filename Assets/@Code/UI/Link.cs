using System;
using UnityEngine;

public class Link : MonoBehaviour {
    public string url;

    private void Start() {
        
    }

    private void Update() {
        
    }

    public void OpenLink() {
        Application.OpenURL(url);
    }

    public void OpenDirectory() {
        print("opening directory: " + url);
        Application.OpenURL(@"file:" + url);
    }
}
