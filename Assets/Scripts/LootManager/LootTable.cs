using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [Header("MAKE SURE DROP RARITY ORDERED HIGHEST TO LOWEST IN LISTs OR LOOTTABLE FAILS")]
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
        //TODO: If player leaves hubworld, deductable loot pools from Total
        //NOTE: This function will be removed when you start having an actual loot pool??
        deductableLootMap = new Dictionary<LootListType, List<Loot>>();
        if (PersistentGameData.Instance.currentDeductableLootMap != null)
        {
            foreach (KeyValuePair<LootListType, List<Loot>> entry in PersistentGameData.Instance.currentDeductableLootMap)
            {
                //Leave this for each loop to load in components and other values to loot table that are not deductable
                deductableLootMap.Add(entry.Key, entry.Value);
            }
            DeductablePhysicalRecipes.Clear();
            DeductablePhysicalRecipes = deductableLootMap[LootListType.PhysicalRecipe];
            DeductableGunpowderRecipes.Clear();
            DeductableGunpowderRecipes = deductableLootMap[LootListType.GunpowderRecipe];
            DeductableExplosiveRecipes.Clear();
            DeductableExplosiveRecipes = deductableLootMap[LootListType.ExplosiveRecipe];
            DeductableWeapons.Clear();
            DeductableWeapons = deductableLootMap[LootListType.Weapon];
        }
        else
        {
            //Note: This where you load new lists from playerLootPools to the deductables
            SavePlayerLootPool SavePlayerLootPool = SaveSystem.LoadPlayerLootPoolData();
            DeductablePhysicalRecipes.Clear();
            DeductablePhysicalRecipes = new List<Loot>(SavePlayerLootPool.PhysicalRecipeLootPool);
            DeductableGunpowderRecipes.Clear();
            DeductableGunpowderRecipes = new List<Loot>(SavePlayerLootPool.GunPowderRecipeLootPool);
            DeductableExplosiveRecipes.Clear();
            DeductableExplosiveRecipes = new List<Loot>(SavePlayerLootPool.ExplosiveRecipeLootPool);
            DeductableWeapons.Clear();
            DeductableWeapons = new List<Loot>(SavePlayerLootPool.WeaponLootPool);

            //After you figure that out, then do this next bit so you have a persistent map
            deductableLootMap.Add(LootListType.PhysicalRecipe, DeductablePhysicalRecipes);
            deductableLootMap.Add(LootListType.GunpowderRecipe, DeductableGunpowderRecipes);
            deductableLootMap.Add(LootListType.ExplosiveRecipe, DeductableExplosiveRecipes);
            deductableLootMap.Add(LootListType.Weapon, DeductableWeapons);
            deductableLootMap.Add(LootListType.CraftComponents, CraftComponents);
        }

        //On start sorts all lists in descending order to make sure loot table is in the correct order
        foreach (KeyValuePair<LootListType, List<Loot>> entry in deductableLootMap)
        {
            sortListByWeight(entry.Value);
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

    public GameObject generateRandomLoot(LootListType lootList, int chestRarityRange)
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

        int total = 0;
        int randomNumber = 0;

        //tally the total weight
        foreach (Loot loot in lootListType)
        {
            total += loot.weight;
        }

        //draw a random number between 0 and the total weight (100)
        randomNumber = UnityEngine.Random.Range(0, total);

        for (int i = 0; i < lootListType.Count; i++)
        {
            if (randomNumber <= lootListType[i].weight)
            {
                return lootListType[i].item;
            }
            else
            {
                randomNumber -= lootListType[i].weight;
            }
        }

        foreach (Loot loot in lootListType)
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
