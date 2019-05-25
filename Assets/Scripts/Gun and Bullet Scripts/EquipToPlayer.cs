using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipToPlayer : MonoBehaviour
{
    private GameObject weaponHolder;

    // Start is called before the first frame update
    void Start()
    {
        weaponHolder = GameObject.Find("WeaponHolder");

        if (gameObject.transform.IsChildOf(weaponHolder.transform))
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
            bool playerHasGun = false;
            foreach (Transform weapon in weaponHolder.transform)
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
                transform.SetParent(weaponHolder.transform);
                GetComponent<GunControls>().enabled = true;
                GetComponent<GunFiring>().enabled = true;
                GetComponent<GunProperties>().enabled = true;
                GetComponent<Collider2D>().enabled = false;
                print("New Gun added to WeaponHolster");
            }
        }
    }
}


