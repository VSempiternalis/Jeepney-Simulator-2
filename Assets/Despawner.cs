using UnityEngine;

public class Despawner : MonoBehaviour {
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
            // print("DESPAWNING");
            if(Vector3.Distance(transform.position, newPos) < 0.1f) Destroy(gameObject);
            transform.position = newPos;
            // transform.position = Vector3.MoveTowards(transform.position, newPos, 1000f);
        }

        else if(Time.time >= (nextSecUpdate)) {
            nextSecUpdate ++;
            DistanceCheck();
        }
    }

    private void DistanceCheck() {
        float dist = Vector3.Distance(player.position, transform.position);
        print("DIST: " + dist);
        if(dist >= despawnDist + 10) Despawn();
    }

    public void Despawn() {
        if(despawning) return;
        // if(GetComponent<aiCarController>() != null && GetComponent<Test_script>() != null) return;

        // bool test = true;
        if(GetComponent<aiCarController>() != null) {
            // SpawnArea.current.vicCount --;
            GetComponent<aiCarController>().enabled = false;
            // test = false;
        }
        if(GetComponent<aiCarInput>() != null) GetComponent<aiCarInput>().enabled = false;
        if(GetComponent<Rigidbody>() != null) GetComponent<Rigidbody>().isKinematic = true;

        // if(GetComponent<Test_script>() != null) {
        //     SpawnArea.current.personCount --;
        //     // test = false;
        // }

        //Dont despawn when in vehicle
        // if(transform.parent != null && transform.parent.name.Contains("Seat") && GetComponent<Test_script>() && GetComponent<Test_script>().state != "Sitting") return;
        
        //[!] if(GetComponent<PersonController>() != null && (GetComponent<PersonController>().state == "Waiting to drop" || GetComponent<PersonController>().state == "Waiting for change" || GetComponent<PersonController>().state == "Waiting to pay")) return;

        // print(gameObject.layer);
        // print(SpawnArea.current.vicCount);
        if(gameObject.layer == 6) spawnArea.vicCount --;
        else if(gameObject.layer == 13) spawnArea.personCount --;

        // if(test) print(name + " despawning without decrement!");
        // print("DESPAWN: " + name + " has aiCarCon: " + (GetComponent<aiCarController>() != null));
        // print("DESPAWN: " + name + " has aiCarCon: " + GetComponent<aiCarController>() != null);

        newPos = transform.position;
        newPos.y += 1000f;
        despawning = true;
        //Destroy(gameObject);

        // print("DESPAWN FR");
    }
}
