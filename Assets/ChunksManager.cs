using UnityEngine;
using System.Collections.Generic;

public class ChunksManager : MonoBehaviour {
    [SerializeField] private int loadDist;
    [SerializeField] private int loadFreq = 1;
    [SerializeField] private Transform player;
    [SerializeField] private List<Transform> chunks;
    private float dist;

    private void Start() {
        // chunks = GameObject.FindGameObjectsWithTag("Chunk").;
        foreach(GameObject chunk in GameObject.FindGameObjectsWithTag("Chunk")) {
            chunks.Add(chunk.transform);
        }

        InvokeRepeating("ChunkCheck", 0f, loadFreq);
    }

    private void ChunkCheck() {
        // print(Time.time + " chunk check");
        Vector3 playerPos = player.position;
        foreach(Transform chunk in chunks) {
            dist = Vector3.Distance(playerPos, chunk.position);
            // print("dist: " + dist);

            //TURN OFF
            if(chunk.gameObject.activeSelf && dist > loadDist) {
                chunk.gameObject.SetActive(false);
            }
            //TURN ON
            else if(!chunk.gameObject.activeSelf && dist <= loadDist) {
                chunk.gameObject.SetActive(true);
            }
        }
    }
}
