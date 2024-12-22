using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoiceHandler : MonoBehaviour {
    private AudioSource audioSource;
    private SubsManager sm;

    private bool subtitlesOn = true;
    private Language language;
    private bool isLanguageReceived;
    [SerializeField] private Target subs;

    //should be clear on start, to be filled by person handler
    [Header("MAIN")]
    public string voiceName;
    [SerializeField] public List<AudioClip> payAudios;
    [SerializeField] public List<AudioClip> changeAudios;
    [SerializeField] public List<AudioClip> stopAudios;
    [SerializeField] public List<AudioClip> deathAudios;

    [Header("OPTIONAL")]
    public List<AudioClip> dropAudios;
    public List<AudioClip> thanksAudios;
    public List<AudioClip> chatterAudios;
    public List<AudioClip> gasStationAudios;
    public List<AudioClip> hitAudios;
    public List<AudioClip> policeAudios;

    [Header("SUBS: ORIGINAL")]
    public List<List<string>> subsORIG;
    [SerializeField] public List<string> paySubsORIG;
    [SerializeField] public List<string> changeSubsORIG;
    [SerializeField] public List<string> stopSubsORIG;
    [SerializeField] public List<string> deathSubsORIG;
    [SerializeField] public List<string> dropSubsORIG;
    [SerializeField] public List<string> thanksSubsORIG;
    [SerializeField] public List<string> chatterSubsORIG;
    [SerializeField] public List<string> gasSubsORIG;
    [SerializeField] public List<string> hitSubsORIG;
    [SerializeField] public List<string> policeSubsORIG;

    [Header("SUBS: ENGLISH")]
    private List<List<string>> subsENG;
    [SerializeField] public List<string> paySubsENG;
    [SerializeField] public List<string> changeSubsENG;
    [SerializeField] public List<string> stopSubsENG;
    [SerializeField] public List<string> deathSubsENG;
    [SerializeField] public List<string> dropSubsENG;
    [SerializeField] public List<string> thanksSubsENG;
    [SerializeField] public List<string> chatterSubsENG;
    [SerializeField] public List<string> gasSubsENG;
    [SerializeField] public List<string> hitSubsENG;
    [SerializeField] public List<string> policeSubsENG;

    private Coroutine clearSubsCoroutine;

    private void Awake() {
        sm = SubsManager.current;
        sm.onSubsOnChanged += OnSubsOnChanged;
        sm.onLanguageChanged += OnLanguageChanged;
    }

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        subs = GetComponentInChildren<Target>();

        StartCoroutine(CheckSource());
    }

    private void Update() {
        if(!isLanguageReceived) {
            language = sm.language;
            isLanguageReceived = true;
        }
    }

    private IEnumerator CheckSource() {
        while(true) {
            if(audioSource.enabled && !audioSource.isPlaying) audioSource.enabled = false;

            yield return new WaitForSeconds(1f);
        }
    }

    public void Say(string sayType) {
        print("saying: " + sayType);
        if(audioSource && audioSource.isPlaying) return;
        
        List<AudioClip> audios = new List<AudioClip>();
        int subsIndex = 0;

        if(sayType == "Pay") {
            audios = payAudios;
            subsIndex = 0;
        } else if(sayType == "Change") {
            audios = changeAudios;
            subsIndex = 1;
        } else if(sayType == "Stop") {
            audios = stopAudios;
            subsIndex = 2;
        } else if(sayType == "Drop") {
            audios = dropAudios;
            subsIndex = 3;
        }  else if(sayType == "Thanks") {
            audios = thanksAudios;
            subsIndex = 4;
        } else if(sayType == "Death") {
            audios = deathAudios;
            subsIndex = 5;
        } else if(sayType == "Chatter") {
            audios = chatterAudios;
            subsIndex = 6;
        } else if(sayType == "GasStation") {
            audios = gasStationAudios;
            subsIndex = 7;
        } else if(sayType == "Hit") {
            audios = hitAudios;
            subsIndex = 8;
        } else if(sayType == "Police") {
            audios = policeAudios;
            subsIndex = 9;
        }

        int voiceLineIndex = GetRandomIndex(audios.Count);

        Play(audios, voiceLineIndex);
        float length = 3;
        if(voiceLineIndex < audios.Count || audios.Count != 0) length = audios[voiceLineIndex].length;

        if(subtitlesOn && subs != null) SetSubtitles(subsIndex, voiceLineIndex, length);
    }

    public void SetAudioClips(VoiceType voiceType) {
        // print("setting audio clips for " + voiceType.voiceName);
        //MAIN
        voiceName = voiceType.voiceName;
        payAudios = voiceType.payAudios;
        changeAudios = voiceType.changeAudios;
        stopAudios = voiceType.stopAudios;
        dropAudios = voiceType.dropAudios;
        deathAudios = voiceType.deathAudios;

        //OPTIONAL
        thanksAudios = voiceType.thanksAudios;
        chatterAudios = voiceType.chatterAudios;
        gasStationAudios = voiceType.gasStationAudios;
        hitAudios = voiceType.hitAudios;
        policeAudios = voiceType.policeAudios;

        //SUBS: ORIGINAL
        paySubsORIG = voiceType.paySubsORIG;
        changeSubsORIG = voiceType.changeSubsORIG;
        stopSubsORIG = voiceType.stopSubsORIG;
        dropSubsORIG = voiceType.dropSubsORIG;
        deathSubsORIG = voiceType.deathSubsORIG;
        thanksSubsORIG = voiceType.thanksSubsORIG;
        chatterSubsORIG = voiceType.chatterSubsORIG;
        gasSubsORIG = voiceType.gasSubsORIG;
        hitSubsORIG = voiceType.hitSubsORIG;
        policeSubsORIG = voiceType.policeSubsORIG;

        //SUBS: ENGLISH
        paySubsENG = voiceType.paySubsENG;
        changeSubsENG = voiceType.changeSubsENG;
        stopSubsENG = voiceType.stopSubsENG;
        dropSubsENG = voiceType.dropSubsENG;
        deathSubsENG = voiceType.deathSubsENG;
        thanksSubsENG = voiceType.thanksSubsENG;
        chatterSubsENG = voiceType.chatterSubsENG;
        gasSubsENG = voiceType.gasSubsENG;
        hitSubsENG = voiceType.hitSubsENG;
        policeSubsENG = voiceType.policeSubsENG;
    }
    
    public void Play(List<AudioClip> audios, int i) {
        // print("PLAY VOICE");
        if(i >= audios.Count || audios.Count == 0) return;
        audioSource.enabled = true;

        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(audios[i]);
    }

    private void SetSubtitles(int subsIndex, int voiceLineIndex, float length) {
        print("SET SUBTITLES. Length: " + length);
        length += 2;
        List<string> subCategory = paySubsORIG; //SUBTITLE CATEGORY

        if(language == Language.OFF) return;

        else if(language == Language.ORIGINAL) {
            if(subsIndex == 0) subCategory = paySubsORIG;
            else if(subsIndex == 1) subCategory = changeSubsORIG;
            else if(subsIndex == 2) subCategory = stopSubsORIG;
            else if(subsIndex == 3) subCategory = dropSubsORIG;            
            else if(subsIndex == 4) subCategory = thanksSubsORIG;
            else if(subsIndex == 5) subCategory = deathSubsORIG;
            else if(subsIndex == 6) subCategory = chatterSubsORIG;
            else if(subsIndex == 7) subCategory = gasSubsORIG;
            else if(subsIndex == 8) subCategory = hitSubsORIG;
            else if(subsIndex == 9) subCategory = policeSubsORIG;
        }

        else if(language == Language.ENGLISH) {
            if(subsIndex == 0) subCategory = paySubsENG;
            else if(subsIndex == 1) subCategory = changeSubsENG;
            else if(subsIndex == 2) subCategory = stopSubsENG;
            else if(subsIndex == 3) subCategory = dropSubsENG;            
            else if(subsIndex == 4) subCategory = thanksSubsENG;
            else if(subsIndex == 5) subCategory = deathSubsENG;
            else if(subsIndex == 6) subCategory = chatterSubsENG;
            else if(subsIndex == 7) subCategory = gasSubsENG;
            else if(subsIndex == 8) subCategory = hitSubsENG;
            else if(subsIndex == 9) subCategory = policeSubsENG;
        }
        
        if(subCategory.Count == 0) return;

        // subs.SetText(voiceName.ToUpper() + "\n'" + subCategory[voiceLineIndex] + "'");
        subs.speaker = voiceName.ToUpper();
        subs.text = "''" + subCategory[voiceLineIndex] + "''";

        if(clearSubsCoroutine != null) StopCoroutine(ClearSubsIn(length));

        //Make sure it doesnt stay for too long
        if(length > 10) length = 10;
        clearSubsCoroutine = StartCoroutine(ClearSubsIn(length));
    }

    private IEnumerator ClearSubsIn(float duration) {
        print(gameObject.name + " CLEAR SUBS IN: " + duration);
        yield return new WaitForSeconds(duration);
        print(gameObject.name + " CLEARING SUBS: " + subs.text);
        subs.speaker = "";
        subs.text = "";
    }

    private int GetRandomIndex(int audioCount) {
        return Random.Range(0, audioCount);
    }

    private void OnSubsOnChanged() {
        subtitlesOn = sm.isSubsOn;
    }

    private void OnLanguageChanged(Language newLanguage) {
        print("on language changed: " + newLanguage);
        language = newLanguage;
    }
}