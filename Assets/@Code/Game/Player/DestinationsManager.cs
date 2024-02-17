using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DestinationsManager : MonoBehaviour {
    public static DestinationsManager current;

    public List<string> destinations; //Dests the player accepted
    public List<Toggle> lockedDestinationButtons; //Dests the player accepted
    public List<Toggle> destinationButtons; //Dests the player accepted
    [SerializeField] private GameObject uiIsTakingPassengers;
    [SerializeField] private GameObject uiNotTakingPassengers;

    private void Awake() {
        current = this;
    }

    private void Start() {
        
    }

    private void Update() {
        
    }

    public void ToggleDestination(string destination) {
        // if(lockedDestinations.Contains(destination)) return;

        if(destinations.Contains(destination)) destinations.Remove(destination);
        else destinations.Add(destination);
    }

    public void ResetDestinations(int destsToLock) {
        UnlockAllDests();
        ToggleAllDestinations(true);
        LockRandomDests(destsToLock);
    }

    private void UnlockAllDests() {
        foreach(Toggle destButton in destinationButtons) {
            destButton.interactable = true;
        }
    }

    private void LockRandomDests(int num) {
        lockedDestinationButtons.Clear();
        while(lockedDestinationButtons.Count < num) {
            int randInt = Random.Range(0, destinationButtons.Count);
            Toggle newLock = destinationButtons[randInt];

            if(!lockedDestinationButtons.Contains(newLock)) {
                lockedDestinationButtons.Add(newLock);
                newLock.interactable = false;
            }
        }
    }

    public void ToggleAllDestinations(bool isOn) {
        foreach(Toggle destButton in destinationButtons) {
            if(destButton.interactable) destButton.isOn = isOn;
        }
    }
}
