using UnityEngine;
using System.Collections.Generic;

public class NotificationManager : MonoBehaviour {
    public static NotificationManager current;

    [SerializeField] private GameObject notifPF;
    [SerializeField] private Transform notifs;

    [SerializeField] private List<string> dupesAllowed;

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
        if(!Settings.current.isTutorialPanelsOn) return;
        // print("notifs count: " + notifs.childCount);

        if(notifs.childCount > 3) {
            notifs.GetChild(0).GetComponent<Notification>().DestroySelf();
        }

        //remove dupes
        foreach(Transform notif in notifs) {
            string notifHeader = notif.GetComponent<Notification>().headerText.text;
            if(notifHeader == header && !dupesAllowed.Contains(notifHeader)) return;
        }

        AudioManager.current.PlayUI(13);
        // print("New notif: " + header);
        GameObject newNotif = Instantiate(notifPF, notifs);
        newNotif.GetComponent<Notification>().Setup(header, desc);
        // newNotif.GetComponent<uiAnimGroup>().In();
    }
}
