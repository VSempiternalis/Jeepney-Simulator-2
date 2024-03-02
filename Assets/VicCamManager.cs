using UnityEngine;

public class VicCamManager : MonoBehaviour {
    [SerializeField] private Vector3 camPos1;
    [SerializeField] private Vector3 camPos2;
    [SerializeField] private Vector3 camPos3;
    [SerializeField] private Vector3 camPos4;
    [SerializeField] private Vector3 camPos5;

    [SerializeField] private float transitionTime;
    [SerializeField] private PlayerDriveInput pdi;

    [Space(10)]
    [Header("KEYBINDS")]
    private KeyCode Key_CamPos1;
    private KeyCode Key_CamPos2;
    private KeyCode Key_CamPos3;
    private KeyCode Key_CamPos4;
    private KeyCode Key_CamPos5;

    private void Start() {
        OnKeyChangeEvent();
        pdi = GetComponent<PlayerDriveInput>();
    }

    private void Update() {
        if(pdi.isDriving) GetInput();
    }

    private void GetInput() {
        Vector3 newPos;

        if(Input.GetKeyDown(Key_CamPos1)) newPos = camPos1;
        else if(Input.GetKeyDown(Key_CamPos2)) newPos = camPos2;
        else if(Input.GetKeyDown(Key_CamPos3)) newPos = camPos3;
        else if(Input.GetKeyDown(Key_CamPos4)) newPos = camPos4;
        else if(Input.GetKeyDown(Key_CamPos5)) newPos = camPos5;
        else return;

        LeanTween.moveLocal(gameObject, newPos, transitionTime).setEaseOutQuint();
    }

    private void OnKeyChangeEvent() {
        //Set keys
        Key_CamPos1 = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_CamPos1", "F1"));
        Key_CamPos2 = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_CamPos2", "F2"));
        Key_CamPos3 = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_CamPos3", "F3"));
        Key_CamPos4 = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_CamPos4", "F4"));
        Key_CamPos5 = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_CamPos5", "F5"));
    }
}
