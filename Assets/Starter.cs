using UnityEngine;
using System.Collections.Generic;

public class Starter : MonoBehaviour {
    [SerializeField] private List<GameObject> toActivate;
    [SerializeField] private List<GameObject> toDeactivate;

    private void Start() {
        foreach(GameObject go in toActivate) {
            go.SetActive(true);
        }

        foreach(GameObject go in toDeactivate) {
            go.SetActive(false);
        }
    }

    private void Update() {
        
    }
}
