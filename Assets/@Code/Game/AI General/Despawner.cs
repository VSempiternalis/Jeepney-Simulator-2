using UnityEngine;

public class Despawner : MonoBehaviour {
    [SerializeField] private string objectType;
    [SerializeField] private Transform pool;

    [SerializeField] private bool isMainMenuDespawner;
    private int despawnDist = 100;

    private float nextSecUpdate;

    private Transform player;

    private bool despawning;
    private Vector3 newPos;

    private SpawnArea spawnArea;

    private void Start() {
        nextSecUpdate = Time.time + 1;

        player = GameObject.Find("PLAYER").transform;

        spawnArea = SpawnArea.current;

        despawnDist = PlayerPrefs.GetInt("Settings_SpawnDist", 100);
    }

    private void Update() {
        if(despawning) {
            if(Vector3.Distance(transform.position, newPos) < 1f) BackToPool();
            transform.position = newPos; //Must move to triggerexit
        }

        else if(Time.time >= (nextSecUpdate)) {
            nextSecUpdate ++;
            DistanceCheck();
        }
        
        // if(Time.time >= (nextSecUpdate)) {
        //     nextSecUpdate ++;
        //     DistanceCheck();
        // }
    }

    private void DistanceCheck() {
        float dist = Vector3.Distance(player.position, transform.position);
        // print("DIST: " + dist);
        if(isMainMenuDespawner && dist >= 140) Despawn();
        if(!isMainMenuDespawner && dist >= despawnDist + 10) Despawn();
    }

    public void Despawn() {
        // print("DESPAWNING " + name);
        // if(despawning) return;
        // if(GetComponent<aiCarController>() != null && GetComponent<Test_script>() != null) return;

        // // bool test = true;
        // if(GetComponent<aiCarController>() != null) {
        //     // SpawnArea.current.vicCount --;
        //     GetComponent<aiCarController>().enabled = false;
        //     // test = false;
        // }
        // if(GetComponent<aiCarInput>() != null) GetComponent<aiCarInput>().enabled = false;
        // if(GetComponent<Rigidbody>() != null) GetComponent<Rigidbody>().isKinematic = true;

        // // if(GetComponent<Test_script>() != null) {
        // //     SpawnArea.current.personCount --;
        // //     // test = false;
        // // }

        //Dont despawn when in vehicle
        // if(transform.parent != null && transform.parent.name.Contains("Seat") && GetComponent<Test_script>() && GetComponent<Test_script>().state != "Sitting") return;
        
        //[!] if(GetComponent<PersonController>() != null && (GetComponent<PersonController>().state == "Waiting to drop" || GetComponent<PersonController>().state == "Waiting for change" || GetComponent<PersonController>().state == "Waiting to pay")) return;

        if(objectType == "Vehicle") {
            spawnArea.vicCount --;
            GetComponent<aiCarController>().Reset();
        }
        else if(objectType == "Person") {
            spawnArea.personCount --;
            GetComponent<PersonHandler>().Reset();
        }
        else if(objectType == "Road Event") {
            spawnArea.roadEventCount --;
            GetComponent<RoadEvent>().Reset();
        }

        newPos = transform.position;
        newPos.y += 1000f;
        despawning = true;
    }

    private void BackToPool() {
        // print("BACK TO POOL " + name);
        transform.parent = pool;
        transform.localPosition = Vector3.zero;
        transform.rotation = pool.rotation;
        gameObject.SetActive(false);
        despawning = false;
    }
}