using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Assign this script to the indicator prefabs.
/// </summary>
public class Indicator : MonoBehaviour
{
    [SerializeField] private IndicatorType indicatorType;
    private Image indicatorImage;
    public TMP_Text speakerText;
    public TMP_Text subtitleText;

    public Target subsHolder;

    /// <summary>
    /// Gets if the game object is active in hierarchy.
    /// </summary>
    public bool Active
    {
        get
        {
            return transform.gameObject.activeInHierarchy;
        }
    }

    /// <summary>
    /// Gets the indicator type
    /// </summary>
    public IndicatorType Type
    {
        get
        {
            return indicatorType;
        }
    }

    void Awake() {
        indicatorImage = transform.GetComponent<Image>();
        speakerText = transform.GetChild(0).GetComponent<TMP_Text>();
        subtitleText = transform.GetChild(1).GetComponent<TMP_Text>();
    }

    /// <summary>
    /// Sets the image color for the indicator.
    /// </summary>
    /// <param name="color"></param>
    public void SetImageColor(Color color) {
        indicatorImage.color = color;
    }

    /// <summary>
    /// Sets the distance text for the indicator.
    /// </summary>
    /// <param name="value"></param>
    public void SetDistanceText(float value) {
        if(indicatorType != IndicatorType.SUBTITLE) subtitleText.text = value >= 0 ? Mathf.Floor(value) + " m" : "";
    }

    public void SetText(string speaker, string subtitles) {
        // text.text = value;
        speakerText.text = speaker;
        subtitleText.text = subtitles;
    }

    /// <summary>
    /// Sets the distance text rotation of the indicator.
    /// </summary>
    /// <param name="rotation"></param>
    public void SetTextRotation(Quaternion rotation)
    {
        speakerText.rectTransform.rotation = rotation;
        subtitleText.rectTransform.rotation = rotation;
    }

    /// <summary>
    /// Sets the indicator as active or inactive.
    /// </summary>
    /// <param name="value"></param>
    public void Activate(bool value)
    {
        transform.gameObject.SetActive(value);
    }
}

public enum IndicatorType {
    BOX,
    ARROW,
    SUBTITLE
}
