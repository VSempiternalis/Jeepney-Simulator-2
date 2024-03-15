using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioHandler : MonoBehaviour {
    private AudioSource source;

    [SerializeField] private float pitchRange;
    [SerializeField] private List<AudioClip> audios;

    private void Start() {
        source = GetComponent<AudioSource>();
        StartCoroutine(CheckSource());
    }

    //Every second, check if playing. If not, turn source off
    private IEnumerator CheckSource() {
        while(true) {
            if(source.enabled && !source.isPlaying) source.enabled = false;

            yield return new WaitForSeconds(1f);
        }
    }

    public void Play(int i) {
        if(i >= audios.Count) return;
        source.enabled = true;

        source.pitch = Random.Range(1f - pitchRange, 1f + pitchRange);
        // source.pitch = Random.Range(0.75f, 1.25f);
        source.clip = audios[i];
        source.Play();
        // source.PlayOneShot(audios[i]);
    }

    public void PlayOneShot(int i) {
        if(i < audios.Count) {
            source.enabled = true;
            
            source.pitch = Random.Range(1f - pitchRange, 1f + pitchRange);
            // source.pitch = Random.Range(0.75f, 1.25f);
            source.PlayOneShot(audios[i]);
        }
    }

    public void PlayRandom() {
        if(!source.isPlaying) {
            source.enabled = true;

            int i = Random.Range(0, audios.Count - 1);
            source.PlayOneShot(audios[i]);
        }
    }
}