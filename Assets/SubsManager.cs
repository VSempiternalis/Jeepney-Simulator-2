using UnityEngine;
using System;

public class SubsManager : MonoBehaviour {
    public static SubsManager current;
    public Language language;
    public bool isSubsOn;

    public event Action onSubsOnChanged;
    public event Action<Language> onLanguageChanged;

    // [Header("ORIGINAL")]


    // [Header("ENGLISH")]

    private void Awake() {
        current = this;
    }

    private void Start() {
        
    }

    private void Update() {
        
    }

    public void SetSubtitlesOn(bool isOn) {
        isSubsOn = isOn;
        onSubsOnChanged?.Invoke();
    }

    public void SetLanguage(Language newLanguage) {
        print("SETTING SUBS LANGUAGE TO: " + newLanguage);
        language = newLanguage;
        onLanguageChanged?.Invoke(newLanguage);
    }

    public string GetSubs(int voiceIndex, int categoryIndex, int voiceLineIndex) {
        //[1] LANGUAGE

        //[2] VOICE TYPE

        //[3] CATEGORY

        //[2] VOICE LINE

        return "";
    }
}

public enum Language {
    OFF,
    ORIGINAL,
    ENGLISH
}