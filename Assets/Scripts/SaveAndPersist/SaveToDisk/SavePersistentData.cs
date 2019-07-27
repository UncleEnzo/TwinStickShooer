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
    public int weaponCount;
    public List<WeaponType> gunTypes;
    public int keys;
    public int physicalCraftComponents;
    public int gunpowderCraftComponents;
    public int explosiveCraftComponents;
    public List<string> acquiredRecipes = new List<string>();
    public Dictionary<LootListType, List<string>> DeductableLootDict = new Dictionary<LootListType, List<string>>();

    //Todo >> Persist the number of explosives you've crafted...

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
        acquiredRecipes.Clear();
        foreach (Item entry in PersistentGameData.currentRecipes)
        {
            acquiredRecipes.Add(entry.name);
        }
        DeductableLootDict.Clear();
        foreach (KeyValuePair<LootListType, List<Loot>> entry in PersistentGameData.currentDeductableLootMap)
        {
            List<string> entryValues = new List<string>();
            foreach (Loot listLoot in entry.Value)
            {
                entryValues.Add(listLoot.item.name);
            }
            DeductableLootDict.Add(entry.Key, entryValues);
        }
    }
}
