using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class RouteSelector : MonoBehaviour {
    public static RouteSelector current;
    private AudioManager am;

    public List<string> destinations; //destinations the player is taking
    public List<string> allDestinations; //main, basic destinations based on office/beginner
    public List<string> lockedDestinations; //destinations that cannot be toggled for this shift
    
    public List<List<string>> landmarkAreas; //office, house 1, 2, and 3

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

    //auto toggle
    public bool isAutoUnlockDests = true;
    public bool isAutoOffDests = false;

    private void Awake() {
        current = this;
    }

    private void Start() {
        am = AudioManager.current;
        // print("Audio Manager exists: " + (am != null? "true" : "false"));

        landmarkAreas = new List<List<string>>();
        // print("all destinations: " + allDestinations);
        AddNewLandmarkArea(allDestinations);
    }

    private void Update() {
        // if(Input.GetKeyDown(KeyCode.Alpha9)) ForceDestUnlock("Tondoo");
    }

    public void AddNewLandmarkArea(List<string> newLandmarksArea) {
        // print("Adding new landmark area");
        landmarkAreas.Add(newLandmarksArea);
        // print("landmarkAreas count: " + landmarkAreas.Count);

        foreach(string landmark in newLandmarksArea) {
            // print("adding landmark: " + landmark);
        }
    }

    public void ArrivedAt(string dest) {
        if(isAutoUnlockDests) ForceDestUnlock(dest);
        if(isAutoOffDests) ForceDestOff(dest);
    }

    public void ToggleAutoUnlockDests(bool newVal) {
        isAutoUnlockDests = newVal;
    }

    public void ToggleAutoOffDests(bool newVal) {
        isAutoOffDests = newVal;
    }

    public void ForceDestUnlock(string dest) {
        // print("FORCE UNLOCK: " + dest);
        // if(!isAutoToggleDests) return;

        if(lockedDestinations.Contains(dest)) {
            lockedDestinations.Remove(dest);
            ColorDest(dest, officeWhite, uiWhite);
        }

        am.PlayUI(1);
    }

    public void ForceDestOff(string dest) {
        print("FORCE OFF: " + dest);

        if(destinations.Contains(dest)) {
            destinations.Remove(dest);
            ColorDest(dest, officeRed, uiRed);
        }
    }

    public void ToggleDestination(string destination) {
        if(lockedDestinations.Contains(destination)) {
            am.PlayUI(7);
            return;
        }

        if(destinations.Contains(destination)) { //REMOVE DESTINATION
            destinations.Remove(destination);
            ColorDest(destination, officeRed, uiRed);

        } else { //ADD DESTINATION
            destinations.Add(destination);
            ColorDest(destination, officeWhite, uiWhite);
        }

        am.PlayUI(1);
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
        //get landmark area
        List<string> currentLandmarkArea;
        int randInt = Random.Range(0, landmarkAreas.Count);
        // print("randInt: " + randInt);
        // print("lock. landmark areas count: " + landmarkAreas.Count);
        currentLandmarkArea = landmarkAreas[randInt];

        //fill in with new locked landmarks
        while(lockedDestinations.Count < num) {
            int randInt2 = Random.Range(0, currentLandmarkArea.Count);
            // print("randInt2: " + randInt2);
            string newLockedDest = currentLandmarkArea[randInt2];
            // print("new locked dest: " + newLockedDest);

            if(!lockedDestinations.Contains(newLockedDest)) {
                lockedDestinations.Add(newLockedDest);
                destinations.Add(newLockedDest);

                ColorDest(newLockedDest, officeGreen, uiGreen);
            }
        }
    }
}
