using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PoliceCar : MonoBehaviour {
    public Transform target;
    private aiCarController carCon;
    private aiCarInput aci;
    private CrimeManager cm;
    private NavMeshAgent nma;
    private Rigidbody rb;
    private BoxCollider bc;
    private Transform player;
    private Transform playerJeep;

    public bool isChasingTarget;
    [SerializeField] private int arrestRange = 10;
    private Coroutine chaseCoroutine;

    [SerializeField] private int visionRange;
    [SerializeField] private float nmaAcc;
    [SerializeField] private LayerMask nodeLayer;

    [SerializeField] private GameObject lights;
    [SerializeField] private GameObject redLight;
    [SerializeField] private GameObject blueLight;
    private Coroutine lightCoroutine;
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
        // rb.isKinematic = true;
        audioSource = GetComponent<AudioSource>();
        bc = GetComponent<BoxCollider>();
        player = PlayerDriveInput.current.transform;
        playerJeep = PlayerDriveInput.current.carCon.transform;

        if(!cm.policeCars.Contains(this)) cm.policeCars.Add(this);
        visionRange = cm.copCarRange;
    }

    private void FixedUpdate() {
        if(carCon.enabled == false) {
            float distToTarget = Vector3.Distance(transform.position, target.position);
            float distToPlayer = Vector3.Distance(transform.position, player.position);
            float distToPlayerJeep = Vector3.Distance(transform.position, playerJeep.position);

            //stop when too close
            if(nma.isOnNavMesh && (distToTarget < 3 || distToPlayer < 5 || distToPlayerJeep < 5 )) {
                // print(name + " STOPPING POLICE CAR");
                nma.enabled = false;
            } else if(!nma.enabled && !(distToTarget < 3 || distToPlayer < 5 || distToPlayerJeep < 5 )) {
                nma.enabled = true;
            }

            // if(nma.isOnNavMesh) {
            //     // print(name + " STOPPING POLICE CAR");
            //     if(distToTarget < 3 || distToPlayer < 5 || distToPlayerJeep < 5 ) nma.enabled = false;
            //     else nma.enabled = true;
            // } 

            //check if close to target, start arresting
            // if(distToTarget < arrestRange) {
            // if(distToPlayer < arrestRange) {
            if(distToPlayerJeep < arrestRange) {
                // print("SHOULD BE ARRESTING");
                cm.arrestProgress ++;
            }

            //if can see player, no aggro loss
            if(distToTarget < visionRange) {
                if(cm.isAggroLoss != false) cm.isAggroLoss = false;
            }

            //chase target
            // nma.destination = target.position;
            if(nma.isOnNavMesh && nma != null) {
                nma.SetDestination(target.position);
                nma.acceleration = nmaAcc*carCon.healthFactor;
            }
        }
    }

    public void SetIsChasing(bool newIsChasing) {
        // print(name + " is chasing: " + newIsChasing);
        if(!gameObject.activeSelf) return;

        isChasingTarget = newIsChasing;
        
        // Stop the previous coroutine if it's running
        if(chaseCoroutine != null) {
            StopCoroutine(chaseCoroutine);
        }

        rb.isKinematic = newIsChasing;
        // bc.enabled = !newIsChasing;
        chaseCoroutine = StartCoroutine(WaitAndChase(newIsChasing));

        cm.NewCopCarChasing(this, newIsChasing);
    }

    private IEnumerator LightCoroutine() {
        while(isChasingTarget) {
            redLight.SetActive(true);
            blueLight.SetActive(false);
            yield return new WaitForSeconds(0.5f);

            redLight.SetActive(false);
            blueLight.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
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
        if(nma != null && nma.isOnNavMesh) {
            nma.enabled = isChasingTarget;
            nma.isStopped = !isChasingTarget;
        }
        rb.isKinematic = isChasingTarget;

        //Audio
        if(isChasingTarget && !audioSource.isPlaying) audioSource.Play();
        else if(!isChasingTarget)audioSource.Stop();

        //LIGHTS
        if(isChasingTarget) {
            lightCoroutine = StartCoroutine(LightCoroutine());
        } else if(!isChasingTarget) {
            if(lightCoroutine != null) StopCoroutine(lightCoroutine);
            redLight.SetActive(false);
            blueLight.SetActive(false);
        }

        //if not chasing anymore, find nearby node and set it as target
        // if(!isChasingTarget) {
        //     print("not chasing target");
        //     float nodeFindRange = 100f;
        //     float closestNodeDistance = 100f;
        //     Transform closestNode = null;

        //     Collider[] colliders = Physics.OverlapSphere(transform.position, nodeFindRange, nodeLayer);
        //     print("COLLIDERS LENGTH: " + colliders.Length);
        //     if(colliders.Length > 0) {
        //         print("colliders length: " + colliders.Length);
        //         foreach(Collider col in colliders) {
        //             print("COL: " + col.gameObject.name);
        //             if(col.gameObject.layer == LayerMask.NameToLayer("Node")) {
        //                 print("NODE: " + col.gameObject.name);
        //                 float distance = Vector3.Distance(transform.position, col.transform.position);
        //                 if(distance < closestNodeDistance) {
        //                     closestNodeDistance = distance;
        //                     closestNode = col.transform;
        //                     break;
        //                 }
        //             }
        //         }
        //     }

        //     if(closestNode != null) {
        //         carCon.currentNode = closestNode.gameObject.GetComponent<NodeHandler>();
        //         carCon.nextNode = carCon.currentNode.GetRandomNode();
        //     }
        // }
    }

    // private void OnCollisionEnter(Collision other) {
    //     if((collisionLayer.value & (1 << other.gameObject.layer)) != 0) {
    //         //COLLIDE WITH PLAYER CAR
    //         if(other.gameObject.GetComponent<CarController>()) {
    //             print(name + " enter");
    //             // print("has car controller");
    //             bc.enabled = true;
    //             // nma.isStopped = false;
    //         }
    //     }
    // }

    private void OnTriggerEnter(Collider other) {
        // print("ontriggerenter: " + other.gameObject.name);

        //layer 6 is VEHICLES
        if(other.gameObject.layer == 6) {
            bc.isTrigger = false;
            rb.isKinematic = false;
        }
    }

    private void OnCollisionStay(Collision other) {
        if((collisionLayer.value & (1 << other.gameObject.layer)) != 0) {
            //COLLIDE WITH PLAYER CAR
            if(other.gameObject.GetComponent<CarController>() && nma.isOnNavMesh && isChasingTarget) {
                // print("STAY");
                bc.isTrigger = true;
                rb.isKinematic = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        // print("ontriggerexit: " + other.gameObject.name);

        //layer 6 is VEHICLES
        if(other.gameObject.layer == 6) {
            bc.isTrigger = false;
            rb.isKinematic = false;
        }
    }
}