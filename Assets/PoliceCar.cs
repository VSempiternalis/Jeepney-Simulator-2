using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PoliceCar : MonoBehaviour {
    [SerializeField] private Transform target;
    private aiCarController carCon;
    private aiCarInput aci;
    private CrimeManager cm;
    private NavMeshAgent nma;
    private Rigidbody rb;

    public bool isChasingTarget;
    [SerializeField] private int arrestRange = 10;
    private Coroutine chaseCoroutine;

    [SerializeField] private int visionRange;
    [SerializeField] private float nmaAcc;
    [SerializeField] private LayerMask nodeLayer;

    [SerializeField] private GameObject lights;
    private AudioSource audioSource;

    //COLLISION
    [SerializeField] private LayerMask collisionLayer;

    private void Start() {
        carCon = GetComponent<aiCarController>();
        aci = GetComponent<aiCarInput>();
        // target = PlayerDriveInput.current.transform;
        cm = CrimeManager.current;
        nma = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        if(!cm.policeCars.Contains(this)) cm.policeCars.Add(this);
        visionRange = cm.copCarRange;
    }

    private void FixedUpdate() {
        if(isChasingTarget) {
            //check if close to target, start arresting
            if(Vector3.Distance(transform.position, target.position) < arrestRange) {
                cm.arrestProgress ++;
            }

            //if can see player, no aggro loss
            if(Vector3.Distance(transform.position, target.position) < visionRange) {
                if(cm.isAggroLoss != false) cm.isAggroLoss = false;
            }

            //chase target
            // nma.destination = target.position;
            if(nma != null) {
                nma.SetDestination(target.position);
                nma.acceleration = nmaAcc*carCon.healthFactor;
            }
        }
    }

    public void SetIsChasing(bool newIsChasing) {
        if(!gameObject.activeSelf) return;

        isChasingTarget = newIsChasing;
        
        // Stop the previous coroutine if it's running
        if(chaseCoroutine != null) {
            StopCoroutine(chaseCoroutine);
        }

        // rb.isKinematic = newIsChasing;
        chaseCoroutine = StartCoroutine(WaitAndChase(newIsChasing));

        cm.NewCopCarChasing(this, newIsChasing);
    }

    private IEnumerator WaitAndChase(bool newIsChasing) {
        yield return new WaitForSeconds(2f);

        // isChasingTarget = newIsChasing;

        //carCon
        carCon.enabled = !isChasingTarget;
        // carCon.isChasingTarget = isChasingTarget;
        // carCon.target = target;

        //ai car input
        aci.enabled = !isChasingTarget;
        // aci.isChasingTarget = isChasingTarget;

        //nma
        if(nma != null) {
            nma.enabled = isChasingTarget;
            nma.isStopped = !isChasingTarget;
        }
        rb.isKinematic = isChasingTarget;

        //Audio
        if(isChasingTarget && !audioSource.isPlaying) audioSource.Play();
        else if(!isChasingTarget)audioSource.Stop();

        //Lights
        lights.SetActive(isChasingTarget);

        //if not chasing anymore, find nearby node and set it as target
        float nodeFindRange = 100f;
        float closestNodeDistance = 100f;
        Transform closestNode = null;

        Collider[] colliders = Physics.OverlapSphere(transform.position, nodeFindRange, nodeLayer);
        // print("COLLIDERS LENGTH: " + colliders.Length);
        if(colliders.Length > 0) {
            foreach(Collider col in colliders) {
                print("COL: " + col.gameObject.name);
                if(col.gameObject.layer == LayerMask.NameToLayer("Node")) {
                    print("NODE: " + col.gameObject.name);
                    float distance = Vector3.Distance(transform.position, col.transform.position);
                    if(distance < closestNodeDistance) {
                        closestNodeDistance = distance;
                        closestNode = col.transform;
                        break;
                    }
                }
            }
        }

        if(closestNode != null) {
            carCon.currentNode = closestNode.gameObject.GetComponent<NodeHandler>();
            carCon.nextNode = carCon.currentNode.GetRandomNode();
        }
    }

    // private void OnCollisionEnter(Collision other) {
    //     print("police car collision: " + other.gameObject.name);
    //     if((collisionLayer.value & (1 << other.gameObject.layer)) != 0) {
    //         //COLLIDE WITH PLAYER CAR
    //         if(other.gameObject.GetComponent<CarController>()) {
    //             print("has car controller");
    //         }
    //     }
    // }

    private void OnCollisionStay(Collision other) {
        if((collisionLayer.value & (1 << other.gameObject.layer)) != 0) {
            //COLLIDE WITH PLAYER CAR
            if(other.gameObject.GetComponent<CarController>()) {
                print("stopped");
                rb.isKinematic = false;
                nma.isStopped = true;
            }
        }
    }

    private void OnCollisionExit(Collision other) {
        if((collisionLayer.value & (1 << other.gameObject.layer)) != 0) {
            //COLLIDE WITH PLAYER CAR
            if(other.gameObject.GetComponent<CarController>()) {
                print("go");
                rb.isKinematic = true;
                nma.isStopped = false;
            }
        }
    }
}