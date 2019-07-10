using System.Security.AccessControl;
using System.Dynamic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPersistData
{
    public float health;
    public int weaponCount;
    public List<WeaponType> gunTypes = new List<WeaponType>();
    //Do not need to persist ammo since it's infinite and resets each level
    //Level is persisted in the persister, not here, because this loads the current information into the next scene
}
