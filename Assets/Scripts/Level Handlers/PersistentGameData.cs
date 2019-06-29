using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Keep all game data that will persist across scenes in this class
//Health, Ammo, Guns, recipies, crafting components, power up effects and timers
public class PersistentGameData : MonoBehaviour
{
    public GameObject player;
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
        PersistentGameData.Instance.PlayerSavedData.health = player.GetComponent<PlayerHealth>().localPlayerData.health;
        PersistentGameData.Instance.PlayerSavedData.weaponCount = WeaponSwitching.Instance.gameObject.transform.childCount;
        PersistentGameData.Instance.PlayerSavedData.gunTypes = WeaponSwitching.Instance.localWeaponData.gunTypes;

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
