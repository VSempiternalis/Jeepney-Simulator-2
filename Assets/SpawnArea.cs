using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnArea : MonoBehaviour {
    public static SpawnArea current;
    public int spawnDist;

    [Space(10)]
    [Header("VEHICLES")]
    private Transform world;
    [SerializeField] private LayerMask vicSpawnLayerMask;
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
    [Space(10)]

    [Header("OTHER")]
    [SerializeField] private Transform player;
    [SerializeField] private float positionUpdateFreq;

    private void Awake() {
        current = this;
    }

    private void Start() {
        world = GameObject.Find("WORLD").transform;
        StartCoroutine(VehicleSpawnLoop());
        spawnDist = PlayerPrefs.GetInt("Settings_SpawnDist", 100);
        // InvokeRepeating("UpdatePosition", 0f, positionUpdateFreq);
        // StartCoroutine(PersonSpawnLoop());

        //Add all destinations
        foreach(Transform landmark in landmarkSpots) {
            // PlayerInput.current.ToggleDestination(landmark.name);
        }

        isSpawningVehicles = true;
    }

    private IEnumerator VehicleSpawnLoop() {
        if(vicCount >= maxVicCount) yield return null;

        // int vicLayerMask = 1 << 20;
        VehicleSpawn currentVehicleSpawn;

        while (true) {
            Collider[] vicSpots = Physics.OverlapSphere(transform.position, spawnDist, vicSpawnLayerMask);

            for(int i = 0; i < vicSpots.Length; i++) {
                Collider vicSpot = vicSpots[i];

                if(Vector3.Distance(transform.position, vicSpot.transform.position) >= spawnDist - 10) {
                    currentVehicleSpawn = vicSpot.GetComponent<VehicleSpawn>();

                    if(currentVehicleSpawn.triggerCount == 0) {
                        currentVehicleSpawn.isSpawnable = !currentVehicleSpawn.isSpawnable;
                        TrySpawnVehicle(vicSpot.transform);
                    }
                    else if(Vector3.Distance(transform.position, vicSpot.transform.position) > spawnDist)
                        currentVehicleSpawn.isSpawnable = true;
                }
            }

            yield return new WaitForSeconds(vehicleSpawnInterval);
        }
    }

    private void UpdatePosition() {
        transform.position = player.position;
    }

    private void TrySpawnPerson(Transform spot) {
        if(personCount >= maxPersonCount) return;

        //set number of persons to spawn
        int toSpawn = Random.Range(personSpawnRange.x, personSpawnRange.y + 1);

        //Spawn people
        for(int i = 0; i < toSpawn; i++) {
            //Set random variables
            float spawnX = Random.Range(spot.position.x - (spot.localScale.x/2), spot.position.x + (spot.localScale.x/2));
            float spawnZ = Random.Range(spot.position.z - (spot.localScale.z/2), spot.position.z + (spot.localScale.z/2));
            float rotY = Random.Range(0, 361);

            GameObject newPerson = Instantiate(personPF, new Vector3(spawnX, spot.position.y, spawnZ), Quaternion.identity);
            newPerson.SetActive(true);
            newPerson.transform.Rotate(new Vector3(0, rotY, 0));

            // newPerson.GetComponent<PersonController>().from = spot.parent.name;
            // newPerson.GetComponent<PersonController>().to = GetDestination(spot.parent.name);

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
        
        Transform vehicles;

        Vector3 spawnPos = spot.localPosition;

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

        int vicIndex = Random.Range(0, vehicles.childCount);
        // print("SPAWN POS: " + spawnPos);
        GameObject prefab = vehicles.GetChild(vicIndex).gameObject;

        GameObject newVehicle = Instantiate(prefab, world);
        newVehicle.transform.localPosition = spawnPos;
        newVehicle.transform.rotation = spot.rotation;

        newVehicle.SetActive(true);

        //Set nodes
        if(spot.GetComponent<VehicleSpawn>().node == null) newVehicle.GetComponent<aiCarController>().isActive = false;
        newVehicle.GetComponent<aiCarController>().nextNode = spot.GetComponent<VehicleSpawn>().node;

        vicCount ++;
    }

//==============================================================================================================

    // Try spawn on enter
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == 10) TrySpawnPerson(other.transform); //10 = Person spawn spot
        // else if(other.gameObject.layer == 20 && other.GetComponent<VehicleSpot>().triggerCount == 0 && Vector3.Distance(transform.position, other.transform.position) >= spawnDist - 20) TrySpawnVehicle(other.transform); //20 = Vehicle Spot
    }

    // private void OnTriggerStay(Collider other) {
    //     if(other.gameObject.layer == 20 && other.GetComponent<VehicleSpot>().triggerCount == 0 && Vector3.Distance(transform.position, other.transform.position) > 90) TrySpawnVehicle(other.transform); //20 = Vehicle Spot
    // }
}