using UnityEngine;

public class JeepneySLS : MonoBehaviour {
    //Info: "Jeepney Save/Load System", handles saving and loading of jeepney data

    private string gameMode;
    private SaveLoadSystem sls;
    private CarController carcon;

    private void Start() {
        sls = SaveLoadSystem.current;
        carcon = GetComponent<CarController>();
    }

    private void Update() {
        if(gameMode == null && sls.gameMode != null) {
            gameMode = sls.gameMode;

            Load();
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



        #endregion
        #region CUSTOMIZATION ================================================================================



        #endregion
        #region OTHERS ================================================================================

        PlayerPrefs.SetString(gameMode + "_Jeepney_Name", carcon.jeepName);

        #endregion
    }

    public void Load() { //DEFAULT VALUES MUST BE SET
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
        int fuelLoss = PlayerPrefs.GetInt(gameMode + "_Jeepney_FuelLoss", 100); //Realizing late (right now), that 'eff' is the worst possible name for this variable
        //max gear
        int maxGear = PlayerPrefs.GetInt(gameMode + "_Jeepney_MaxGear", 8);

        print("maxHealth: " + maxHealth);
        print("health: " + health);
        print("fuelCap: " + fuelCap);
        print("fuel: " + fuel);
        print("fuelLoss: " + fuelLoss);
        print("maxGear: " + maxGear);

        #endregion
        #region UPGRADES ================================================================================



        #endregion
        #region CUSTOMIZATION ================================================================================



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
}
