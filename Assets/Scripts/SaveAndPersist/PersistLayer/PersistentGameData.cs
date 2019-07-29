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
    public int currentKeys;
    public int currentMoney;
    public int currentPhysicalCraftComponents;
    public int currentGunPowderCraftComponents;
    public int currentExplosiveCraftComponents;
    public List<Item> currentRecipes = new List<Item>();
    public Dictionary<LootListType, List<Loot>> currentDeductableLootMap = new Dictionary<LootListType, List<Loot>>();
    public Dictionary<WeaponType, int> currentExplosiveAmmo = new Dictionary<WeaponType, int>();

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
        LoadWeapons();
    }

    private void LoadWeapons()
    {
        if (SceneManager.GetActiveScene().buildIndex != SceneLoader.hubWorldIndex)
        {
            SavePersistentData SavePersistentData = SaveSystem.LoadPersistentData();
            currentGunTypes = SavePersistentData.gunTypes;
            currentWeaponCount = SavePersistentData.weaponCount;
            foreach (KeyValuePair<WeaponType, int> entry in SavePersistentData.ExplosiveAmmo)
            {
                currentExplosiveAmmo.Clear();
                currentExplosiveAmmo.Add(entry.Key, entry.Value);
            }
        }
    }

    //THINGS TO NOT PUT IN THIS METHOD BUT TO LOAD AND SAVE DIRECTLY
    //     List of remaining vendor items
    public void saveAndPersistGameData()
    {
        currentHealth = Player.Instance.health;
        currentWeaponCount = WeaponSwitching.Instance.gameObject.transform.childCount;
        currentGunTypes = WeaponSwitching.Instance.gunTypes;
        currentExplosiveAmmo.Clear();
        foreach (ThrowExplosive entry in WeaponSwitching.Instance.GetComponentsInChildren<ThrowExplosive>())
        {
            currentExplosiveAmmo.Add(entry.GunProperties.weaponType, entry.currentAmmo);
        }
        currentKeys = Inventory.Instance.getKeyCount();
        currentMoney = Inventory.Instance.getMoneyCount();
        currentPhysicalCraftComponents = Inventory.Instance.getPhysicalCount();
        currentGunPowderCraftComponents = Inventory.Instance.getGunpowderCount();
        currentExplosiveCraftComponents = Inventory.Instance.getExplosiveCount();
        currentLevel = SceneManager.GetActiveScene().buildIndex;

        //Persists the recipes the player acquired on the run
        currentRecipes.Clear();
        foreach (Item item in Inventory.Instance.getRecipes())
        {
            currentRecipes.Add(item);
        }
        // todo: Persist and save in persistent save file recipes you have in your Recipe panel :P
        // Todo: Save in persistent save file deductable lists

        //NOTE: Takes the deductable loot map and persists it
        currentDeductableLootMap.Clear();
        foreach (KeyValuePair<LootListType, List<Loot>> entry in LootTable.instance.deductableLootMap)
        {
            currentDeductableLootMap.Add(entry.Key, entry.Value);
        }

        SaveSystem.SaveGlobalMoneyData(this);
        SaveSystem.SavePersistentData(this);
    }

    public void resetPersistentGameData()
    {
        SaveSystem.DeletePersistenSaveDataPath();
        Destroy(gameObject);
    }
    public void resetPersistentGameDataRetainSave()
    {
        Destroy(gameObject);
    }
}
