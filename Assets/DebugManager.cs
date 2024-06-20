using UnityEngine;

public class DebugManager : MonoBehaviour {
    private bool isDebugOn;
    [SerializeField] private GameObject debugOnText;

    private Transform player;
    [SerializeField] private Transform poi1; //position of interest
    [SerializeField] private Transform poi2;
    [SerializeField] private Transform poi3;

    private void Start() {
        player = PlayerDriveInput.current.transform;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.BackQuote) && Input.GetKey(KeyCode.LeftControl)) {
            isDebugOn = !isDebugOn;
            debugOnText.SetActive(isDebugOn);
        }

        if(isDebugOn) {
            //[1] Teleport to POI
            if(Input.GetKeyDown(KeyCode.Alpha1)) {
                player.position = poi1.position;
            }
            //[2] Teleport to POI
            if(Input.GetKeyDown(KeyCode.Alpha2)) {
                player.position = poi2.position;
            }
            //[3] Teleport to POI
            if(Input.GetKeyDown(KeyCode.Alpha3)) {
                player.position = poi3.position;
            }
            //[2] Teleport to POI
            else if(Input.GetKeyDown(KeyCode.Alpha0)) {
                BoundaryManager.current.AddToDeposit(10000);
            }
        }
    }
}
