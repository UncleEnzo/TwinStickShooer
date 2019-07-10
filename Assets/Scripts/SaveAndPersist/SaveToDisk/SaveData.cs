using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    //Note: Saves only at start of levels, not during runs (lose everything you get in the level if you don't get to next one)
    //This prevents farming a level for keys etc to get ahead

    //Save and load values only
    //Todo: Need to save Available LootTable pool (objects bought from vendors.)
    public int coins;

    //PersistentGameData
    public int currentLevel;
    public float health;

    public int currentWeaponCount; //Add to persistent game data and take from there
    public List<WeaponType> currentGunTypes;  //Add to persistent game data and take from there
    public int keys;//Add to persistent game data and take from there
    public int physicalCraftComponents;//Add to persistent game data and take from there
    public int gunpowderCraftComponents;//Add to persistent game data and take from there
    public int explosiveCraftComponents;//Add to persistent game data and take from there

    public Recipe[] recipesAcquiredDuringRun;//Add to persistent game data and take from there
    //Todo: List of recipes already picked up on run.
    //Todo: Count for number of each explosive

    public SaveData(PersistentGameData PersistentGameData)
    {
        currentLevel = PersistentGameData.currentLevel;
        health = PersistentGameData.currentHealth;
        currentWeaponCount = PersistentGameData.currentWeaponCount;
        currentGunTypes = PersistentGameData.currentGunTypes;
    }
}
