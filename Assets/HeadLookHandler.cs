using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HeadLookHandler : MonoBehaviour {
    [SerializeField] private Transform target;
    [SerializeField] private float currentDist;
    [SerializeField] private int lookDist;
    [SerializeField] private int lookLimit;
    [SerializeField] private MultiAimConstraint mac;
    [SerializeField] private float smoothness;

    private void Update() {
        currentDist = Vector3.Distance(transform.position, target.position);
        float targetWeight = 0;

        //limit headlook to 30
        if(currentDist > lookLimit) {
            targetWeight = 0;
        } else if(currentDist < lookDist) {
            targetWeight = 1;
        } else {
            print(lookDist/currentDist);
            targetWeight = lookDist/currentDist;
        }

        // Smoothly adjust the weight towards the target weight
        mac.weight = Mathf.Lerp(mac.weight, targetWeight, Time.deltaTime * smoothness);
    }
}
