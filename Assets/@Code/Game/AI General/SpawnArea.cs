using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TGStylizedWorld;
// using UnityEditor.ShaderGraph;

public class SpawnArea : MonoBehaviour {
    public static SpawnArea current;
    public int spawnDist;
    [SerializeField] private bool isMainMenuSpawn;

    [Space(10)]
    [Header("SPAWNING")]
    private Transform world;
    [SerializeField] private LayerMask vicSpawnLayerMask;
    [SerializeField] private LayerMask spawnsLayerMask;
    [SerializeField] private int vicSpawnLayer;
    [SerializeField] private int personSpawnLayer;
    [SerializeField] private int roadEventSpawnLayer;
    private VehicleSpawn currentVehicleSpawn;
    private VehicleSpawn currentPersonSpawn;
    [SerializeField] private Transform personPool;

    [Space(10)]
    [Header("VEHICLES")]
    public bool isSpawningVehicles;
    [SerializeField] private float vehicleSpawnInterval;
    public int maxVicCount;
    public int wantedVicCount = 10; //vic count when player is wanted
    public int currentMaxVicCount; //used for spawning
    public int vicCount;

    [SerializeField] private Transform smallVehiclePFs;
    [SerializeField] private Transform largeVehiclePFs;
    [SerializeField] private Transform newVehiclePFs;
    public int vehicleSpawnChance;
    [Space(10)]

    [Header("PERSONS")]
    [SerializeField] private int personSpawnTime;
    public int maxPersonCount;
    public int personCount;

    [SerializeField] private GameObject personPF;
    [SerializeField] private Vector2Int personSpawnRange;
    [Space(10)]

    [Header("ROAD EVENTS")]
    public bool isRoadEvents;
    [SerializeField] private Transform roadEventPoolSize1;
    [SerializeField] private Transform roadEventPoolSize2;
    [SerializeField] private Transform roadEventPoolSize3;
    public int roadEventCount;
    public int maxRoadEventCount;
    public int roadEventSpawnChance;

    [SerializeField] private List<Transform> landmarkSpawnParents;
    [SerializeField] private Transform landmarkSpawns;

    private void Awake() {
        current = this;
    }

    private void Start() {
        world = GameObject.Find("WORLD").transform;
        // else 
        // spawnDist = PlayerPrefs.GetInt("Settings_SpawnDist", 100);
        spawnDist = 100;
        StartCoroutine(SpawnLoop());
        transform.localScale = new Vector3(spawnDist, spawnDist, spawnDist);
        // StartCoroutine(PersonSpawnLoop());

        //Add all destinations
        foreach(Transform landmark in landmarkSpawnParents) {
            // PlayerInput.current.ToggleDestination(landmark.name);
        }

        foreach(Transform landmark in landmarkSpawns) {
            landmarkSpawnParents.Add(landmark);
        }

        isSpawningVehicles = true;
        currentMaxVicCount = maxVicCount;
    }

    private IEnumerator SpawnLoop() {
        while(isSpawningVehicles) {
            if(isMainMenuSpawn) spawnDist = 130;
            Collider[] spawns = Physics.OverlapSphere(transform.position, spawnDist, spawnsLayerMask);

            for(int i = 0; i < spawns.Length; i++) {
                if(vicCount >= currentMaxVicCount) break;

                Collider spawn = spawns[i];

                if(Vector3.Distance(transform.position, spawn.transform.position) >= spawnDist - 10) {
                    if(spawn.gameObject.layer == vicSpawnLayer) {
                        currentVehicleSpawn = spawn.GetComponent<VehicleSpawn>();

                        if(currentVehicleSpawn.triggerCount == 0) {
                            currentVehicleSpawn.isSpawnable = !currentVehicleSpawn.isSpawnable;
                            TrySpawnVehicle(spawn.transform);
                        }
                        else if(Vector3.Distance(transform.position, spawn.transform.position) > spawnDist) {
                            currentVehicleSpawn.isSpawnable = true;
                        }
                    }
                }
            }

            yield return new WaitForSeconds(vehicleSpawnInterval);
        }
    }

    private void TrySpawnPerson(Transform spawn) {
        // print("tryspawnperson");
        if(personCount >= maxPersonCount || personPool.childCount == 0) return;

        int toSpawn;
        Crosswalk crosswalk = null;

        //CROSSWALK
        if(spawn.GetComponent<Crosswalk>()) {
            crosswalk = spawn.GetComponent<Crosswalk>();
            toSpawn = crosswalk.GetSpawn();
        } else {
            //set number of persons to spawn on spawn transform
            toSpawn = Random.Range(personSpawnRange.x, personSpawnRange.y + 1);
        }

        //Spawn people
        for(int i = 0; i < toSpawn; i++) {
            if(personCount >= maxPersonCount || personPool.childCount == 0) break;
            
            //Set random variables
            float spawnX = Random.Range(spawn.position.x - (spawn.localScale.x/2), spawn.position.x + (spawn.localScale.x/2));
            float spawnZ = Random.Range(spawn.position.z - (spawn.localScale.z/2), spawn.position.z + (spawn.localScale.z/2));
            float rotY = Random.Range(0, 361);

            int newPersonIndex = Random.Range(0, personPool.childCount);
            GameObject newPerson = personPool.GetChild(newPersonIndex).gameObject;
            newPerson.transform.position = new Vector3(spawnX, spawn.position.y, spawnZ);
            newPerson.transform.Rotate(new Vector3(0, rotY, 0));
            // newPerson.transform.SetParent(null);
            newPerson.transform.parent = world;
            newPerson.SetActive(true);
            newPerson.GetComponent<TGCharacterAppearance>().Start();

            newPerson.GetComponent<PersonHandler>().from = spawn.parent.name;
            newPerson.GetComponent<PersonHandler>().landmarkDest = GetDestination(spawn.parent.name);

            if(crosswalk != null) newPerson.GetComponent<PersonHandler>().CrossRoad(crosswalk.otherCrosswalk);
            else newPerson.GetComponent<PersonHandler>().MakeWait();
            personCount ++;
        }
    }

    private string GetDestination(string currentLandmark) {
        string destination = "";

        //MUST HAVE MORE THAN ONE LANDMARKSPAWNPARENT
        while(true) {
            int destIndex = Random.Range(0, landmarkSpawnParents.Count);
            if(currentLandmark != landmarkSpawnParents[destIndex].name) {
                destination = landmarkSpawnParents[destIndex].name;
                break;
            }
        }

        return destination;
    }

    private void TrySpawnVehicle(Transform spot) {
        // print("try spawn vic " + Time.time);
        //Spawn roll
        int spawnRoll = Random.Range(0, 101);
        if(spawnRoll >= vehicleSpawnChance) return;

        Vector3 spawnPos = spot.position;
        
        //Get vehicles pool to use
        Transform vehicles;
        if(spot.GetComponent<VehicleSpawn>().onlySpawnSmallVics) vehicles = smallVehiclePFs;
        else {
            int pfIndex = Random.Range(0, 3);
            if(pfIndex == 1) vehicles = smallVehiclePFs;
            else if(pfIndex == 2) vehicles = largeVehiclePFs;
            else {
                vehicles = newVehiclePFs;
                spawnPos.y -= 2;
            }
        }

        if(vehicles.childCount == 0) return;

        //OBJECT POOLING
        int vicIndex = Random.Range(0, vehicles.childCount);
        GameObject newVic = vehicles.GetChild(vicIndex).gameObject;
        newVic.transform.position = spawnPos;
        newVic.transform.rotation = spot.rotation;
        newVic.transform.parent = world;
        newVic.SetActive(true);

        //Audio

        //Set nodes
        newVic.GetComponent<aiCarController>().SetActive(true);
        if(spot.GetComponent<VehicleSpawn>().node == null) newVic.GetComponent<aiCarController>().SetActive(false);
        newVic.GetComponent<aiCarController>().nextNode = spot.GetComponent<VehicleSpawn>().node;

        vicCount ++;
    }

    private void TrySpawnRoadEvent(RoadEventSpawn res) {
        // print("ROAD EVENT SPAWN: " + res.name);

        //max spawn
        if(roadEventCount >= maxRoadEventCount) return;
        if(res.triggerCount > 0) return;

        //check if event happens
        int randInt = Random.Range(0, 101);
        if(randInt >= roadEventSpawnChance) return;

        Transform pool;

        //get random event for size
        if(res.laneSize == 1) pool = roadEventPoolSize1;
        else if(res.laneSize == 2) pool = roadEventPoolSize2;
        else pool = roadEventPoolSize3;

        //skip if no events in pool
        if(pool.childCount == 0) return;

        //spawn
        int randInt2 = Random.Range(0, pool.childCount);
        RoadEvent re = pool.GetChild(randInt2).GetComponent<RoadEvent>();
        re.transform.position = res.transform.position;
        re.transform.rotation = res.transform.rotation;
        re.transform.parent = world;
        re.gameObject.SetActive(true);

        roadEventCount ++;
    }
    
    // Try spawn on enter
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == personSpawnLayer) TrySpawnPerson(other.transform);
        else if(isRoadEvents && other.gameObject.layer == roadEventSpawnLayer) TrySpawnRoadEvent(other.GetComponent<RoadEventSpawn>());
    }
}