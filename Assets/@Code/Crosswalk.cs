using UnityEngine;

public class Crosswalk : MonoBehaviour {
    [SerializeField] private Vector2 toSpawn;
    [SerializeField] private int spawnChance;
    public Transform otherCrosswalk;

    private void Start() {
        
    }

    private void Update() {
        
    }

    public int GetSpawn() {
        int returnInt = 0;

        int roll = Random.Range(0, 101);
        if(roll <= spawnChance) {
            returnInt = (int)Random.Range(toSpawn.x, toSpawn.y);
        }

        return returnInt;
    }
}
