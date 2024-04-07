using UnityEngine;

public class Carousel : MonoBehaviour {
    [SerializeField] private int index;
    [SerializeField] private int childCount;

    private void Start() {
        // childCount = transform.childCount;

        // foreach(Transform child in transform) {
        //     child.gameObject.SetActive(true);
        // }

        // transform.GetChild(0).gameObject.SetActive(true);
    }

    public void Next() {
        print("Next " + index + " child:" + transform.GetChild(index).name + " childCount: " + childCount);
        transform.GetChild(index).gameObject.SetActive(false);

        if(index == (childCount - 1)) {
            index = 0;
        } else index ++;
        
        transform.GetChild(index).gameObject.SetActive(true);
    }

    public void Back() {
        print("Back " + index + " " + transform.GetChild(index).name);
        transform.GetChild(index).gameObject.SetActive(false);

        if(index == 0) {
            index = childCount - 1;
        } else index --;

        transform.GetChild(index).gameObject.SetActive(true);
    }
}
