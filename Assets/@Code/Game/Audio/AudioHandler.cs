using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioHandler : MonoBehaviour {
    private AudioSource audioSource;

    [SerializeField] private List<AudioClip> audios;

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

    public void Play(int i) {
        if(i >= audios.Count) return;
        audioSource.enabled = true;
        //audioSource.Stop();

        audioSource.pitch = Random.Range(0.75f, 1.25f);
        audioSource.PlayOneShot(audios[i]);
    }
}
