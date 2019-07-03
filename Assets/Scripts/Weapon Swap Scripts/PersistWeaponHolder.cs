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
        PersistentGameData persistentGameData = PersistentGameData.Instance;
        GameObject weaponHolder = WeaponSwitching.Instance.gameObject;

        if (persistentGameData.currentWeaponCount != weaponHolder.transform.childCount)
        {
            //get all weaponTypes in the newly instantiated weaponholder
            List<WeaponType> weaponsInHolder = new List<WeaponType>();
            foreach (Transform weapon in weaponHolder.transform)
            {
                weaponsInHolder.Add(weapon.GetComponentInChildren<Weapon>().GunProperties.weaponType);
            }

            //Compare weapons in weaponholder to weapons in persisted weapons and add missing ones
            foreach (WeaponType weaponType in persistentGameData.currentGunTypes)
            {
                if (!weaponsInHolder.Contains(weaponType))
                {
                    int weaponIndexValue = (int)weaponType;
                    print(weaponIndexValue);
                    Instantiate(weaponsToInstantiate[weaponIndexValue], weaponHolder.transform);
                }
            }
        }
    }
}
