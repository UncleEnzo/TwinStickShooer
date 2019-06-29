using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Keep all game data that will persist across scenes in this class
//Health, Ammo, Guns, recipies, crafting components, power up effects and timers
public class PersistentGameData : MonoBehaviour
{
    private PlayerSavedData PlayerSavedData = new PlayerSavedData();
    public static PersistentGameData Instance;
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
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
    #endregion

    public void savePlayerStats()
    {
        PersistentGameData.Instance.PlayerSavedData.health = FindObjectOfType<PlayerHealth>().localPlayerData.health;
        PersistentGameData.Instance.PlayerSavedData.weaponCount = FindObjectOfType<WeaponSwitching>().gameObject.transform.childCount;
        PersistentGameData.Instance.PlayerSavedData.gunTypes = FindObjectOfType<WeaponSwitching>().localWeaponData.gunTypes;

        currentHealth = PersistentGameData.Instance.PlayerSavedData.health;
        currentWeaponCount = PersistentGameData.Instance.PlayerSavedData.weaponCount;
        currentGunTypes = PersistentGameData.Instance.PlayerSavedData.gunTypes;
        //crafing materials
        //recipies
        //timers
        //powerups
    }

    public void resetPersistentStats()
    {
        Destroy(gameObject);
    }
}
