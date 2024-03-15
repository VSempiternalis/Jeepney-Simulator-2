using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System;

public class Settings : MonoBehaviour {
    public AudioMixer audioMixer;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerCam;
    private bool isCursorOn;
    [SerializeField] private uiAnimGroup settingsPanel;

    [Space(10)]
    [Header("AUDIO")]
    [SerializeField] private Slider masterVolSlider;
    [SerializeField] private TMP_Text masterVolumeText;

    [Space(10)]
    [Header("VIDEO")]
    private Resolution[] resolutions;
    private int currentResolutionIndex;
    private List<string> resolutionOptions;

    [SerializeField] private RenderPipelineAsset[] qualityLevels;
    [SerializeField] private TMP_Dropdown graphicsDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle fogToggle;
    [SerializeField] private GameObject ppVolume;
    [SerializeField] private Toggle ppToggle; //post processing
    [SerializeField] private Light sun;
    [SerializeField] private Toggle shadowsToggle; //post processing
    [SerializeField] private GameObject reflectionProbe;
    [SerializeField] private Toggle reflectionProbeToggle; //post processing
    [SerializeField] private TMP_Text reflectionProbeSizeText;
    [SerializeField] private Slider reflectionProbeSizeSlider;
    [SerializeField] private TMP_Text fovText;
    [SerializeField] private Slider fovSlider;

    [Space(10)]
    [Header("GAME")]
    [SerializeField] private Slider zoomSensSlider;
    [SerializeField] private TMP_Text zoomSensText;
    [SerializeField] private Slider mouseSensSlider;
    [SerializeField] private TMP_Text mouseSensText;
    [SerializeField] private Slider renderDistSlider;
    [SerializeField] private TMP_Text renderDistText;
    [SerializeField] private Slider spawnDistSlider;
    [SerializeField] private TMP_Text spawnDistText;

    private void Start() {
        resolutions = Screen.resolutions;

        // Add Resolutions
        resolutionOptions = new List<string>();
        for(int i = 0; i < resolutions.Length; i++) {
            string resolution = resolutions[i].width + " x " + resolutions[i].height;
            resolutionOptions.Add(resolution);
        }

        // Populate resolution dropdown
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        // resolutionDropdown.value = currentResolutionIndex - 1;
        resolutionDropdown.RefreshShownValue();

        // PlayerPrefs.SetInt("IsNewPlay", 0);

        // If new player, set settings to default
        if(PlayerPrefs.GetInt("IsNewPlay") == 0) {
            print("NEW PLAYER");
            currentResolutionIndex = 0;
            currentResolutionIndex = resolutionOptions.Count - 1;

            LoadDefaultSettings();

            PlayerPrefs.SetInt("IsNewPlay", 1);
        }
        // If old player, load saved settings
        else {
            print("OLD PLAYER");
            LoadSavedSettings();
        }
    }

    private void Update() {
        //Toggle Settings
        if(Input.GetKeyDown(KeyCode.Escape)) {
            ToggleSettings();
        }
    }

    public void LoadDefaultSettings() {
        print("[SETTINGS] Loading default settings");
        SetMasterVolume(0);
        // SetMusic
        
        SetGraphicsPreset(2);
        SetResolution(resolutions.Length - 1);
        SetFullscreen(true);
        SetFog(false);
        SetPP(true);
        SetShadows(true);
        SetReflectionProbe(false);
        SetReflectionProbeSize(3f);
        SetFOV(80);

        SetMouseSens(2);
        SetZoomSens(3);
        SetRenderDist(5);
        SetSpawnDistance(100);
        //SetTutorialUI
    }

    private void LoadSavedSettings() {
        print("[SETTINGS] Loading saved settings");
        float volume = PlayerPrefs.GetFloat("Settings_MasterVolume");
        SetMasterVolume(volume);

        int qualityIndex = PlayerPrefs.GetInt("Settings_GraphicsPreset");
        SetGraphicsPreset(qualityIndex);
        // SetFullscreen((PlayerPrefs.GetInt("Settings_IsFullscreen") == 1 ? true : false));
        int resolutionIndex = PlayerPrefs.GetInt("Settings_ResolutionIndex");
        SetResolution(resolutionIndex);
        SetFog(PlayerPrefs.GetInt("Settings_ActiveFog") == 1? true : false);
        SetPP(PlayerPrefs.GetInt("Settings_ActivePP") == 1? true : false);
        SetShadows(PlayerPrefs.GetInt("Settings_ActiveShadows") == 1? true : false);
        SetReflectionProbe(PlayerPrefs.GetInt("Settings_ReflectionProbe") == 1? true : false);
        SetReflectionProbeSize(PlayerPrefs.GetFloat("Settings_ReflectionProbeSize"));
        SetFOV(PlayerPrefs.GetFloat("Settings_FOV"));

        float mouseSens = PlayerPrefs.GetFloat("Settings_MouseSens", 0.3f);
        SetMouseSens(mouseSens);
        float zoomSens = PlayerPrefs.GetFloat("Settings_ZoomSens", 3f);
        SetZoomSens(zoomSens);
        float renderDist = PlayerPrefs.GetFloat("Settings_RenderDist", 5);
        SetRenderDist(renderDist);
        int spawnDist = PlayerPrefs.GetInt("Settings_SpawnDist", 100);
        SetSpawnDistance(100);
    }

    public void ToggleSettings() {
        isCursorOn = !isCursorOn;
        if(isCursorOn) settingsPanel.In();
        else settingsPanel.Out();
        ToggleCursor();
    }

    public void ToggleCursor() {
        if(player.transform.parent != null) player.GetComponent<Rigidbody>().isKinematic = true; //Player in driverpos
        // else player.GetComponent<Rigidbody>().isKinematic = isCursorOn;
        // playerCam.GetComponent<FirstPersonLook>().isOn = !isCursorOn;
        playerCam.GetComponent<FirstPersonLook>().enabled = !isCursorOn;
        playerCam.GetComponent<Zoom>().enabled = !isCursorOn;
        Cursor.visible = isCursorOn;

        if(isCursorOn) Cursor.lockState = CursorLockMode.None;
        else Cursor.lockState = CursorLockMode.Locked;
        // else Cursor.lockState = CursorLockMode.Locked;
    }

    #region AUDIO

    public void SetMasterVolume(float newVol) {
        audioMixer.SetFloat("MasterVolume", newVol);
        masterVolumeText.text = (newVol + 80) + "";
        masterVolSlider.value = newVol;

        PlayerPrefs.SetFloat("Settings_MasterVolume", newVol);
    }

    #endregion

    #region VIDEO

    public void SetGraphicsPreset(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);

        PlayerPrefs.SetInt("Settings_GraphicsPreset", qualityIndex);

        graphicsDropdown.value = qualityIndex;
    }

    public void SetResolution(int resolutionIndex) {
        currentResolutionIndex = resolutionIndex;
        Resolution resolution = resolutions[currentResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        //Saving
        PlayerPrefs.SetInt("Settings_ResolutionIndex", resolutionIndex);

        //Update UI
        resolutionDropdown.value = resolutionIndex;
    }

    public void SetFullscreen(bool newIsFullscreen) {
        Screen.fullScreen = newIsFullscreen;
        fullscreenToggle.isOn = newIsFullscreen;
    }

    public void SetFog(bool isFog) {
        RenderSettings.fog = isFog;

        //Saving
        // print("SAVING Fog: " + isFog);
        PlayerPrefs.SetInt("Settings_ActiveFog", isFog? 1:0);

        //Update UI
        fogToggle.isOn = isFog;
    }

    public void SetPP(bool isPP) {
        // ppVolume.SetActive(isPP);
        ppVolume.GetComponent<Volume>().enabled = isPP;

        //Saving
        // print("SAVING PP: " + isPP);
        PlayerPrefs.SetInt("Settings_ActivePP", isPP? 1:0);

        //Update UI
        ppToggle.isOn = isPP;
    }

    public void SetShadows(bool isOn) {
        sun.shadows = isOn? LightShadows.Hard : LightShadows.None;
        // foreach(Camera cam in GameObject.FindObjectsOfType<Camera>()) {
        //     cam.allowHDR = isOn;
        //     cam.allowMSAA = isOn;    
        // }

        //Saving
        PlayerPrefs.SetInt("Settings_ActiveShadows", isOn? 1:0);

        //Update UI
        shadowsToggle.isOn = isOn;
    }

    public void SetReflectionProbe(bool isOn) {
        reflectionProbe.SetActive(isOn);

        //Saving
        PlayerPrefs.SetInt("Settings_ReflectionProbe", isOn? 1:0);

        //Update UI
        reflectionProbeToggle.isOn = isOn;
    }

    public void SetReflectionProbeSize(float sizeIndex) {
        int realSize = (int)Mathf.Pow(2, sizeIndex+4);
        // print("Setting size: " + realSize);
        reflectionProbe.GetComponent<ReflectionProbe>().resolution = realSize;
        reflectionProbe.GetComponent<ReflectionProbe>().enabled = false;
        reflectionProbe.GetComponent<ReflectionProbe>().enabled = true;

        //Saving
        PlayerPrefs.SetFloat("Settings_ReflectionProbeSize", sizeIndex);

        //Update UI
        reflectionProbeSizeText.text = realSize.ToString();
        reflectionProbeSizeSlider.value = sizeIndex;
    }

    public void SetFOV(float newFOV) {
        if(!playerCam) return;
        // print("Setting fov to: " + newFOV);
        playerCam.GetComponent<Zoom>().defaultFOV = newFOV;
        playerCam.GetComponent<Camera>().fieldOfView = newFOV;
        fovText.text = newFOV.ToString();
        fovSlider.value = newFOV;

        //Saving
        PlayerPrefs.SetFloat("Settings_FOV", newFOV);
        // print("Saving FOV from: " + newFOV + " to: " + PlayerPrefs.GetFloat(""));
    }

    #endregion

    #region GAME

    public void SetMouseSens(float newMouseSens) {
        if(!playerCam) return;
        playerCam.GetComponent<FirstPersonLook>().sensitivity = newMouseSens;
        mouseSensText.text = Mathf.Round(newMouseSens*10.0f) * 0.01f + "";
        mouseSensSlider.value = newMouseSens;

        //Saving
        PlayerPrefs.SetFloat("Settings_MouseSens", newMouseSens);
    }

    public void SetZoomSens(float newZoomSens) {
        if(!playerCam) return;
        playerCam.GetComponent<Zoom>().sensitivity = newZoomSens;
        zoomSensText.text = newZoomSens.ToString();
        zoomSensSlider.value = newZoomSens;

        //Saving
        PlayerPrefs.SetFloat("Settings_ZoomSens", newZoomSens);
    }

    public void SetRenderDist(float newRenderDist) {
        if(!playerCam) return;
        float renderDist = newRenderDist*100f;
        renderDistSlider.value = newRenderDist;

        playerCam.GetComponent<Camera>().farClipPlane = renderDist;
        renderDistText.text = renderDist.ToString();

        //Saving
        PlayerPrefs.SetFloat("Settings_RenderDist", newRenderDist);
    }

    public void SetSpawnDistance(float newSpawnDist) {
        int spawnDist = Mathf.RoundToInt(newSpawnDist);

        //Save
        PlayerPrefs.SetInt("Settings_SpawnDist", spawnDist);

        //Update
        SpawnArea.current.spawnDist = spawnDist;
        // SpawnArea.current.GetComponent<SphereCollider>().radius = spawnDist;

        //Update UI
        spawnDistText.text = spawnDist + "";
        spawnDistSlider.value = spawnDist;
    }

    #endregion
}