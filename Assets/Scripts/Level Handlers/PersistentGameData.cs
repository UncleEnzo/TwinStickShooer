using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//Keep all game data that will persist across scenes in this class
//Health, Ammo, Guns, recipies, crafting components, power up effects and timers
public class PersistentGameData : MonoBehaviour
{
    private LevelPersistData LevelPersistData = new LevelPersistData();
    public static PersistentGameData Instance;
    public int currentLevel;
    public float currentHealth;

    public int currentWeaponCount;
    public List<WeaponType> currentGunTypes;


    //On scene start, checks that there is only one of this script and deletes any duplicates
    #region Singleton
    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            print("Triggered AWAKE DESTROY");
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
    #endregion

    public void LoadGame()
    {
        SaveData SaveData = SaveSystem.LoadASave();
        currentHealth = SaveData.health;
        currentGunTypes = SaveData.currentGunTypes;
        currentWeaponCount = SaveData.currentWeaponCount;
    }

    void Start()
    {
        LoadGame();
    }

    //THINGS TO NOT PUT IN THIS METHOD BUT TO LOAD AND SAVE DIRECTLY
    //     Global coins > that should always be what it is in save
    //     Loottable list items you unlocked from vendors
    //     List of remaining vedor items
    public void persistGameData()
    {
        PersistentGameData.Instance.LevelPersistData.health = Player.localPlayerData.health;
        PersistentGameData.Instance.LevelPersistData.weaponCount = WeaponSwitching.Instance.gameObject.transform.childCount;
        PersistentGameData.Instance.LevelPersistData.gunTypes = WeaponSwitching.Instance.localWeaponData.gunTypes;

        currentHealth = PersistentGameData.Instance.LevelPersistData.health;
        currentWeaponCount = PersistentGameData.Instance.LevelPersistData.weaponCount;
        currentGunTypes = PersistentGameData.Instance.LevelPersistData.gunTypes;

        currentLevel = SceneManager.GetActiveScene().buildIndex;

        //crafting material count
        //recipies you have for the run
        //Things in loottable you have already acquired, that shouldn't appear in chests again

    }

    public void resetPersistentGameData()
    {
        Destroy(gameObject);
    }
}
