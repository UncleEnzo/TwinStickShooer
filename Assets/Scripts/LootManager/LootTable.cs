using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LootTable : MonoBehaviour
{
    #region Singleton
    public static LootTable instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one Instance of LootTable found.");
        }
        instance = this;
    }
    #endregion

    //Make List of each type of loot item. Give each a corresponding rarity
    [Header("Drop rarity needs to be ordered highest to lowest or loot table fails")]
    public List<Loot> DeductablePhysicalRecipes;
    public List<Loot> DeductableGunpowderRecipes;
    public List<Loot> DeductableExplosiveRecipes;
    public List<Loot> DeductableWeapons;
    public List<Loot> CraftComponents;
    private int rareDropChanceIncrease = 30;
    [System.NonSerialized]
    public Dictionary<LootListType, List<Loot>> deductableLootMap;

    void Start()
    {
        deductableLootMap = new Dictionary<LootListType, List<Loot>>();

        //When leaving hub world, resets all deductables from the player total loot pools
        if (SceneManager.GetActiveScene().buildIndex == SceneLoader.hubWorldIndex + 1)
        {
            print("Leaving Hub World. Resetting Deductables from player loot pool.");
            //step one > Loads the player loot pools and creates deductables out of them
            SavePlayerLootPool SavePlayerLootPool = SaveSystem.LoadPlayerLootPoolData();
            ResetDeductableList(DeductablePhysicalRecipes, SavePlayerLootPool.PlayerLootPoolDict[LootListType.PhysicalRecipe]);
            ResetDeductableList(DeductableGunpowderRecipes, SavePlayerLootPool.PlayerLootPoolDict[LootListType.GunpowderRecipe]);
            ResetDeductableList(DeductableExplosiveRecipes, SavePlayerLootPool.PlayerLootPoolDict[LootListType.ExplosiveRecipe]);
            ResetDeductableList(DeductableWeapons, SavePlayerLootPool.PlayerLootPoolDict[LootListType.Weapon]);

            //Step two > makes a deductable map
            deductableLootMap.Add(LootListType.PhysicalRecipe, DeductablePhysicalRecipes);
            deductableLootMap.Add(LootListType.GunpowderRecipe, DeductableGunpowderRecipes);
            deductableLootMap.Add(LootListType.ExplosiveRecipe, DeductableExplosiveRecipes);
            deductableLootMap.Add(LootListType.Weapon, DeductableWeapons);
            deductableLootMap.Add(LootListType.CraftComponents, CraftComponents);
        }
        //This will also happen if you load into hub world, which we don't want
        else if (PersistentGameData.Instance.currentDeductableLootMap != null && SceneManager.GetActiveScene().buildIndex != SceneLoader.hubWorldIndex)
        {
            print("Entering next level. Retrieving saved Deductable lists");
            SavePersistentData SavePersistentData = SaveSystem.LoadPersistentData();

            //Step 1 > Takes Entries from the persistent map, and adds them to the deductable map
            foreach (KeyValuePair<LootListType, List<string>> entry in SavePersistentData.DeductableLootDict)
            {
                List<Loot> entryValues = new List<Loot>();
                foreach (string listLoot in entry.Value)
                {
                    entryValues.Add(LootLedger.LootLedgerDict[listLoot]);
                }
                //Leave this for each loop to load in components and other values to loot table that are not deductable
                deductableLootMap.Add(entry.Key, entryValues);
            }

            //Step 2 > Takes Entries from the Deductable loot map in this class and adds them to the deductable lists
            PersistDeductableList(DeductablePhysicalRecipes, LootListType.PhysicalRecipe);
            PersistDeductableList(DeductableGunpowderRecipes, LootListType.GunpowderRecipe);
            PersistDeductableList(DeductableExplosiveRecipes, LootListType.ExplosiveRecipe);
            PersistDeductableList(DeductableWeapons, LootListType.Weapon);
        }
        else
        {
            Debug.Log("Loading into hub world, taking no action");
        }

        //On start sorts all lists in descending order to make sure loot table is in the correct order
        foreach (KeyValuePair<LootListType, List<Loot>> entry in deductableLootMap)
        {
            sortListByWeight(entry.Value);
        }
    }

    private void PersistDeductableList(List<Loot> DeductableList, LootListType PersistLootType)
    {
        DeductableList.Clear();
        foreach (Loot entry in deductableLootMap[PersistLootType])
        {
            DeductableList.Add(entry);
        }
    }

    private void ResetDeductableList(List<Loot> DeductableList, List<string> LoadList)
    {
        DeductableList.Clear();
        foreach (string entry in LoadList)
        {
            DeductableList.Add(LootLedger.LootLedgerDict[entry]);
        }
    }

    public List<Loot> sortListByWeight(List<Loot> lootListType)
    {
        lootListType.OrderByDescending(i => i.weight).ToList();
        return lootListType;
    }

    public void RemoveItemFromPool(GameObject itemToRemove)
    {
        Loot lootToRemove = null;
        List<Loot> listToRemoveFrom = null;
        foreach (KeyValuePair<LootListType, List<Loot>> entry in deductableLootMap)
        {
            foreach (Loot loot in entry.Value)
            {
                if (loot.item.name + "(Clone)" == itemToRemove.name || loot.item.name == itemToRemove.name)
                {
                    lootToRemove = loot;
                    listToRemoveFrom = entry.Value;
                    break;
                }
            }
        }
        if (lootToRemove != null && listToRemoveFrom != null)
        {
            listToRemoveFrom.Remove(lootToRemove);
        }
        else
        {
            Debug.LogWarning("The item you are trying to remove could not be found");
            return;
        }
    }

    public Loot generateRandomLootFromDeductable(LootListType lootList, int chestRarityRange)
    {
        List<Loot> lootListType = new List<Loot>();
        foreach (Loot loot in deductableLootMap[lootList])
        {
            lootListType.Add(new Loot(loot));
        }

        if (chestRarityRange != 0)
        {
            foreach (Loot loot in lootListType)
            {
                if (loot.weight <= chestRarityRange)
                {
                    loot.weight += rareDropChanceIncrease;
                }
            }
            lootListType = sortListByWeight(lootListType);
        }
        else
        {
            //add this back in for debug if you need it
            //print("Chest rarity is neutral");
        }

        return generateRandomLoot(lootListType);
    }

    public Loot generateRandomLoot(List<Loot> lootList)
    {
        int total = 0;
        int randomNumber = 0;

        //tally the total weight
        foreach (Loot loot in lootList)
        {
            total += loot.weight;
        }

        //draw a random number between 0 and the total weight (100)
        randomNumber = UnityEngine.Random.Range(0, total);

        for (int i = 0; i < lootList.Count; i++)
        {
            if (randomNumber <= lootList[i].weight)
            {
                return lootList[i];
            }
            else
            {
                randomNumber -= lootList[i].weight;
            }
        }

        foreach (Loot loot in lootList)
        {
            int lootWeight = loot.weight;
            if (randomNumber <= lootWeight)
            {
                Debug.Log("AWARD: " + lootWeight);
            }
            else
            {
                randomNumber -= lootWeight;
            }
        }
        return null;
    }
}

[Serializable]
public class Loot
{
    public Loot(Loot obj)
    {
        this.item = obj.item;
        this.weight = obj.weight;
    }
    public GameObject item;
    public int weight;
}

public enum LootListType { PhysicalRecipe, GunpowderRecipe, ExplosiveRecipe, Weapon, CraftComponents }