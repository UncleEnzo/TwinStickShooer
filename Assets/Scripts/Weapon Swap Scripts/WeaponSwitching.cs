using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    private int selectedWeapon = 0;
    private int weaponCount = 0;
    private int previousWeaponCount = 0;


    // Start is called before the first frame update
    void Start()
    {
        selectWeapon();
        weaponCount = transform.childCount;
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
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.GetComponent<GunFiring>().enabled = true; //reenables reload
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.GetComponent<GunFiring>().enabled = false; //Cuts reload short if switching in middle
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
