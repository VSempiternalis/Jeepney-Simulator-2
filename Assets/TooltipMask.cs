using UnityEngine;

public class TooltipMask : MonoBehaviour {
    public static TooltipMask current;

    [SerializeField] private RectTransform rt;
    [SerializeField] private RectTransform headerBG;
    [SerializeField] private RectTransform descBG;
    [SerializeField] private float smoothness;

    [SerializeField] private float inTime;
    public LeanTweenType inEasingType = LeanTweenType.easeOutQuart;

    private void Awake() {
        current = this;
    }

    private void Start() {
        rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(0, 0);
    }

    private void Update() {
        
    }

    public void In() {
        float newX = descBG.sizeDelta.x;
        float newY = headerBG.sizeDelta.y + 2 + descBG.sizeDelta.y;
        float x = 0;
        float y = 0;
        // float x = newX;
        // float y = Mathf.Lerp(rt.sizeDelta.y, newY, Time.deltaTime * smoothness);

        if(headerBG.sizeDelta.x > descBG.sizeDelta.x) newX = headerBG.sizeDelta.x;
        headerBG.sizeDelta = new Vector2(descBG.sizeDelta.x, headerBG.sizeDelta.y);
        // if(descBG.sizeDelta.x > headerBG.sizeDelta.x) {
        //     newX = descBG.sizeDelta.x;
        //     headerBG.sizeDelta = new Vector2(descBG.sizeDelta.x, headerBG.sizeDelta.y);
        // }

        // float x = Mathf.Lerp(rt.sizeDelta.x, newX, Time.deltaTime * smoothness);
        // float y = headerBG.sizeDelta.y;
        // rt.sizeDelta = new Vector2(x, y);

        if(rt.sizeDelta.x < newX - 10) {
            x = Mathf.Lerp(rt.sizeDelta.x, newX, Time.deltaTime * smoothness);
            y = headerBG.sizeDelta.y;
        } else {
            x = Mathf.Lerp(rt.sizeDelta.x, newX, Time.deltaTime * smoothness);
            y = Mathf.Lerp(rt.sizeDelta.y, newY, Time.deltaTime * smoothness);
            // rt.sizeDelta = new Vector2(x, y);
        }
        rt.sizeDelta = new Vector2(x, y);
    }

    public void Out() {
        headerBG.sizeDelta = Vector2.zero;
        rt.sizeDelta = Vector2.zero;
    }
}
