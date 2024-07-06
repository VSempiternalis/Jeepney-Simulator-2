using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class MusicPlayer : MonoBehaviour {
    public bool isOn;

    public bool isPlaying;
    public string audioName;
    private int songIndex = 0;
    public float volume;
    public bool isShuffling;
    // private float stopTime;

    private string[] songs; // Get all MP3 files in the default directory
    private List<AudioClip> audioClips = new List<AudioClip>();

    private string songDirectory;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private TMP_Text directoryText;
    [SerializeField] private Link openSongsFolderButton;
    [SerializeField] private GameObject shuffleOnButton;
    [SerializeField] private TMP_Text songNameText;
    [SerializeField] private GameObject musicPlayerOnSign;

    [SerializeField] private AudioClip[] sfx;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        // print("Songs folder: " + Application.persistentDataPath);
        // directoryText.text = Application.persistentDataPath;

        // Create the folder if it doesn't exist
        songDirectory = Path.Combine(Application.persistentDataPath, "Songs");
        if (!Directory.Exists(songDirectory)) {
            Directory.CreateDirectory(songDirectory);
            // Debug.Log("Songs folder created: " + songDirectory);
            if(directoryText) directoryText.text = songDirectory;
        } else {
            // Debug.Log("Songs folder found: " + songDirectory);
            if(directoryText) directoryText.text = songDirectory;
            // C:\ Users\PC\AppData\LocalLow\Spacezero Interactive\Jeepney Simulator 2\Songs
        }
        songs = Directory.GetFiles(songDirectory);
        // songs = Directory.GetFiles(songDirectory, "*.ogg");
        // print("Length: " + songs.Length);
        // print(songs[0]);

        //Update 'open song folder' button link
        openSongsFolderButton.url = songDirectory;

        //populate audioclips list
        foreach(string song in songs) {
            audioClips.Add(LoadAudioClip(song));
        }


    }

    private void Update() {
        if(isOn && isPlaying && audioSource.clip && audioSource.time >= audioSource.clip.length) { // && audioSource.isPlaying
            PlayNext();
        }
    }

    public void SetActive(bool isActive) {
        // audioSource.enabled = true;
        isOn = isActive;
        musicPlayerOnSign.SetActive(isActive);

        // if(!isActive) audioSource.Stop();
        audioSource.enabled = isActive;
    }

    public void NextPrev(bool isNext) {
        if(!isOn) return;
        if(songIndex > songs.Length) return;

        if(isNext) PlayNext();
        else {
            if(songIndex == 0) songIndex = songs.Length - 1;
            else songIndex --;

            PlayCurrentIndex();
        }

        //SFX
        audioSource.PlayOneShot(sfx[1]);
    }

    public void TogglePlay() {
        if(!isOn) return;

        isPlaying = !isPlaying;

        if(isPlaying && audioSource.clip == null) PlayNext();
        else if(isPlaying) PlayCurrentIndex();
        else audioSource.Stop();

        //SFX
        audioSource.PlayOneShot(sfx[0]);
    }

    public void ToggleShuffle() {
        if(!isOn) return;

        isShuffling = !isShuffling;
        shuffleOnButton.SetActive(isShuffling);

        //SFX
        audioSource.PlayOneShot(sfx[0]);
    }

    public void AddVolume(float add) {
        if(!isOn) return;

        if((add > 0f && volume < 10f) || (add < 0f && volume > 0f)) {
            print("Adding volume: " + add);
            volume += add;
            // audioSource.volume += add*0.01f;
            audioSource.volume += add*0.1f;

            //SFX
            audioSource.PlayOneShot(sfx[1]);
        }
    }

    private void PlayNext() {
        if(!isOn) return;
        if(songIndex > songs.Length) return;

        if(isShuffling) {
            int oldIndex = songIndex;
            while(true) {
                songIndex = UnityEngine.Random.Range(0, songs.Length);

                if(songIndex != oldIndex) break;
            }
        }
        else {
            if(songIndex == songs.Length - 1) songIndex = 0;
            else songIndex ++;
        }

        PlayCurrentIndex();
    }

    private void PlayCurrentIndex() {
        // AudioClip songClip;
        // songClip = LoadAudioClip(songs[songIndex]);
        // songClip = audioClips[songIndex];
        audioSource.clip = audioClips[songIndex];
        string fileName = Path.GetFileNameWithoutExtension(songs[songIndex]);
        // audioName = fileName;
        songNameText.text = fileName;

        audioSource.Play();
    }

    private AudioClip LoadAudioClip(string filePath) {
        // print("LOADING AUDIO CLIP: " + filePath);
        WWW www = new WWW("file://" + filePath);
        // UnityWebRequest www = new UnityWebRequest("file://" + filePath);
        while (!www.isDone) { }

        AudioClip audioClip = www.GetAudioClip(false, false);

        return audioClip;
    }
}
