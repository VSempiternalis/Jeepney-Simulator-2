using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class Settings : MonoBehaviour {
    public static Settings current;

    public AudioMixer audioMixer;
    public AudioMixer bgmMixer;
    public AudioMixer musicPlayerMixer;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerCam;
    private bool isCursorOn;
    [SerializeField] private uiAnimGroup settingsPanel;

    [Space(10)]
    [Header("AUDIO")]
    [SerializeField] private Slider masterVolSlider;
    [SerializeField] private TMP_Text masterVolumeText;
    [SerializeField] private Slider bgmVolSlider;
    [SerializeField] private TMP_Text bgmVolumeText;
    [SerializeField] private Slider musicPlayerVolSlider;
    [SerializeField] private TMP_Text musicPlayerVolumeText;

    [Space(10)]
    [Header("VIDEO")]
    private Resolution[] resolutions;
    private int currentResolutionIndex;
    private List<string> resolutionOptions;

    [SerializeField] private TMP_Text specsText;
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
    [SerializeField] private Toggle tutorialPanelsToggle;
    [SerializeField] private Toggle autoTransToggle;
    [SerializeField] private Slider renderDistSlider;
    [SerializeField] private TMP_Text renderDistText;
    [SerializeField] private Slider spawnDistSlider;
    [SerializeField] private TMP_Text spawnDistText;

    public bool isTutorialPanelsOn;
    [SerializeField] private CanvasGroup tutorialUI; //help/page at game start
    [SerializeField] private uiAnimGroup tutorialPanels;

    [SerializeField] private CarController carCon;

    private void Awake() {
        current = this;
    }

    private void Start() {
        //Set quality button text
        if(specsText) specsText.text = "CPU: " + SystemInfo.processorType + "\nRAM: " + SystemInfo.systemMemorySize + "MB\nGPU: " + SystemInfo.graphicsDeviceName + "\nVRAM: " + SystemInfo.graphicsMemorySize + " MB";

        resolutions = Screen.resolutions;

        // Add Resolutions
        resolutionOptions = new List<string>();
        for(int i = 0; i < resolutions.Length; i++) {
            string resolution = resolutions[i].width + " x " + resolutions[i].height + " (" + resolutions[i].refreshRate + "hz)";
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
        if(PlayerPrefs.GetInt("IsNewPlay", 0) == 0) {
            // print("NEW PLAYER");
            currentResolutionIndex = 0;
            currentResolutionIndex = resolutionOptions.Count - 1;

            LoadDefaultSettings();

            PlayerPrefs.SetInt("IsNewPlay", 1);
        }
        // If old player, load saved settings
        else {
            // print("OLD PLAYER");
            bool gameCrashed = PlayerPrefs.GetInt("GameCrashed", 0) == 1? true : false;
            if(gameCrashed) {
                // print("CRASHED");
                currentResolutionIndex = 0;
                currentResolutionIndex = resolutionOptions.Count - 1;

                LoadDefaultSettings();
            } else {
                // print("NO CRASH");
                LoadSavedSettings();
            }
        }
        
        //Reset GameCrash save
        PlayerPrefs.SetInt("GameCrashed", 0);
    }

    private void Update() {
        //Toggle Settings
        if(Input.GetKeyDown(KeyCode.Escape) && tutorialUI != null && tutorialUI.alpha < 1) {
            ToggleSettings();
        }
    }

    public void LoadDefaultSettings() {
        print("[SETTINGS] Loading default settings");
        SetMasterVolume(-10); //0 is full
        SetBGMVolume(-15);
        SetMusicPlayerVolume(-10);
        
        SetGraphicsPresetFromSpecs();
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
        SetTutorialPanels(true);
        SetAutoTrans(true);
        SetRenderDist(2);
        SetSpawnDistance(100);
    }

    private void LoadSavedSettings() {
        // print("[SETTINGS] Loading saved settings");
        float volume = PlayerPrefs.GetFloat("Settings_MasterVolume", 0);
        SetMasterVolume(volume);
        float bgmVolume = PlayerPrefs.GetFloat("Settings_BGMVolume", 0);
        SetBGMVolume(bgmVolume);
        float musicPlayerVolume = PlayerPrefs.GetFloat("Settings_MusicPlayerVolume", 0);
        SetMusicPlayerVolume(musicPlayerVolume);

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
        SetFOV(PlayerPrefs.GetFloat("Settings_FOV", 80));

        float mouseSens = PlayerPrefs.GetFloat("Settings_MouseSens", 0.3f);
        SetMouseSens(mouseSens);
        float zoomSens = PlayerPrefs.GetFloat("Settings_ZoomSens", 3f);
        SetZoomSens(zoomSens);
        SetTutorialPanels(PlayerPrefs.GetInt("Settings_IsTutorialPanels", 1) == 1? true : false);
        SetAutoTrans(PlayerPrefs.GetInt("Settings_IsAutomaticTransmission", 1) == 1? true : false);
        float renderDist = PlayerPrefs.GetFloat("Settings_RenderDist", 5);
        SetRenderDist(renderDist);
        int spawnDist = PlayerPrefs.GetInt("Settings_SpawnDist", 100);
        SetSpawnDistance(100);
    }

    public void ToggleCursor() {
        isCursorOn = !isCursorOn;
    }

    public void ToggleSettings() {
        ToggleCursor();
        if(isCursorOn) {
            settingsPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
            settingsPanel.In();
        } else {
            settingsPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            settingsPanel.Out();
        }
        UpdateCursor();

        SetPlayer(!isCursorOn);
    }

    public void SetPlayer(bool isOn) {
        //pause shift
        TimeManager.current.isPauseShift = !isOn;

        //stop movement
        if(player.GetComponent<PlayerDriveInput>() && !player.GetComponent<PlayerDriveInput>().isDriving) {
            player.GetComponent<FirstPersonMovement>().enabled = isOn;
            player.GetComponent<Jump>().enabled = isOn;
            player.GetComponent<Crouch>().enabled = isOn;
        }
        if(player.GetComponent<PlayerInteraction>()) player.GetComponent<PlayerInteraction>().enabled = isOn;
        if(player.GetComponent<PlayerDriveInput>()) player.GetComponent<PlayerDriveInput>().enabled = isOn;
    }

    public void UpdateCursor() {
        if(player.transform.parent != null) player.GetComponent<Rigidbody>().isKinematic = true; //Player in driverpos
        playerCam.GetComponent<FirstPersonLook>().enabled = !isCursorOn;
        playerCam.GetComponent<Zoom>().enabled = !isCursorOn;
        Cursor.visible = isCursorOn;

        if(isCursorOn) Cursor.lockState = CursorLockMode.None;
        else Cursor.lockState = CursorLockMode.Locked;
    }

    #region AUDIO

    public void SetMasterVolume(float newVol) {
        audioMixer.SetFloat("MasterVolume", newVol);
        // masterVolumeText.text = ((newVol + 80)) + "";
        masterVolumeText.text = (newVol + 100) + "";
        // masterVolumeText.text = ((newVol + 25) * 4) + "";
        masterVolSlider.value = newVol;

        PlayerPrefs.SetFloat("Settings_MasterVolume", newVol);
    }

    public void SetBGMVolume(float newVol) {
        bgmMixer.SetFloat("BGMVolume", newVol);
        // bgmVolumeText.text = ((newVol + 80)) + "";
        bgmVolumeText.text = (newVol + 100) + "";
        bgmVolSlider.value = newVol;

        PlayerPrefs.SetFloat("Settings_BGMVolume", newVol);
    }

    public void SetMusicPlayerVolume(float newVol) {
        musicPlayerMixer.SetFloat("MusicPlayerVolume", newVol);
        // musicPlayerVolumeText.text = ((newVol + 80)) + "";
        musicPlayerVolumeText.text = (newVol + 100) + "";
        musicPlayerVolSlider.value = newVol;

        PlayerPrefs.SetFloat("Settings_MusicPlayerVolume", newVol);
    }

    #endregion

    #region VIDEO

    public void SetGraphicsPresetFromSpecs() {
        //Graphics preset
        if(SystemInfo.graphicsMemorySize <= 1024) SetGraphicsPreset(0);
        else if(SystemInfo.graphicsMemorySize <= 2048) SetGraphicsPreset(1);
        else if(SystemInfo.graphicsMemorySize <= 4096) SetGraphicsPreset(2);
        else SetGraphicsPreset(3);
        
        //Render Dist
        if(SystemInfo.graphicsMemorySize <= 1024) SetRenderDist(1);
        else if(SystemInfo.graphicsMemorySize <= 2048) SetRenderDist(2);
        else if(SystemInfo.graphicsMemorySize <= 4096) SetRenderDist(3);
        else SetRenderDist(4);
    }

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

    public void SetTutorialPanels(bool newValue) {
        if(tutorialPanels == null) return;

        isTutorialPanelsOn = newValue;

        if(newValue) tutorialPanels.In();
        else tutorialPanels.Out();

        tutorialPanelsToggle.isOn = newValue;

        //Saving
        // print("SAVING Fog: " + isFog);
        PlayerPrefs.SetInt("Settings_IsTutorialPanels", newValue? 1:0);
    }

    public void SetAutoTrans(bool newValue) {
        if(carCon == null) return;

        carCon.isAutoTrans = newValue;

        autoTransToggle.isOn = newValue;

        //Saving
        // print("SAVING Fog: " + isFog);
        PlayerPrefs.SetInt("Settings_IsAutomaticTransmission", newValue? 1:0);
    }

    public void SetRenderDist(float newRenderDist) {
        //float newRenderDist 1 = 100, 2 = 200, etc

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

    private void OnApplicationQuit() {
        // Set the crash flag if the application is quitting unexpectedly
        if (Application.platform != RuntimePlatform.WindowsEditor && !Application.isEditor) {
            PlayerPrefs.SetInt("GameCrashed", 1);
            PlayerPrefs.Save();

            //STEAM ACH
            // SteamAchievements.current.UnlockAchievement("ACH_HOUSTON");
        }
    }
}