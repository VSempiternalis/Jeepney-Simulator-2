using UnityEngine;
using System.Collections.Generic;

public class NotificationManager : MonoBehaviour {
    public static NotificationManager current;

    [SerializeField] private GameObject notifPF;
    [SerializeField] private Transform notifs;

    [SerializeField] private List<string> dupesAllowed;

    [SerializeField] private Color headerWhite;
    [SerializeField] private Color headerGreen;
    [SerializeField] private Color headerYellow;
    [SerializeField] private Color headerRed;

    private void Awake() {
        current = this;
    }

    private void Start() {
        
    }

    private void Update() {
        // if(Input.GetKeyDown(KeyCode.F11)) NewNotif("F11 PRESSED!", "The time is: " + Time.time);
    }

    public void Clear() {
        // print("Clearing");
        foreach(Transform notif in notifs) {
            // Destroy(notif.GetComponent<GameObject>());
            notif.GetComponent<Notification>().DestroySelf();
        }
    }

    public void NewNotif(string header, string desc) {
        if(notifs.childCount > 3) {
            notifs.GetChild(0).GetComponent<Notification>().DestroySelf();
        }

        //remove dupes
        foreach(Transform notif in notifs) {
            string notifHeader = notif.GetComponent<Notification>().headerText.text;
            if(notifHeader == header && !dupesAllowed.Contains(notifHeader)) return;
        }

        AudioManager.current.PlayUI(13);
        GameObject newNotif = Instantiate(notifPF, notifs);
        newNotif.GetComponent<Notification>().Setup(header, desc);
    }

    public void NewNotifColor(string header, string desc, int colorInt) {
        if(notifs.childCount > 3) {
            notifs.GetChild(0).GetComponent<Notification>().DestroySelf();
        }

        //remove dupes
        foreach(Transform notif in notifs) {
            string notifHeader = notif.GetComponent<Notification>().headerText.text;
            if(notifHeader == header && !dupesAllowed.Contains(notifHeader)) return;
        }

        AudioManager.current.PlayUI(13);
        GameObject newNotif = Instantiate(notifPF, notifs);
        newNotif.GetComponent<Notification>().Setup(header, desc);

        //set color
        Color headerColor = headerWhite;

        if(colorInt == 1) headerColor = headerGreen;
        else if(colorInt == 2) headerColor = headerYellow;
        else if(colorInt == 3) headerColor = headerRed;

        newNotif.GetComponent<Notification>().SetHeaderColor(headerColor);
    }
}
