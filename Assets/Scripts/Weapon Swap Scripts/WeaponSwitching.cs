using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;
    public Weapons[] allWeapons;

    // Start is called before the first frame update
    void Start()
    {
        selectWeapon();

        //ensures that duplicate weapons are not instantiated in your weaponHolder
        int numOfWeaponTypes = System.Enum.GetNames(typeof(WeaponSlot)).Length;
        allWeapons = new Weapons[numOfWeaponTypes];
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        weaponSwitchScrollWheel(); // need to make rules for how to create
        weaponSwitchNumPad(previousSelectedWeapon); // need to have weapons appear in appropriate number
    }

    //currently if you select a weapon that you don't have, you just won't have a weapon equipped.
    private void weaponSwitchNumPad(int previousSelectedWeapon)
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            selectedWeapon = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
        {
            selectedWeapon = 2;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 4)
        {
            selectedWeapon = 3;
        }

        if (Input.GetKeyDown(KeyCode.Alpha5) && transform.childCount >= 5)
        {
            selectedWeapon = 4;
        }

        if (Input.GetKeyDown(KeyCode.Alpha6) && transform.childCount >= 6)
        {
            selectedWeapon = 5;
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
            selectWeapon();
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

    public void selectWeapon()
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                print("Set Selected weapon active");
            }
            else
            {
                weapon.gameObject.SetActive(false);
                print("Set other weapons inactive");
            }
            i++;
        }
    }
}
