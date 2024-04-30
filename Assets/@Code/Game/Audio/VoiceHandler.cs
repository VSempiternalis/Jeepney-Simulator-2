using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoiceHandler : MonoBehaviour {
    private AudioSource audioSource;

    //should be clear on start, to be filled by person handler
    [SerializeField] public List<AudioClip> payAudios;
    [SerializeField] public List<AudioClip> changeAudios;
    [SerializeField] public List<AudioClip> stopAudios;
    [SerializeField] public List<AudioClip> dropAudios;
    [SerializeField] public List<AudioClip> deathAudios;

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
        if(audioSource.isPlaying) return;
        
        List<AudioClip> audios = new List<AudioClip>();

        if(sayType == "Pay") {
            audios = payAudios;
        } else if(sayType == "Change") {
            audios = changeAudios;
        } else if(sayType == "Stop") {
            audios = stopAudios;
        } else if(sayType == "Drop") {
            audios = dropAudios;
        } else if(sayType == "Death") {
            audios = deathAudios;
        }

        Play(audios, GetRandomIndex(audios.Count));
    }

    public void SetAudioClips(List<AudioClip> newPayAudios, List<AudioClip> newChangeAudios, List<AudioClip> newStopAudios, List<AudioClip> newDropAudios, List<AudioClip> newDeathAudios) {
        payAudios = newPayAudios;
        changeAudios = newChangeAudios;
        stopAudios = newStopAudios;
        dropAudios = newDropAudios;
        deathAudios = newDeathAudios;
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