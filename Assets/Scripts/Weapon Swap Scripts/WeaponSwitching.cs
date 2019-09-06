using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponSwitching : MonoBehaviour
{
    #region Singleton
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one Instance of Inventory found.");
        }
        Instance = this;
    }
    #endregion

    public static WeaponSwitching Instance;
    private int selectedWeapon = 0;
    public int weaponCount = 0;
    private int previousWeaponCount = 0;
    public List<WeaponType> gunTypes;
    public Dictionary<WeaponType, int> currentExplosiveAmmo = new Dictionary<WeaponType, int>();
    public GameObject[] arrayOfPossibleWeaponsToLoad;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != SceneLoader.hubWorldIndex)
        {
            SavePersistentData SavePersistentData = SaveSystem.LoadPersistentData();
            if (SavePersistentData != null)
            {
                gunTypes = SavePersistentData.gunTypes;
                weaponCount = SavePersistentData.weaponCount;
                foreach (KeyValuePair<WeaponType, int> entry in SavePersistentData.ExplosiveAmmo)
                {
                    currentExplosiveAmmo.Clear();
                    currentExplosiveAmmo.Add(entry.Key, entry.Value);
                }
            }
        }

        if (PersistentGameData.Instance.currentWeaponCount != this.transform.childCount)
        {
            //get all weaponTypes in the weaponholder
            List<WeaponType> weaponsInHolder = new List<WeaponType>();
            foreach (Transform weapon in this.transform)
            {
                weaponsInHolder.Add(weapon.GetComponentInChildren<Weapon>().GunProperties.weaponType);
            }

            //Compare weapons in weaponholder to weapons in persisted weapons and add missing ones
            foreach (WeaponType weaponType in PersistentGameData.Instance.currentGunTypes)
            {
                if (!weaponsInHolder.Contains(weaponType))
                {
                    int weaponIndexValue = (int)weaponType;
                    Instantiate(arrayOfPossibleWeaponsToLoad[weaponIndexValue], this.transform);
                }
            }
            //For each explosive weapon in weapon holder, updates its current ammo
            foreach (KeyValuePair<WeaponType, int> entry in PersistentGameData.Instance.currentExplosiveAmmo)
            {
                foreach (ThrowExplosive explosive in this.GetComponentsInChildren<ThrowExplosive>())
                {
                    if (explosive.GunProperties.weaponType == entry.Key)
                    {
                        explosive.currentAmmo = entry.Value;
                    }
                }
            }
        }
        selectWeapon();
        addWeaponToPersist();
    }


    // Update is called once per frame
    void Update()
    {
        //Calls select weapon if scroll wheel is used
        int previousSelectedWeapon = selectedWeapon;
        if (Player.Instance.playerUsable)
        {
            weaponSwitchScrollWheel();
            if (previousSelectedWeapon != selectedWeapon)
            {
                selectWeapon();
            }
            //Calls select weapon if new weapon is added
            weaponCount = transform.childCount;
            autoSelectNewWeaponInHolster();
            previousWeaponCount = weaponCount;
        }
    }

    public void autoSelectNewWeaponInHolster()
    {
        if (previousWeaponCount != weaponCount)
        {
            selectedWeapon = transform.childCount - 1;
            selectWeapon();
            addWeaponToPersist();
        }
    }

    private void addWeaponToPersist()
    {
        //Update childcount
        weaponCount = transform.childCount;

        //add to weapontype array
        foreach (Transform weapon in transform)
        {
            if (!gunTypes.Contains(weapon.GetComponent<Weapon>().GunProperties.weaponType))
            {
                gunTypes.Add(weapon.GetComponent<Weapon>().GunProperties.weaponType);
            }
        }
    }

    private void weaponSwitchScrollWheel()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
            {
                selectedWeapon = 0;
            }
            else
            {
                selectedWeapon++;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
            {
                selectedWeapon = transform.childCount - 1;
            }
            else
            {
                selectedWeapon--;
            }
        }
    }

    public Transform getSelectedWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                return weapon;
            }
            i++;
        }
        print("Player does not have a currently equipped weapon.");
        return null;
    }

    public void selectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                gunEnabled(weapon, true);
                if (weapon.GetComponent<PlayerGun>())
                {
                    PlayerHUBController.Instance.updateDisplayHubGun(weapon.GetComponent<PlayerGun>().gunUIImage);
                }
                if (weapon.GetComponent<ThrowExplosive>())
                {
                    PlayerHUBController.Instance.updateDisplayHubGun(weapon.GetComponent<ThrowExplosive>().explosiveUIImage);
                }
                if (weapon.GetComponent<Gun>())
                {
                    PlayerHUBController.Instance.updateDisplayHubAmmo(weapon.GetComponent<Gun>().getCurrentAmmo());
                }
                if (weapon.GetComponent<ThrowExplosive>())
                {
                    PlayerHUBController.Instance.updateDisplayHubAmmo(weapon.GetComponent<ThrowExplosive>().currentAmmo);
                }

            }
            else
            {
                gunEnabled(weapon, false);
            }
            i++;
        }
    }

    private void gunEnabled(Transform weapon, bool gunEnabled)
    {
        weapon.GetComponent<Weapon>().enabled = gunEnabled; //Stops reload coroutine so it doesn't jam up
        weapon.GetComponent<SpriteRenderer>().enabled = gunEnabled;
    }
}
