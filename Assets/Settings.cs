using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class Settings : MonoBehaviour {
    public AudioMixer audioMixer;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerCam;
    private bool isCursorOn;
    [SerializeField] private uiAnimGroup settingsPanel;

    [Space(10)]
    [Header("AUDIO")]
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
    [SerializeField] private TMP_Text fovText;

    [Space(10)]
    [Header("GAME")]
    [SerializeField] private TMP_Text zoomSensText;
    [SerializeField] private TMP_Text mouseSensText;

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
    }

    private void Update() {
        //Toggle Settings
        if(Input.GetKeyDown(KeyCode.Escape)) {
            ToggleSettings();
        }
    }

    public void ToggleSettings() {
        isCursorOn = !isCursorOn;
        if(isCursorOn) settingsPanel.In();
        else settingsPanel.Out();
        ToggleCursor();
    }

    public void ToggleCursor() {
        if(player.transform.parent != null) player.GetComponent<Rigidbody>().isKinematic = true; //Player in driverpos
        else player.GetComponent<Rigidbody>().isKinematic = isCursorOn;
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

        PlayerPrefs.SetFloat("Settings_MasterVolume", newVol);
    }

    #endregion

    #region VIDEO

    public void SetGraphicsPreset(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);

        PlayerPrefs.SetInt("Settings_GraphicsPreset", qualityIndex);
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
        print("SAVING Fog: " + isFog);
        PlayerPrefs.SetInt("Settings_ActiveFog", isFog? 1:0);

        //Update UI
        fogToggle.isOn = isFog;
    }

    public void SetPP(bool isPP) {
        ppVolume.SetActive(isPP);

        //Saving
        print("SAVING PP: " + isPP);
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

    public void SetFOV(float newFOV) {
        playerCam.GetComponent<Zoom>().defaultFOV = newFOV;
        fovText.text = newFOV.ToString();

        //Saving
        PlayerPrefs.SetFloat("Settings_FOV", newFOV);
    }

    #endregion

    #region GAME

    public void SetMouseSens(float newMouseSens) {
        playerCam.GetComponent<FirstPersonLook>().sensitivity = newMouseSens;
        mouseSensText.text = Mathf.Round(newMouseSens*10.0f) * 0.01f + "";

        //Saving
        PlayerPrefs.SetFloat("Settings_MouseSens", newMouseSens);
    }

    public void SetZoomSens(float newZoomSens) {
        playerCam.GetComponent<Zoom>().sensitivity = newZoomSens;
        zoomSensText.text = newZoomSens.ToString();

        //Saving
        PlayerPrefs.SetFloat("Settings_ZoomSens", newZoomSens);
    }

    #endregion
}