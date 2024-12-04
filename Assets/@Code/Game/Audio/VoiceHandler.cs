using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoiceHandler : MonoBehaviour {
    private AudioSource audioSource;

    //should be clear on start, to be filled by person handler
    [Header("MAIN")]
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

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(CheckSource());
    }

    private IEnumerator CheckSource() {
        while(true) {
            if(audioSource.enabled && !audioSource.isPlaying) audioSource.enabled = false;

            yield return new WaitForSeconds(1f);
        }
    }

    public void Say(string sayType) {
        if(audioSource && audioSource.isPlaying) return;
        
        List<AudioClip> audios = new List<AudioClip>();

        if(sayType == "Pay") {
            audios = payAudios;
        } else if(sayType == "Change") {
            audios = changeAudios;
        } else if(sayType == "Stop") {
            audios = stopAudios;
        } else if(sayType == "Drop") {
            audios = dropAudios;
        }  else if(sayType == "Thanks") {
            audios = thanksAudios;
        } else if(sayType == "Death") {
            audios = deathAudios;
        } else if(sayType == "Chatter") {
            audios = chatterAudios;
        } else if(sayType == "GasStation") {
            audios = gasStationAudios;
        } else if(sayType == "Hit") {
            audios = hitAudios;
        } else if(sayType == "Police") {
            audios = policeAudios;
        }

        Play(audios, GetRandomIndex(audios.Count));
    }

    public void SetAudioClips(List<AudioClip> newPayAudios, List<AudioClip> newChangeAudios, List<AudioClip> newStopAudios, List<AudioClip> newDropAudios, List<AudioClip> newDeathAudios, List<AudioClip> newThanksAudios, List<AudioClip> newChatterAudios, List<AudioClip> newGasStationAudios, List<AudioClip> newHitAudios, List<AudioClip> newPoliceAudios) {
        payAudios = newPayAudios;
        changeAudios = newChangeAudios;
        stopAudios = newStopAudios;
        dropAudios = newDropAudios;
        deathAudios = newDeathAudios;

        thanksAudios = newThanksAudios;
        chatterAudios = newChatterAudios;
        gasStationAudios = newGasStationAudios;
        hitAudios = newHitAudios;
        policeAudios = newPoliceAudios;
    }
    
    public void Play(List<AudioClip> audios, int i) {
        if(i >= audios.Count || audios.Count == 0) return;
        audioSource.enabled = true;

        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(audios[i]);
    }

    private int GetRandomIndex(int audioCount) {
        return Random.Range(0, audioCount);
    }
}