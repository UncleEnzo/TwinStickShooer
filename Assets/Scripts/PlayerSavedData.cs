using System.Security.AccessControl;
using System.Dynamic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSavedData
{
    public float health;
    public int weaponCount;
    public List<WeaponType> gunTypes = new List<WeaponType>();

    //Do not need to persist ammo since it's infinite and resets each level
}
