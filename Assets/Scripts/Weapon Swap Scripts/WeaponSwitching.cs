using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    private int selectedWeapon = 0;
    private int weaponCount = 0;
    private int previousWeaponCount = 0;
    public PlayerSavedData localWeaponData = new PlayerSavedData();

    private PlayerHUBController playerHUBController;


    // Start is called before the first frame update
    void Start()
    {
        playerHUBController = FindObjectOfType<PlayerHUBController>();
        selectWeapon();
        weaponCount = transform.childCount;
        addWeaponToPersist();
    }


    // Update is called once per frame
    void Update()
    {
        //Calls select weapon if scroll wheel is used
        int previousSelectedWeapon = selectedWeapon;
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

    private void autoSelectNewWeaponInHolster()
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
        localWeaponData.weaponCount = transform.childCount;

        //add to weapontype array
        foreach (Transform weapon in transform)
        {
            if (!localWeaponData.gunTypes.Contains(weapon.GetComponent<GunProperties>().weaponType))
            {
                localWeaponData.gunTypes.Add(weapon.GetComponent<GunProperties>().weaponType);
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
        print("COULD NOT GET SELECTED WEAPON. RETURNING NULL.");
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
                playerHUBController.updateDisplayHubGun(weapon.GetComponent<GunProperties>().weaponType);
                playerHUBController.updateDisplayHubAmmo(weapon.GetComponent<GunFiring>().currentAmmo);
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
        weapon.GetComponent<GunFiring>().enabled = gunEnabled; //Stops reload coroutine so it doesn't jam up
        weapon.GetComponent<SpriteRenderer>().enabled = gunEnabled;
        weapon.GetComponent<GunProperties>().enabled = gunEnabled;
    }
}
