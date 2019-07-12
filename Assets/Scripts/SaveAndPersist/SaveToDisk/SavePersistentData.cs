using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavePersistentData
{
    //Note: Saves only at start of levels, not during runs (lose everything you get in the level if you don't get to next one)
    //This prevents farming a level for keys etc to get ahead

    //Save and load values only
    //Todo: Need to save Available LootTable pool (objects bought from vendors.)

    //PersistentGameData
    public int level;
    public float health;
    public int weaponCount; //Add to persistent game data and take from there
    public List<WeaponType> gunTypes;  //Add to persistent game data and take from there
    public int keys;//Add to persistent game data and take from there
    public int physicalCraftComponents;//Add to persistent game data and take from there
    public int gunpowderCraftComponents;//Add to persistent game data and take from there
    public int explosiveCraftComponents;//Add to persistent game data and take from there

    public Recipe[] recipesAcquiredDuringRun;//Add to persistent game data and take from there
    //Todo: List of recipes already picked up on run.
    //Todo: Count for number of each explosive

    public SavePersistentData(PersistentGameData PersistentGameData)
    {
        level = PersistentGameData.currentLevel;
        health = PersistentGameData.currentHealth;
        weaponCount = PersistentGameData.currentWeaponCount;
        gunTypes = PersistentGameData.currentGunTypes;
        physicalCraftComponents = PersistentGameData.currentPhysicalCraftComponents;
        gunpowderCraftComponents = PersistentGameData.currentGunPowderCraftComponents;
        explosiveCraftComponents = PersistentGameData.currentExplosiveCraftComponents;
        keys = PersistentGameData.currentKeys;
    }
}
