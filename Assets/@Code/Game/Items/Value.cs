using UnityEngine;

public class Value : MonoBehaviour, ITooltipable {
    public int value;

    public string GetHeader() {
        return name;
    }

    public string GetText() {
        return "Value: " + value;
    }
}
