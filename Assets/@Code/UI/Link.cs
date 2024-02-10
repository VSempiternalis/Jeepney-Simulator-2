using System;
using UnityEngine;

public class Link : MonoBehaviour {
    [SerializeField] private string url;

    private void Start() {
        
    }

    private void Update() {
        
    }

    public void OpenLink() {
        Application.OpenURL(url);
    }
}
