using UnityEngine;
using System.Collections.Generic;

public class ChunksManager : MonoBehaviour {
    [SerializeField] private int loadDist;
    [SerializeField] private int loadFreq = 1;
    [SerializeField] private Transform player;
    [SerializeField] private Transform chunks;
    [SerializeField] private List<Transform> chunksList;
    private float dist;

    private void Start() {
        // chunks = GameObject.FindGameObjectsWithTag("Chunk").;
        foreach(Transform chunk in chunks) {
            chunksList.Add(chunk);
        }

        InvokeRepeating("ChunkCheck", 0f, loadFreq);
    }

    private void ChunkCheck() {
        loadDist = ((int)PlayerPrefs.GetFloat("Settings_RenderDist", 30)*100) + 200;

        // print(Time.time + " chunk check");
        Vector3 playerPos = player.position;
        foreach(Transform chunk in chunksList) {
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
