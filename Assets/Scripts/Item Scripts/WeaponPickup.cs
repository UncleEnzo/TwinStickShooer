using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Interactable
{
    private GameObject weaponHolder;
    public GameObject weapon;
    public Weapons weapontype;

    void Start()
    {
         weaponHolder = GameObject.FindGameObjectWithTag("WeaponHolder");
    }

    public override void interact()
    {
        base.interact();
        weaponPickUp();
    }

    void weaponPickUp()
    {
        print("Picked up " + weapon.name);

        Weapons[] allWeapons = FindObjectOfType<WeaponSwitching>().allWeapons;
        print(allWeapons);
        addWeapon(weapontype, weapon, allWeapons);
        Destroy(gameObject);
       
    }

    private void addWeapon(Weapons weaponType, GameObject weapon, Weapons[] allWeapons)
    {
        int weaponSlotIndex = (int)weaponType.weaponSlot;
        if(allWeapons[weaponSlotIndex] == null)
        {
            allWeapons[weaponSlotIndex] = weaponType;
            Instantiate(weapon, weaponHolder.transform);
        }
        else
        {
            print("You already have this weapon.");
        }
    }
}
