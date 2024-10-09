using UnityEngine;

public class PoliceCar : MonoBehaviour {
    private Transform target;
    private aiCarController carCon;
    private aiCarInput aci;

    public bool isChasingTarget;

    [SerializeField] private GameObject lights;
    [SerializeField] private AudioSource audioSource;

    private void Start() {
        carCon = GetComponent<aiCarController>();
        aci = GetComponent<aiCarInput>();
        target = PlayerDriveInput.current.transform;
    }

    private void Update() {
        if(isChasingTarget) {
            //check if close to target, start arresting
        }
    }

    public void SetIsChasing(bool newIsChasing) {
        isChasingTarget = newIsChasing;

        //carCon
        carCon.isChasingTarget = isChasingTarget;
        carCon.target = target;

        //carCon
        aci.isChasingTarget = isChasingTarget;

        //Audio
        if(isChasingTarget) if(!audioSource.isPlaying) audioSource.Play();
        else audioSource.Stop();

        //Lights
        lights.SetActive(isChasingTarget);
    }
}
