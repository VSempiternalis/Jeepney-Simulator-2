using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class RouteSelector : MonoBehaviour {
    public static RouteSelector current;

    public List<string> destinations; //destinations the player is taking
    public List<string> allDestinations;
    public List<string> lockedDestinations; //destinations that cannot be toggled for this shift

    //office map
    [SerializeField] private List<TMP_Text> officeTexts;
    [SerializeField] private Color officeWhite; //ON
    [SerializeField] private Color officeRed; //OFF
    [SerializeField] private Color officeGreen; //LOCKED

    //ui map
    [SerializeField] private List<TMP_Text> uiTexts;
    [SerializeField] private Color uiWhite;
    [SerializeField] private Color uiRed;
    [SerializeField] private Color uiGreen;

    private void Awake() {
        current = this;
    }

    private void Start() {
        
    }

    private void Update() {
        
    }

    public void ToggleDestination(string destination) {
        if(lockedDestinations.Contains(destination)) {
            AudioManager.current.PlayUI(7);
            return;
        }

        if(destinations.Contains(destination)) { //REMOVE DESTINATION
            destinations.Remove(destination);
            ColorDest(destination, officeRed, uiRed);

        } else { //ADD DESTINATION
            destinations.Add(destination);
            ColorDest(destination, officeWhite, uiWhite);
        }

        AudioManager.current.PlayUI(1);
    }

    private void ColorDest(string dest, Color officeColor, Color uiColor) {
        foreach(TMP_Text officeText in officeTexts) {
            if(officeText.name.Contains(dest)) officeText.color = officeColor;
        }

        foreach(TMP_Text uiText in uiTexts) {
            if(uiText.name.Contains(dest)) uiText.color = uiColor;
        }
    }

    public void NewShift(int destsToLock) {
        AllDestsOff();
        // AllDestsOff(true);
        LockRandomDests(destsToLock);
    }

    private void AllDestsOff() {
        destinations.Clear();
        lockedDestinations.Clear();

        foreach(TMP_Text officeText in officeTexts) {
            officeText.color = officeRed;
        }

        foreach(TMP_Text uiText in uiTexts) {
            uiText.color = uiRed;
        }
    }

    private void LockRandomDests(int num) {
        while(lockedDestinations.Count < num) {
            int randInt = Random.Range(0, allDestinations.Count);
            string newLockedDest = allDestinations[randInt];

            if(!lockedDestinations.Contains(newLockedDest)) {
                lockedDestinations.Add(newLockedDest);
                destinations.Add(newLockedDest);

                ColorDest(newLockedDest, officeGreen, uiGreen);
            }
        }
    }
}
