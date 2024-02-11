using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TGStylizedWorld;

public class SpawnArea : MonoBehaviour {
    public static SpawnArea current;
    public int spawnDist;

    [Space(10)]
    [Header("SPAWNING")]
    private Transform world;
    [SerializeField] private LayerMask vicSpawnLayerMask;
    [SerializeField] private LayerMask spawnsLayerMask;
    [SerializeField] private int vicSpawnLayer;
    [SerializeField] private int personSpawnLayer;
    private VehicleSpawn currentVehicleSpawn;
    private VehicleSpawn currentPersonSpawn;
    [SerializeField] private Transform personPool;

    [Space(10)]
    [Header("VEHICLES")]
    public bool isSpawningVehicles;
    [SerializeField] private float vehicleSpawnInterval;
    public int maxVicCount;
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

    [SerializeField] private List<Transform> landmarkSpots;

    private void Awake() {
        current = this;
    }

    private void Start() {
        world = GameObject.Find("WORLD").transform;
        StartCoroutine(SpawnLoop());
        spawnDist = PlayerPrefs.GetInt("Settings_SpawnDist", 100);
        // StartCoroutine(PersonSpawnLoop());

        //Add all destinations
        foreach(Transform landmark in landmarkSpots) {
            // PlayerInput.current.ToggleDestination(landmark.name);
        }

        isSpawningVehicles = true;
    }

    private IEnumerator SpawnLoop() {
        if(vicCount >= maxVicCount) yield return null;

        // int vicLayerMask = 1 << 20;

        while (true) {
            Collider[] spawns = Physics.OverlapSphere(transform.position, spawnDist, spawnsLayerMask);

            for(int i = 0; i < spawns.Length; i++) {
                Collider spawn = spawns[i];
                // print("COLLIDER: " + spawn.name);

                if(Vector3.Distance(transform.position, spawn.transform.position) >= spawnDist - 10) {
                    //[VEHICLES]
                    if(spawn.gameObject.layer == vicSpawnLayer) {
                        print("VEHICLE SPAWN LAYER");
                        currentVehicleSpawn = spawn.GetComponent<VehicleSpawn>();

                        if(currentVehicleSpawn.triggerCount == 0) {
                            currentVehicleSpawn.isSpawnable = !currentVehicleSpawn.isSpawnable;
                            TrySpawnVehicle(spawn.transform);
                        }
                        else if(Vector3.Distance(transform.position, spawn.transform.position) > spawnDist) {
                            currentVehicleSpawn.isSpawnable = true;
                        }
                    }
                    //[PERSONS]
                    else if(spawn.gameObject.layer == personSpawnLayer) {
                        print("PERSON SPAWN LAYER");
                        TrySpawnPerson(spawn.transform);
                    }
                }
            }

            yield return new WaitForSeconds(vehicleSpawnInterval);
        }

        // while (true) {
        //     Collider[] vicSpots = Physics.OverlapSphere(transform.position, spawnDist, vicSpawnLayerMask);

        //     for(int i = 0; i < vicSpots.Length; i++) {
        //         Collider vicSpot = vicSpots[i];

        //         if(Vector3.Distance(transform.position, vicSpot.transform.position) >= spawnDist - 10) {
        //             currentVehicleSpawn = vicSpot.GetComponent<VehicleSpawn>();

        //             if(currentVehicleSpawn.triggerCount == 0) {
        //                 currentVehicleSpawn.isSpawnable = !currentVehicleSpawn.isSpawnable;
        //                 TrySpawnVehicle(vicSpot.transform);
        //             }
        //             else if(Vector3.Distance(transform.position, vicSpot.transform.position) > spawnDist)
        //                 currentVehicleSpawn.isSpawnable = true;
        //         }
        //     }

        //     yield return new WaitForSeconds(vehicleSpawnInterval);
        // }
    }

    private void TrySpawnPerson(Transform spawn) {
        print("TRY SPAWN PERSON");
        if(personCount >= maxPersonCount) return;

        //set number of persons to spawn on spawn transform
        int toSpawn = Random.Range(personSpawnRange.x, personSpawnRange.y + 1);

        //Spawn people
        for(int i = 0; i < toSpawn; i++) {
            if(personPool.childCount == 0) return;
            
            print("Spawning");
            //Set random variables
            float spawnX = Random.Range(spawn.position.x - (spawn.localScale.x/2), spawn.position.x + (spawn.localScale.x/2));
            float spawnZ = Random.Range(spawn.position.z - (spawn.localScale.z/2), spawn.position.z + (spawn.localScale.z/2));
            float rotY = Random.Range(0, 361);

            int newPersonIndex = Random.Range(0, personPool.childCount);
            GameObject newPerson = personPool.GetChild(newPersonIndex).gameObject;
            print("Spawning " + newPerson.name);
            // newPerson = Instantiate(personPF, new Vector3(spawnX, spawn.position.y, spawnZ), Quaternion.identity);
            newPerson.transform.position = new Vector3(spawnX, spawn.position.y, spawnZ);
            newPerson.transform.Rotate(new Vector3(0, rotY, 0));
            newPerson.transform.parent = world;
            newPerson.SetActive(true);
            newPerson.GetComponent<TGCharacterAppearance>().Start();

            newPerson.GetComponent<PersonHandler>().from = spawn.parent.name;
            newPerson.GetComponent<PersonHandler>().to = GetDestination(spawn.parent.name);

            //spawnManager.spawnCount ++;
        }
    }

    private string GetDestination(string currentLandmark) {
        string destination = "";

        while(true) {
            int destIndex = Random.Range(0, landmarkSpots.Count);
            if(currentLandmark != landmarkSpots[destIndex].name) {
                destination = landmarkSpots[destIndex].name;
                break;
            }
        }

        return destination;
    }

    private void TrySpawnVehicle(Transform spot) {
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
                spawnPos.y += 1;
            }
        }

        print("CHECK");
        if(vehicles.childCount == 0) return;

        //OBJECT POOLING
        print("SPAWN");
        int vicIndex = Random.Range(0, vehicles.childCount);
        GameObject newVic = vehicles.GetChild(vicIndex).gameObject;
        newVic.transform.position = spawnPos;
        newVic.transform.rotation = spot.rotation;
        newVic.transform.parent = world;
        newVic.SetActive(true);
        //Collider
        // newVic.GetComponent<BoxCollider>().enabled = true;
        //Audio
        //Code

        //OLD (Spawning/despawning) ================================================
        // int vicIndex = Random.Range(0, vehicles.childCount);
        // print("SPAWN POS: " + spawnPos);
        // GameObject prefab = vehicles.GetChild(vicIndex).gameObject;
        // GameObject newVehicle = Instantiate(prefab, world);
        // newVehicle.transform.localPosition = spawnPos;
        // newVehicle.transform.rotation = spot.rotation;
        // newVehicle.SetActive(true);

        //Set nodes
        if(spot.GetComponent<VehicleSpawn>().node == null) newVic.GetComponent<aiCarController>().isActive = false;
        newVic.GetComponent<aiCarController>().nextNode = spot.GetComponent<VehicleSpawn>().node;

        vicCount ++;
    }
}