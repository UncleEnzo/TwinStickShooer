using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipToPlayer : MonoBehaviour
{
    private GameObject playerWeaponHolder;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<GunControls>().enabled = false;
        GetComponent<GunFiring>().enabled = false;
        GetComponent<GunProperties>().enabled = false;
        playerWeaponHolder = GameObject.Find("WeaponHolder");

        //issue with this foreach loop > If the player has the gun, and it's in the playing field, it will fire, need to be specific about activating only the one that the player has
        //Issue where the guns fire even though everything is disabled
        if (playerWeaponHolder.transform.parent == gameObject.transform)
        {
            Destroy(gameObject.GetComponent<Collider2D>());
            GetComponent<GunControls>().enabled = true;
            GetComponent<GunFiring>().enabled = true;
            GetComponent<GunProperties>().enabled = true;
            print("PLAYER HAS THIS GUN AND it'S USABLE");
            Destroy(this);
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
                    print("Player has this gun and it won't be added");
                }
            }
            if (playerHasGun == true)
            {
                print("WEAPON EXISTS, DESTROYING");
                Destroy(gameObject);
            } else {
                transform.SetParent(collider.transform.Find("WeaponHolder"));
                GetComponent<GunControls>().enabled = true;
                GetComponent<GunFiring>().enabled = true;
                GetComponent<GunProperties>().enabled = true;
                Destroy(gameObject.GetComponent<Collider2D>());
                Destroy(this);
                print("NEW GUN, ADDING TO WEAPON HOLSTER");
            }
        }
    }
}
       

