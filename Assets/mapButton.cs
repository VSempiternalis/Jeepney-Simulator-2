using UnityEngine;

public class mapButton : MonoBehaviour, IInteractable {
    [SerializeField] private Camera target;
    [SerializeField] private int zoomRate;
    [SerializeField] private float zoomTime;

    private AudioManager am;

    private void Start() {
        am = AudioManager.current;
    }

    private void Update() {
        
    }

    public void Interact(GameObject interactor) {
        float targetSize = target.orthographicSize;
        float newSize = targetSize + zoomRate;

        if(newSize < 0 || newSize > 1000) return;

        LeanTween.value(target.gameObject, targetSize, newSize, zoomTime)
        .setEaseOutQuint()
        .setOnUpdate((float size) => {
            target.orthographicSize = size;
        });

        am.PlayUI(1);
    }
}
