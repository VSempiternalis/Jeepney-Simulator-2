using UnityEngine;

public class JeepneySLS : MonoBehaviour {
    //Info: "Jeepney Save/Load System", handles saving and loading of jeepney data

    private string gameMode;
    private SaveLoadSystem sls;
    private CarController carcon;
    private UpgradesPanel up;
    private SkinCustomizer sc;

    //UPGRADES
    [SerializeField] private GameObject upgBells;
    [SerializeField] private GameObject upgAntennas;
    [SerializeField] private GameObject upgCircleLights;
    [SerializeField] private GameObject upgBumperLights;
    [SerializeField] private GameObject upgFrontGrills;
    [SerializeField] private GameObject upgSideGrills;
    [SerializeField] private GameObject upgHorns;
    [SerializeField] private GameObject upgBackGrills;
    [SerializeField] private GameObject upgTopGrills;
    [SerializeField] private GameObject upgBoomerang;
    [SerializeField] private GameObject upgWings;
    [SerializeField] private GameObject upgTopLights;
    [SerializeField] private GameObject upgCoinHolder1;
    [SerializeField] private GameObject upgCoinHolder2;

    //SKINS
    public int currentSkinIndex; //jeep body

    private void Start() {
        sls = SaveLoadSystem.current;
        up = UpgradesPanel.current;
        carcon = GetComponent<CarController>();
        sc = GetComponent<SkinCustomizer>();
    }

    private void Update() {
        if(gameMode == null && sls.gameMode != null && TimeManager.current) {
            gameMode = sls.gameMode;

            LoadPrevious();
        }
    }

    public void Save() {
        print("Saving jeepney stats");

        if(carcon == null) {
            print("No CARCON!");
            return;
        }

        #region STATS ================================================================================

        //STANDARD: Playerprefs key names should have the same name as the variables

        //max health
        PlayerPrefs.SetInt(gameMode + "_Jeepney_MaxHealth", carcon.maxHealth);
        //health
        PlayerPrefs.SetInt(gameMode + "_Jeepney_Health", carcon.health);
        //fuel cap
        PlayerPrefs.SetInt(gameMode + "_Jeepney_FuelCap", carcon.fuelCapacity);
        //fuel
        PlayerPrefs.SetInt(gameMode + "_Jeepney_Fuel", carcon.fuelAmount);
        //fuel eff
        PlayerPrefs.SetInt(gameMode + "_Jeepney_FuelLoss", carcon.fuelLoss); //Realizing late (right now), that 'eff' is the worst possible name for this variable
        //max gear
        PlayerPrefs.SetInt(gameMode + "_Jeepney_MaxGear", carcon.maxGear);

        print("maxHealth: " + carcon.maxHealth);
        print("health: " + carcon.health);
        print("fuelCap: " + carcon.fuelCapacity);
        print("fuel: " + carcon.fuelAmount);
        print("fuelLoss: " + carcon.fuelLoss);
        print("maxGear: " + carcon.maxGear);

        #endregion
        #region UPGRADES ================================================================================

        PlayerPrefs.SetInt(gameMode + "_Jeepney_UpgBells", upgBells.activeSelf? 1:0);
        PlayerPrefs.SetInt(gameMode + "_Jeepney_UpgAntennas", upgAntennas.activeSelf? 1:0);
        PlayerPrefs.SetInt(gameMode + "_Jeepney_UpgCircleLights", upgCircleLights.activeSelf? 1:0);
        PlayerPrefs.SetInt(gameMode + "_Jeepney_UpgBumperLights", upgBumperLights.activeSelf? 1:0);
        PlayerPrefs.SetInt(gameMode + "_Jeepney_UpgFrontGrills", upgFrontGrills.activeSelf? 1:0);
        PlayerPrefs.SetInt(gameMode + "_Jeepney_UpgSideGrills", upgSideGrills.activeSelf? 1:0);
        PlayerPrefs.SetInt(gameMode + "_Jeepney_UpgHorns", upgHorns.activeSelf? 1:0);
        PlayerPrefs.SetInt(gameMode + "_Jeepney_UpgBackGrills", upgBackGrills.activeSelf? 1:0);
        PlayerPrefs.SetInt(gameMode + "_Jeepney_UpgTopGrills", upgTopGrills.activeSelf? 1:0);
        PlayerPrefs.SetInt(gameMode + "_Jeepney_UpgBoomerang", upgBoomerang.activeSelf? 1:0);
        PlayerPrefs.SetInt(gameMode + "_Jeepney_UpgWings", upgWings.activeSelf? 1:0);
        PlayerPrefs.SetInt(gameMode + "_Jeepney_UpgTopLights", upgTopLights.activeSelf? 1:0);
        PlayerPrefs.SetInt(gameMode + "_Jeepney_UpgCoinHolder1", upgCoinHolder1.activeSelf? 1:0);
        PlayerPrefs.SetInt(gameMode + "_Jeepney_UpgCoinHolder2", upgCoinHolder2.activeSelf? 1:0);

        #endregion
        #region CUSTOMIZATION ================================================================================

        //SKIN
        PlayerPrefs.SetString(gameMode + "_Jeepney_SkinsOwned", sc.GetSkinsOwned());
        PlayerPrefs.SetInt(gameMode + "_Jeepney_CurrentSkin", sc.GetCurrentSkin());
        PlayerPrefs.SetString(gameMode + "_Jeepney_SWSkinsOwned", sc.GetSWSkinsOwned());
        PlayerPrefs.SetInt(gameMode + "_Jeepney_CurrentSWSkin", sc.GetCurrentSWSkin());
        PlayerPrefs.SetString(gameMode + "_Jeepney_SeatSkinsOwned", sc.GetSeatSkinsOwned());
        PlayerPrefs.SetInt(gameMode + "_Jeepney_CurrentSeatSkin", sc.GetCurrentSeatSkin());

        #endregion
        #region OTHERS ================================================================================

        PlayerPrefs.SetString(gameMode + "_Jeepney_Name", carcon.jeepName);

        #endregion
    }

    public void LoadSaved() { //DEFAULT VALUES MUST BE SET
        print("Loading jeepney settings. carcon: " + carcon.name);

        #region STATS ================================================================================

        //STANDARD: Playerprefs key names should have the same name as the variables

        //max health
        int maxHealth = PlayerPrefs.GetInt(gameMode + "_Jeepney_MaxHealth", 100);
        //health
        int health = PlayerPrefs.GetInt(gameMode + "_Jeepney_Health", 100);
        //fuel cap
        int fuelCap = PlayerPrefs.GetInt(gameMode + "_Jeepney_FuelCap", 50000);
        //fuel
        int fuel = PlayerPrefs.GetInt(gameMode + "_Jeepney_Fuel", 50000);
        //fuel eff
        int fuelLoss = PlayerPrefs.GetInt(gameMode + "_Jeepney_FuelLoss", 1); //Realizing late (right now), that 'eff' is the worst possible name for this variable
        //max gear
        int maxGear = PlayerPrefs.GetInt(gameMode + "_Jeepney_MaxGear", 8);
        if(SaveLoadSystem.current.gameMode == "Freeride") maxGear = 8;

        print("maxHealth: " + maxHealth);
        print("health: " + health);
        print("fuelCap: " + fuelCap);
        print("fuel: " + fuel);
        print("fuelLoss: " + fuelLoss);
        print("maxGear: " + maxGear);

        #endregion
        #region UPGRADES ================================================================================

        up.Toggle(upgBells.name, (PlayerPrefs.GetInt(gameMode + "_Jeepney_UpgBells", 0) == 1? true:false));
        up.Toggle(upgAntennas.name, (PlayerPrefs.GetInt(gameMode + "_Jeepney_UpgAntennas", 0) == 1? true:false));
        up.Toggle(upgCircleLights.name, (PlayerPrefs.GetInt(gameMode + "_Jeepney_UpgCircleLights", 0) == 1? true:false));
        up.Toggle(upgBumperLights.name, (PlayerPrefs.GetInt(gameMode + "_Jeepney_UpgBumperLights", 0) == 1? true:false));
        up.Toggle(upgFrontGrills.name, (PlayerPrefs.GetInt(gameMode + "_Jeepney_UpgFrontGrills", 0) == 1? true:false));
        up.Toggle(upgSideGrills.name, (PlayerPrefs.GetInt(gameMode + "_Jeepney_UpgSideGrills", 0) == 1? true:false));
        up.Toggle(upgHorns.name, (PlayerPrefs.GetInt(gameMode + "_Jeepney_UpgHorns", 0) == 1? true:false));
        up.Toggle(upgBackGrills.name, (PlayerPrefs.GetInt(gameMode + "_Jeepney_UpgBackGrills", 0) == 1? true:false));
        up.Toggle(upgTopGrills.name, (PlayerPrefs.GetInt(gameMode + "_Jeepney_UpgTopGrills", 0) == 1? true:false));
        up.Toggle(upgBoomerang.name, (PlayerPrefs.GetInt(gameMode + "_Jeepney_UpgBoomerang", 0) == 1? true:false));
        up.Toggle(upgWings.name, (PlayerPrefs.GetInt(gameMode + "_Jeepney_UpgWings", 0) == 1? true:false));
        up.Toggle(upgTopLights.name, (PlayerPrefs.GetInt(gameMode + "_Jeepney_UpgTopLights", 0) == 1? true:false));
        up.Toggle(upgCoinHolder1.name, (PlayerPrefs.GetInt(gameMode + "_Jeepney_UpgCoinHolder1", 0) == 1? true:false));
        up.Toggle(upgCoinHolder2.name, (PlayerPrefs.GetInt(gameMode + "_Jeepney_UpgCoinHolder2", 0) == 1? true:false));

        #endregion
        #region CUSTOMIZATION ================================================================================

        //SKINS
        sc.LoadSavedSkins(PlayerPrefs.GetString(gameMode + "_Jeepney_SkinsOwned"));
        sc.LoadSkin(PlayerPrefs.GetInt(gameMode + "_Jeepney_CurrentSkin"));
        sc.LoadSavedSWSkins(PlayerPrefs.GetString(gameMode + "_Jeepney_SWSkinsOwned"));
        sc.LoadSWSkin(PlayerPrefs.GetInt(gameMode + "_Jeepney_CurrentSWSkin"));
        sc.LoadSavedSeatSkins(PlayerPrefs.GetString(gameMode + "_Jeepney_SeatSkinsOwned"));
        sc.LoadSeatSkin(PlayerPrefs.GetInt(gameMode + "_Jeepney_CurrentSeatSkin"));

        #endregion
        #region OTHERS ================================================================================

        string jeepName = PlayerPrefs.GetString(gameMode + "_Jeepney_Name", "Jeepney Name");

        #endregion
        #region APPLICAITON ================================================================================

        carcon.maxHealth = maxHealth;
        carcon.SetHealth(health);
        carcon.fuelCapacity = fuelCap;
        carcon.fuelAmount = fuel;
        carcon.fuelLoss = fuelLoss;
        // carcon.maxGear = maxGear;
        carcon.SetMaxGear(maxGear);
        carcon.Rename(jeepName);

        #endregion

        JeepneyPanel.current.UpdateReqs();
    }

    private void LoadDefault() {
        
        print("Loading default jeepney settings. carcon: " + carcon.name);

        #region STATS ================================================================================

        //STANDARD: Playerprefs key names should have the same name as the variables

        //max health
        int maxHealth = 100;
        //health
        int health = 100;
        //fuel cap
        int fuelCap = 50000;
        //fuel
        int fuel = 50000;
        //fuel eff
        int fuelLoss = 1; //Realizing late (right now), that 'eff' is the worst possible name for this variable
        //max gear
        int maxGear = 4;
        if(SaveLoadSystem.current.gameMode == "Freeride") maxGear = 8;

        print("maxHealth: " + maxHealth);
        print("health: " + health);
        print("fuelCap: " + fuelCap);
        print("fuel: " + fuel);
        print("fuelLoss: " + fuelLoss);
        print("maxGear: " + maxGear);

        #endregion
        #region UPGRADES ================================================================================

        //all upgrades off
        up.SetAsDefault();

        #endregion
        #region CUSTOMIZATION ================================================================================

        //SKINS
        sc.LoadDefaultSkins();

        #endregion
        #region OTHERS ================================================================================

        string jeepName = "Jeepney Name";

        #endregion
        #region APPLICAITON ================================================================================

        carcon.maxHealth = maxHealth;
        carcon.SetHealth(health);
        carcon.fuelCapacity = fuelCap;
        carcon.fuelAmount = fuel;
        carcon.fuelLoss = fuelLoss;
        // carcon.maxGear = maxGear;
        carcon.SetMaxGear(maxGear);
        carcon.Rename(jeepName);

        #endregion

        JeepneyPanel.current.UpdateReqs();
    }

    public void LoadPrevious() { //load previous save or default
        if(gameMode == "Freeride") return;
        
        bool isNewGame = TimeManager.current.days == 1? true:false;

        if(isNewGame) LoadDefault();
        else LoadSaved();
    }
}
