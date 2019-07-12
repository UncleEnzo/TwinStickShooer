using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//Keep all game data that will persist across scenes in this class
//Health, Ammo, Guns, recipies, crafting components, power up effects and timers
public class PersistentGameData : MonoBehaviour
{
    public static PersistentGameData Instance;
    public int currentLevel;
    public float currentHealth;
    public int currentWeaponCount;
    public List<WeaponType> currentGunTypes;

    //Testing
    public int currentKeys;
    public int currentMoney;
    public int currentPhysicalCraftComponents;
    public int currentGunPowderCraftComponents;
    public int currentExplosiveCraftComponents;

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
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
    #endregion

    void Start()
    {
        // Note: Just call the method to load wherever it fucking works, there's no reason to it.
        //Guns work here for some reason
        //inventory only works in fucking inventory.instance for some reason
        LoadGame();
    }

    private void LoadGame()
    {
        SavePersistentData SavePersistentData = SaveSystem.LoadPersistentData();
        currentGunTypes = SavePersistentData.gunTypes;
        currentWeaponCount = SavePersistentData.weaponCount;
    }

    //THINGS TO NOT PUT IN THIS METHOD BUT TO LOAD AND SAVE DIRECTLY
    //     Loottable list items you unlocked from vendors
    //     List of remaining vedor items
    public void saveAndPersistGameData()
    {
        currentHealth = Player.Instance.health;
        currentWeaponCount = WeaponSwitching.Instance.gameObject.transform.childCount;
        currentGunTypes = WeaponSwitching.Instance.gunTypes;
        currentKeys = Inventory.Instance.getKeyCount();
        currentMoney = Inventory.Instance.getMoneyCount();
        currentPhysicalCraftComponents = Inventory.Instance.getPhysicalCount();
        currentGunPowderCraftComponents = Inventory.Instance.getGunpowderCount();
        currentExplosiveCraftComponents = Inventory.Instance.getExplosiveCount();
        currentLevel = SceneManager.GetActiveScene().buildIndex;

        //recipies you have for the run
        //Things in loottable you have already acquired, that shouldn't appear in chests again
        SaveSystem.SaveGlobalMoneyData(this);
        SaveSystem.SavePersistentData(this);
    }

    public void resetPersistentGameData()
    {
        Destroy(gameObject);
    }
}
