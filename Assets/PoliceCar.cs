using System.Collections;
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

    //Better timed destination updates
    private float destinationUpdateInterval = 0.5f;
    private float timeSinceLastUpdate = 0f;

    private void Start() {
        carCon = GetComponent<aiCarController>();
        aci = GetComponent<aiCarInput>();
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
        timeSinceLastUpdate += Time.fixedDeltaTime;

        if(carCon.enabled == false) {
            float distToTarget = Vector3.Distance(transform.position, target.position);
            float distToPlayer = Vector3.Distance(transform.position, player.position);
            float distToPlayerJeep = Vector3.Distance(transform.position, playerJeep.position);

            //stop when too close
            if(nma.isOnNavMesh && (distToTarget < 3 || distToPlayer < 5 || distToPlayerJeep < 5 )) {
                print(name + "STOPPING POLICE CAR");
                nma.enabled = false;
                rb.isKinematic = false;
                nma.isStopped = true;
            } else if(!nma.enabled && !(distToTarget < 3 || distToPlayer < 5 || distToPlayerJeep < 5 )) {
                print(name + "STARTING POLICE CAR");
                nma.enabled = true;
                rb.isKinematic = true;
                nma.isStopped = false;
            }

            //check if close to target, start arresting
            if(distToPlayerJeep < arrestRange) {
                cm.arrestProgress ++;
            }

            //if can see player, no aggro loss
            if(distToTarget < visionRange) {
                if(cm.isAggroLoss != false) cm.isAggroLoss = false;
            }

            //chase target
            if(nma.isOnNavMesh && nma != null && timeSinceLastUpdate >= destinationUpdateInterval && isChasingTarget) {
                //check if player is still wanted
                if(!cm.isPlayerWanted) SetIsChasing(false);
                nma.SetDestination(target.position);
                nma.acceleration = nmaAcc*carCon.healthFactor;
                timeSinceLastUpdate = 0f;
            }
        }
    }

    public void SetIsChasing(bool newIsChasing) {
        print(name + " is chasing: " + newIsChasing);
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
        print("COP CAR CHASING");

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
    }

    private void OnTriggerEnter(Collider other) {
        // print("ontriggerenter: " + other.gameObject.name);

        //layer 6 is VEHICLES
        if(other.gameObject.layer == 6) {
            // print("police ontriggerenter. collider on");
            bc.isTrigger = false;
            rb.isKinematic = false;
        }
    }

    private void OnCollisionStay(Collision other) {
        // print("oncollisionstay");
        if((collisionLayer.value & (1 << other.gameObject.layer)) != 0) {
            // print("collisionlayer");
            //COLLIDE WITH PLAYER CAR
            if(other.gameObject.GetComponent<CarController>() && nma.isOnNavMesh && isChasingTarget) {
                // print("police oncollisionstay. collider off");
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
            // print("police ontriggerexit. collider on");
            bc.isTrigger = false;
            rb.isKinematic = false;
        }
    }
}