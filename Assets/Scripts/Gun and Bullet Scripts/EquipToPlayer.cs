using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipToPlayer : MonoBehaviour
{
    private GameObject weaponHolder;

    // Start is called before the first frame update
    void Start()
    {
        weaponHolder = WeaponSwitching.Instance.transform.root.gameObject;

        if (gameObject.transform.IsChildOf(weaponHolder.transform))
        {
            GetComponent<Weapon>().enabled = true;
        }
        else
        {
            GetComponent<Collider2D>().enabled = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == TagsAndLabels.PlayerTag)
        {
            bool playerHasGun = false;
            foreach (Transform weapon in weaponHolder.transform)
            {
                if (weapon.GetComponent<Weapon>().GunProperties.weaponType == GetComponent<Weapon>().GunProperties.weaponType)
                {
                    Destroy(gameObject);
                    playerHasGun = true;
                }
            }
            if (!playerHasGun)
            {
                transform.SetParent(weaponHolder.transform);
                GetComponent<Weapon>().enabled = true;
                GetComponent<Collider2D>().enabled = false;
                print("New Gun added to WeaponHolster");
            }
        }
    }
}


