using UnityEngine;
using System.Collections;
// using System.Collections.Generic;

public class AudioManager : MonoBehaviour {
    public static AudioManager current;
    [SerializeField] private AudioHandler uiHandler;
    [SerializeField] private AudioSource fuelPumpAudio;

    //POOL
    // [SerializeField] private int poolSize = 10;
    // [SerializeField] private List<AudioSource> audioSources = new List<AudioSource>();

    //Ambience
    private AudioHandler oldAmbience;
    private AudioHandler currentAmbience;
    private int currentIndex;
    [SerializeField] private AudioHandler ambienceHandler1;
    [SerializeField] private AudioHandler ambienceHandler2;
    private bool isTransitioning;
    [SerializeField] private float smoothness;
    [SerializeField] private float maxAmbVolume;

    private void Awake() {
        current = this;
    }

    private void Start() {
        currentAmbience = ambienceHandler1;
        // InitializePool();
    }

    private void Update() {
        // if(isTransitioning) {
        //     oldAmbience.GetComponent<AudioSource>().volume = Mathf.Lerp(oldAmbience.GetComponent<AudioSource>().volume, 0, Time.deltaTime * smoothness);
        //     currentAmbience.GetComponent<AudioSource>().volume = Mathf.Lerp(oldAmbience.GetComponent<AudioSource>().volume, maxAmbVolume, Time.deltaTime * smoothness);
            
        //     if(oldAmbience.GetComponent<AudioSource>().volume == 0 && currentAmbience.GetComponent<AudioSource>().volume == maxAmbVolume) {
        //         oldAmbience.GetComponent<AudioSource>().enabled = false;
        //         isTransitioning = false;
        //     }
        // }
    }

    // private void InitializePool() {
    //     for(int i = 0; i < poolSize; i++) {
    //         AudioSource source = gameObject.AddComponent<AudioSource>();
    //         audioSources.Add(source);
    //     }
    // }

    public void PlayUI(int i) {
        uiHandler.PlayOneShot(i);
    }

    public void PlayAmb(int i) {
        // print("Playing ambience: " + i);
        // ambienceHandler.Play(i);
    }

    public void NewAmb(int i) {
        if(currentIndex == i) return;
        currentIndex = i;
        currentAmbience.Play(currentIndex);

        //OLD
        // //Slowly decrease volume of previous ambience
        // oldAmbience = currentAmbience;

        // //set new ambience
        // if(currentAmbience == ambienceHandler1) currentAmbience = ambienceHandler2;
        // else currentAmbience = ambienceHandler1;

        // currentAmbience.GetComponent<AudioSource>().enabled = true;
        // currentAmbience.Play(i);

        // isTransitioning = true;
    }

    public void PlayFuelPump(bool newVal) {
        if(newVal) fuelPumpAudio.Play();
        else fuelPumpAudio.Stop();
    }
}
