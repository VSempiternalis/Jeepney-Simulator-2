using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {
    public static AudioManager current;
    [SerializeField] private AudioHandler uiHandler;
    [SerializeField] private AudioSource fuelPumpAudio;

    //POOL
    // [SerializeField] private int poolSize = 10;
    // [SerializeField] private List<AudioSource> audioSources = new List<AudioSource>();

    //Ambience
    private AudioHandler oldAmbience;
    private AudioSource oldAmbSource;
    private AudioHandler currentAmbience;
    private AudioSource currentAmbSource;
    private int currentIndex; //song/audio index
    public int currentAmbIndex; //audiohandler and source index
    [SerializeField] private List<AudioHandler> ambienceHandlers;
    [SerializeField] private List<AudioSource> ambSources;
    // private bool isTransitioning;
    // [SerializeField] private float smoothness;
    [SerializeField] private float maxAmbVolume;
    [SerializeField] private float transitionTime;

    public bool isSayingStop;

    private void Awake() {
        current = this;
    }

    private void Start() {
        currentIndex = 0;
        currentAmbience = ambienceHandlers[currentAmbIndex];
        currentAmbSource = ambSources[currentAmbIndex];
    }

    private void Update() {
        
    }

    public void SayStop() {
        isSayingStop = true;
        StartCoroutine(ResetSayStop(3f));
    }

    IEnumerator ResetSayStop(float delay) {
        yield return new WaitForSeconds(delay);
        isSayingStop = false;
    }

    public void PlayUI(int i) {
        uiHandler.PlayOneShot(i);
    }

    public void PlayAmb(int i) {
        // print("Playing ambience: " + i);
        // ambienceHandler.Play(i);
    }

    public void NewAmb(int i) {
        if(i == currentIndex) return;
        currentIndex = i;

        // print("NEW AMB: " + i);
        //OLD AMBIENCE
        //slowly decrease volume from 0.4 to 0
        // currentAmbSource.volume
        LeanTween.value(gameObject, maxAmbVolume, 0f, transitionTime)
        .setOnUpdate((float val)=> {
            // print("old val: " + val);
            if(oldAmbSource != null) {
                oldAmbSource.volume = val;
                // print("old vol: " + oldAmbSource.volume);
            }
        });
        // .setOnComplete(() => {
        //     ambSources[currentAmbIndex].Stop();
        // });

        //NEW AMBIENCE
        if(currentAmbIndex == 0) currentAmbIndex = 1;
        else currentAmbIndex = 0;

        currentAmbience = ambienceHandlers[currentAmbIndex];
        currentAmbSource = ambSources[currentAmbIndex];

        //slowly increase volume from 0 to 0.4
        currentAmbience.Play(currentIndex);
        LeanTween.value(gameObject, 0, maxAmbVolume, transitionTime)
        .setOnUpdate((float val)=> {
            // print("new val: " + val);
            currentAmbSource.volume = val;
            // print("new vol: " + currentAmbSource.volume);
        })
        .setOnComplete(() => {
            oldAmbience = currentAmbience;
            oldAmbSource = currentAmbSource;
        });
    }

    public void PlayFuelPump(bool newVal) {
        if(newVal && !fuelPumpAudio.isPlaying) fuelPumpAudio.Play();
        else fuelPumpAudio.Stop();
    }
}
