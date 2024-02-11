using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ChangeHandler : MonoBehaviour, ITooltipable, IScrollable {
    private List<PersonHandler> changees = new List<PersonHandler>();
    public PersonHandler currentChangee;
    private LineRenderer lr;
    [SerializeField] private TMP_Text changeText;

    private void Start() {
        lr = GetComponent<LineRenderer>();

        // if(transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>())
        //     changeText = transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
    }

    private void Update() {
        if(currentChangee == null) {
            // lr.SetPosition(0,transform.position);
            // lr.SetPosition(1,transform.position);
        } else {
            foreach(PersonHandler changee in changees) {
                // changee.GetComponent<Outline>().enabled = false;
            }
            // currentChangee.GetComponent<Outline>().enabled = true;
            // lr.SetPosition(0, transform.position);
            // Vector3 setpos = new Vector3(currentChangee.transform.parent.position.x, currentChangee.transform.parent.position.y + 0.5f, currentChangee.transform.parent.position.z);
            // lr.SetPosition(1, setpos);
        }
        // lr.SetPosition(1, currentChangee.transform.parent.position);
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
        print("ChangeHandler Scrolling");
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
            changeText.text = GetText();
            // if(changees.Count > 1) {
            //     changeText.text += GetText();
            // } else {
            // }
        } 
    }

    public void UpdateText() {
        if(changees.Count == 0) {
            changeText.text = "CHANGE";
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

    public string GetText() {
        string text = "";

        if(changees.Count == 0) {
            text = "Place change here";
        } else {
            if(changees.Count > 1) {
                text = currentChangee.to + "\nP" + currentChangee.change + "\nv";
            } else {
                text = currentChangee.to + "\nP" + currentChangee.change;
            }
        } 

        return text;
    }
}