// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

// public class SpawnArea : MonoBehaviour {
//     public static SpawnArea current;
//     public int spawnDist;

//     [Space(10)]
//     //private SpawnManager spawnManager;
//     //private int maxSpawnCount;
//     [Header("VEHICLES")]
//     public bool isSpawningVehicles;
//     [SerializeField] private float vehicleSpawnTime;
//     public int maxVicCount;
//     public int vicCount;

//     [SerializeField] private Transform smallVehiclePFs;
//     [SerializeField] private Transform largeVehiclePFs;
//     [SerializeField] private Transform newVehiclePFs;
//     public int vehicleSpawnChance;
//     [Space(10)]

//     [Header("PERSONS")]
//     [SerializeField] private int personSpawnTime;
//     public int maxPersonCount;
//     public int personCount;

//     [SerializeField] private GameObject personPF;
//     [SerializeField] private Vector2Int personSpawnRange;

//     [SerializeField] private List<Transform> landmarkSpots;
//     [Space(10)]

//     [Header("OTHER")]
//     [SerializeField] private Transform player;
//     [SerializeField] private float positionUpdateFreq;

//     private void Awake() {
//         current = this;
//     }

//     private void Start() {
//         StartCoroutine(VehicleSpawnLoop());
//         InvokeRepeating("UpdatePosition", 0f, positionUpdateFreq);
//         // StartCoroutine(PersonSpawnLoop());

//         //Add all destinations
//         foreach(Transform landmark in landmarkSpots) {
//             PlayerInput.current.ToggleDestination(landmark.name);
//         }

//         isSpawningVehicles = true;
//     }

//     // private IEnumerator VehicleSpawnLoop() {
//     //     //WaitForSeconds wait = 

//     //     while (true) {
//     //         int vicLayerMask = 1 << 20;
//     //         Collider[] vicSpots = Physics.OverlapSphere(transform.position, spawnDist.y, vicLayerMask);

//     //         for(int i = 0; i < vicSpots.Length; i++) {
//     //             Collider vicSpot = vicSpots[i];
//     //             if(vicSpot.GetComponent<VehicleSpot>().triggerCount == 0 && Vector3.Distance(transform.position, vicSpot.transform.position) >= spawnDist.x)
//     //                 TrySpawnVehicle(vicSpot.transform);
//     //             else if(Vector3.Distance(transform.position, vicSpot.transform.position) > spawnDist.y)
//     //                 vicSpot.GetComponent<VehicleSpot>().isSpawnable = true;
//     //         }

//     //         yield return new WaitForSeconds(vehicleSpawnTime);
//     //     }
//     // }

//     private IEnumerator VehicleSpawnLoop() {
//         int vicLayerMask = 1 << 20;
//         VehicleSpot currentVehicleSpot;

//         while (true) {
//             Collider[] vicSpots = Physics.OverlapSphere(transform.position, spawnDist, vicLayerMask);

//             for(int i = 0; i < vicSpots.Length; i++) {
//                 Collider vicSpot = vicSpots[i];

//                 if(Vector3.Distance(transform.position, vicSpot.transform.position) >= spawnDist - 10) {
//                     currentVehicleSpot = vicSpot.GetComponent<VehicleSpot>();
//                     // currentVehicleSpot.GetComponent<MeshRenderer>().material = null;

//                     if(currentVehicleSpot.triggerCount == 0) {
//                         currentVehicleSpot.isSpawnable = !currentVehicleSpot.isSpawnable;
//                         TrySpawnVehicle(vicSpot.transform);
//                     }
//                     else if(Vector3.Distance(transform.position, vicSpot.transform.position) > spawnDist)
//                         currentVehicleSpot.isSpawnable = true;
//                 }
//             }

//             yield return new WaitForSeconds(vehicleSpawnTime);
//         }
//     }

//     // private IEnumerator PersonSpawnLoop() {
//     //     while (true) {
//     //         int personLayerMask = 1 << 10;
//     //         Collider[] personSpots = Physics.OverlapSphere(transform.position, spawnDist.y, personLayerMask);

//     //         for(int i = 0; i < personSpots.Length; i++) {
//     //             if(personSpots[i].GetComponent<SpawnHandler>().isSpawnable) {
//     //                 TrySpawnPerson(personSpots[i].transform);
//     //                 personSpots[i].GetComponent<SpawnHandler>().isSpawnable = false;
//     //             }
//     //         }

//     //         yield return new WaitForSeconds(personSpawnTime);
//     //     }
//     // }

//     private void UpdatePosition() {
//         transform.position = player.position;
//     }

//     private void TrySpawnPerson(Transform spot) {
//         if(personCount >= maxPersonCount) return;

//         //set number of persons to spawn
//         int toSpawn = Random.Range(personSpawnRange.x, personSpawnRange.y + 1);

//         //Spawn people
//         for(int i = 0; i < toSpawn; i++) {
//             //Set random variables
//             float spawnX = Random.Range(spot.position.x - (spot.localScale.x/2), spot.position.x + (spot.localScale.x/2));
//             float spawnZ = Random.Range(spot.position.z - (spot.localScale.z/2), spot.position.z + (spot.localScale.z/2));
//             float rotY = Random.Range(0, 361);

//             GameObject newPerson = Instantiate(personPF, new Vector3(spawnX, spot.position.y, spawnZ), Quaternion.identity);
//             newPerson.SetActive(true);
//             newPerson.transform.Rotate(new Vector3(0, rotY, 0));

//             newPerson.GetComponent<PersonController>().from = spot.parent.name;
//             newPerson.GetComponent<PersonController>().to = GetDestination(spot.parent.name);

//             //spawnManager.spawnCount ++;
//         }
//     }

//     private string GetDestination(string currentLandmark) {
//         string destination = "";

//         while(true) {
//             int destIndex = Random.Range(0, landmarkSpots.Count);
//             if(currentLandmark != landmarkSpots[destIndex].name) {
//                 destination = landmarkSpots[destIndex].name;
//                 break;
//             }
//         }

//         return destination;
//     }

//     private void TrySpawnVehicle(Transform spot) {
//         // if(!spot.GetComponent<VehicleSpot>().isSpawnable) return;
//         if(vicCount >= maxVicCount) return;

//         //Spawn roll
//         int spawnRoll = Random.Range(0, 101);
//         if(spawnRoll >= vehicleSpawnChance) return;
//         // print("SPAWNING: " + spawnRoll + "/" + vehicleSpawnChance);

//         //Node check
//         //if(spot.GetComponent<VehicleSpot>().node == null) return;

//         //set number of persons to spawn
//         // int pfIndex = Random.Range(0, vehiclePFs.Count);
//         // GameObject prefab = vehiclePFs[pfIndex];
//         Transform vehicles;

//         Vector3 spawnPos = spot.position;

//         if(spot.GetComponent<VehicleSpot>().onlySpawnSmallVics) vehicles = smallVehiclePFs;
//         else {
//             int pfIndex = Random.Range(0, 3);
//             if(pfIndex == 1) vehicles = smallVehiclePFs;
//             else if(pfIndex == 2) vehicles = largeVehiclePFs;
//             else {
//                 vehicles = newVehiclePFs;
//                 spawnPos.y += 1;
//             }
//         }

//         int vicIndex = Random.Range(0, vehicles.childCount);
//         GameObject prefab = vehicles.GetChild(vicIndex).gameObject;

//         GameObject newVehicle = Instantiate(prefab, spot.position, spot.rotation);
//         newVehicle.SetActive(true);

//         //Set nodes
//         if(spot.GetComponent<VehicleSpot>().node == null) newVehicle.GetComponent<aiCarController>().isActive = false;
//         newVehicle.GetComponent<aiCarController>().nextNode = spot.GetComponent<VehicleSpot>().node;

//         vicCount ++;
//     }

// //==============================================================================================================

//     // Try spawn on enter
//     private void OnTriggerEnter(Collider other) {
//         if(other.gameObject.layer == 10) TrySpawnPerson(other.transform); //10 = Person spawn spot
//         // else if(other.gameObject.layer == 20 && other.GetComponent<VehicleSpot>().triggerCount == 0 && Vector3.Distance(transform.position, other.transform.position) >= spawnDist - 20) TrySpawnVehicle(other.transform); //20 = Vehicle Spot
//     }

//     // private void OnTriggerStay(Collider other) {
//     //     if(other.gameObject.layer == 20 && other.GetComponent<VehicleSpot>().triggerCount == 0 && Vector3.Distance(transform.position, other.transform.position) > 90) TrySpawnVehicle(other.transform); //20 = Vehicle Spot
//     // }
// }