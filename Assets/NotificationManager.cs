using UnityEngine;
using UnityEngine.Playables;

public class NotificationManager : MonoBehaviour {
    public static NotificationManager current;

    [SerializeField] private GameObject notifPF;
    [SerializeField] private Transform notifs;

    private void Awake() {
        current = this;
    }

    private void Start() {
        
    }

    private void Update() {
        // if(Input.GetKeyDown(KeyCode.F11)) NewNotif("F11 PRESSED!", "The time is: " + Time.time);
    }

    public void NewNotif(string header, string desc) {
        if(notifs.childCount > 4) {
            notifs.GetChild(0).GetComponent<Notification>().DestroySelf();
        }

        AudioManager.current.PlayUI(13);
        // print("New notif: " + header);
        GameObject newNotif = Instantiate(notifPF, notifs);
        newNotif.GetComponent<Notification>().Setup(header, desc);
        // newNotif.GetComponent<uiAnimGroup>().In();
    }
}
