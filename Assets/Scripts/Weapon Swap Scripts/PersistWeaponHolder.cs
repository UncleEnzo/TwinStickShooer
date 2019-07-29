using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistWeaponHolder : MonoBehaviour
{
    public int currentWeaponCount;
    public List<WeaponType> currentGunTypes;
    public GameObject[] weaponsToInstantiate;


    // Start is called before the first frame update
    void Start()
    {
        GameObject weaponHolder = WeaponSwitching.Instance.gameObject;

        if (PersistentGameData.Instance.currentWeaponCount != weaponHolder.transform.childCount)
        {
            //get all weaponTypes in the newly instantiated weaponholder
            List<WeaponType> weaponsInHolder = new List<WeaponType>();
            foreach (Transform weapon in weaponHolder.transform)
            {
                weaponsInHolder.Add(weapon.GetComponentInChildren<Weapon>().GunProperties.weaponType);
            }

            //Compare weapons in weaponholder to weapons in persisted weapons and add missing ones
            foreach (WeaponType weaponType in PersistentGameData.Instance.currentGunTypes)
            {
                if (!weaponsInHolder.Contains(weaponType))
                {
                    int weaponIndexValue = (int)weaponType;
                    Instantiate(weaponsToInstantiate[weaponIndexValue], weaponHolder.transform);
                }
            }
            //For each explosive weapon in weapon holder, updates its current ammo
            foreach (KeyValuePair<WeaponType, int> entry in PersistentGameData.Instance.currentExplosiveAmmo)
            {
                foreach (ThrowExplosive explosive in weaponHolder.GetComponentsInChildren<ThrowExplosive>())
                {
                    if (explosive.GunProperties.weaponType == entry.Key)
                    {
                        explosive.currentAmmo = entry.Value;
                    }
                }
            }
        }
    }
}
