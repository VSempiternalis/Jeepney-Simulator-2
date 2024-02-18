using UnityEngine;

public class SeatHandler : MonoBehaviour, IInteractable, ITooltipable {
    [SerializeField] private string header;
    [SerializeField] private string text;

    [SerializeField] private Vector3 sitLocalPos;
    [SerializeField] private Vector3 exitLocalPos;
    [SerializeField] private Transform exitPoint;

    [SerializeField] private AudioSource audioSource;

    private void Start() {
        
    }

    private void Update() {
        
    }

    public void Interact(GameObject interactor) {
        Transform player = interactor.transform;
        if(transform.childCount > 1) {
            print("exiting");
            //EXIT
            // player.localPosition = exitLocalPos;
            player.position = exitPoint.position;
            // player.rotation = transform.rotation;
            // player.SetParent(GameObject.Find("WORLD").transform);
            player.SetParent(null);

            // player.GetComponent<Rigidbody>().isKinematic = false;
            // player.GetComponent<CapsuleCollider>().isTrigger = false;
            player.GetComponent<PlayerDriveInput>().SetIsSitting(false, false);
        } else {
            player.GetComponent<PlayerDriveInput>().SetIsSitting(true, false);

            print("entering");
            //ENTER
            player.SetParent(transform);
            //player.localPosition = Vector3.zero;
            player.localPosition = sitLocalPos;
            // player.rotation = transform.rotation;
        }

        if(audioSource) audioSource.Play();
    }

    public string GetHeader() {
        return header;
    }

    public string GetText() {
        return text;
    }
}
