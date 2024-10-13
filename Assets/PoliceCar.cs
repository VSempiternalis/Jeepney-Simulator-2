using System.Collections;
using UnityEngine;

public class PoliceCar : MonoBehaviour {
    private Transform target;
    private aiCarController carCon;
    private aiCarInput aci;
    private CrimeManager cm;

    public bool isChasingTarget;
    [SerializeField] private int arrestRange = 10;
    private Coroutine chaseCoroutine;

    [SerializeField] private GameObject lights;
    [SerializeField] private AudioSource audioSource;

    private void Start() {
        carCon = GetComponent<aiCarController>();
        aci = GetComponent<aiCarInput>();
        target = PlayerDriveInput.current.transform;
        cm = CrimeManager.current;
    }

    private void FixedUpdate() {
        if(isChasingTarget) {
            //check if close to target, start arresting
            if(Vector3.Distance(transform.position, target.position) < arrestRange) {
                cm.arrestProgress ++;
            }
        }

        //if can see player, no aggro loss
    }

    public void SetIsChasing(bool newIsChasing) {
        if(!gameObject.activeSelf) return;
        
        // Stop the previous coroutine if it's running
        if(chaseCoroutine != null) {
            StopCoroutine(chaseCoroutine);
        }

        chaseCoroutine = StartCoroutine(WaitAndChase(newIsChasing));
    }

    private IEnumerator WaitAndChase(bool newIsChasing) {
        yield return new WaitForSeconds(1f);

        isChasingTarget = newIsChasing;

        //carCon
        carCon.isChasingTarget = isChasingTarget;
        carCon.target = target;

        //ai car input
        aci.isChasingTarget = isChasingTarget;

        //Audio
        if(isChasingTarget && !audioSource.isPlaying) audioSource.Play();
        else if(!isChasingTarget)audioSource.Stop();

        //Lights
        lights.SetActive(isChasingTarget);
    }
}
