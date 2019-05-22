using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipToPlayer : MonoBehaviour
{
    private GameObject playerWeaponHolder;

    // Start is called before the first frame update
    void Start()
    {
        playerWeaponHolder = GameObject.Find("WeaponHolder");

        if (gameObject.transform.IsChildOf(playerWeaponHolder.transform))
        {
            GetComponent<GunControls>().enabled = true;
            GetComponent<GunFiring>().enabled = true;
            GetComponent<GunProperties>().enabled = true;
        }
        else
        {
            GetComponent<Collider2D>().enabled = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            print(GetComponent<GunProperties>().weaponType);
            bool playerHasGun = false;
            Transform weaponHolder = collider.transform.Find("WeaponHolder");
            foreach (Transform weapon in weaponHolder)
            {
                if (weapon.GetComponent<GunProperties>().weaponType == GetComponent<GunProperties>().weaponType)
                {
                    playerHasGun = true;
                }
            }
            if (playerHasGun == true)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.SetParent(collider.transform.Find("WeaponHolder"));
                GetComponent<GunControls>().enabled = true;
                GetComponent<GunFiring>().enabled = true;
                GetComponent<GunProperties>().enabled = true;
                GetComponent<Collider2D>().enabled = false;
                print("NEW GUN, ADDING TO WEAPON HOLSTER");
            }
        }
    }
}


