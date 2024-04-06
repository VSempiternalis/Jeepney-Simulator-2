using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoiceHandler : MonoBehaviour {
    private AudioSource audioSource;

    [SerializeField] private List<AudioClip> payAudios;
    [SerializeField] private List<AudioClip> stopAudios;
    [SerializeField] private List<AudioClip> deathAudios;

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
        print("Saying: " + sayType);
        if(audioSource.isPlaying) return;
        
        List<AudioClip> audios = new List<AudioClip>();

        if(sayType == "Pay") {
            audios = payAudios;
        } else if(sayType == "Stop") {
            print("Saying stop");
            audios = stopAudios;
        } else if(sayType == "Death") {
            audios = deathAudios;
        }

        Play(audios, GetRandomIndex(audios));
    }

    public void SetAudioClips(List<AudioClip> newPayAudios, List<AudioClip> newStopAudios, List<AudioClip> newDeathAudios) {
        payAudios = newPayAudios;
        stopAudios = newStopAudios;
        deathAudios = newDeathAudios;
    }
    
    public void Play(List<AudioClip> audios, int i) {
        if(i >= audios.Count) return;
        audioSource.enabled = true;

        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(audios[i]);
    }

    private int GetRandomIndex(List<AudioClip> audios) {
        return Random.Range(0, audios.Count-1);
    }
}