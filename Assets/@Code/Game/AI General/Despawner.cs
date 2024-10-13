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
    }

    private void DistanceCheck() {
        float dist = Vector3.Distance(player.position, transform.position);
        // print("DIST: " + dist);
        if(isMainMenuDespawner && dist >= 140) Despawn();
        if(!isMainMenuDespawner && dist >= despawnDist + 10) Despawn();
    }

    public void Despawn() {
        if(objectType == "Vehicle") {
            spawnArea.vicCount --;
            GetComponent<aiCarController>().Reset();
            GetComponent<aiCarController>().ResetHealth();
        }
        else if(objectType == "Person") {
            //Dont despawn when in vehicle
            if(!GetComponent<PersonHandler>().CanReset()) return;

            // print("despawning: " + name);
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