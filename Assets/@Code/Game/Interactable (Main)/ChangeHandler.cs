using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ChangeHandler : MonoBehaviour, ITooltipable, IScrollable {
    [SerializeField] private string header;
    [SerializeField] private string controls;

    public List<PersonHandler> changees = new List<PersonHandler>();
    public PersonHandler currentChangee;
    private LineRenderer lr;
    [SerializeField] private TMP_Text changeText;

    private void Start() {
        lr = GetComponent<LineRenderer>();

        // if(transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>())
        //     changeText = transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
    }

    private void Update() {
        
    }

    public void AddChangee(PersonHandler changee) {
        changees.Add(changee);
        UpdateText();
    }

    public void ClearChangees() {
        currentChangee = null;
        List<PersonHandler> clearList = new List<PersonHandler>();
        foreach(PersonHandler changee in changees) {
            clearList.Add(changee);
        }
        foreach(PersonHandler changee in clearList) {
            RemoveChangee(changee);
        }
    }

    public void RemoveChangee(PersonHandler changee) {
        // changee.GetComponent<Outline>().enabled = false;
        changees.Remove(changee);
        if(changees.Count == 0) currentChangee = null;
        UpdateText();
    }

    public void Scroll(float direction) {
        // print("ChangeHandler Scrolling");
        if(changees.Count <= 1) return;

        int currentChangeeIndex = changees.IndexOf(currentChangee);

        if(direction > 0) {
            if(currentChangeeIndex == changees.Count - 1) {
                SetChangee(0);
            } else {
                SetChangee(currentChangeeIndex + 1);
            }
        } else {
            if(currentChangeeIndex == 0) {
                SetChangee(changees.Count - 1);
            } else {
                SetChangee(currentChangeeIndex - 1);
            }
        }
    }

    private void SetChangee(int index) {
        currentChangee = changees[index];
        // changeText.text = "";

        if(changees.Count == 0) {
            changeText.text = "Place change here";
        } else {
            changeText.text = GetDesc();
            // if(changees.Count > 1) {
            //     changeText.text += GetText();
            // } else {
            // }
        }

        PayChangeUIManager.current.SetChangeText(changeText.text);
    }

    public void UpdateText() {
        if(changees.Count == 0) {
            changeText.text = "CHANGE";
            PayChangeUIManager.current.SetChangeText(changeText.text);
        } else if(changees.Contains(currentChangee)) {
            SetChangee(changees.IndexOf(currentChangee));
        } else {
            SetChangee(0);
        }
    }

    public string GetHeader() {
        string header = "CHANGE";

        return header;
    }

    public string GetControls() {
        return "[R Mouse] Place item\n[Mid Mouse] Take all items";
    }

    public string GetDesc() {
        string text = "";

        if(changees.Count == 0) {
            text = "Place change here";
        } else {
            if(changees.Count > 1) {
                text = currentChangee.landmarkDest + "\nP" + currentChangee.change + "\nv";
            } else {
                text = currentChangee.landmarkDest + "\nP" + currentChangee.change;
            }
        } 

        return text;
    }
}