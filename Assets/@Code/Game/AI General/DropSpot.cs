using UnityEngine;

public class DropSpot : MonoBehaviour {
    [SerializeField] private Transform dropField;
    public bool isIllegal;

    private void Start() {
        
    }

    private void Update() {
        
    }

    public Vector3 GetDropPos() {
        float dropX = Random.Range(dropField.position.x - (dropField.localScale.x/2), dropField.position.x + (dropField.localScale.x/2));
        float dropZ = Random.Range(dropField.position.z - (dropField.localScale.z/2), dropField.position.z + (dropField.localScale.z/2));
        float dropY = dropField.position.y - 1;
        // float dropY = 0;

        return new Vector3(dropX, dropY, dropZ);
    }
}
