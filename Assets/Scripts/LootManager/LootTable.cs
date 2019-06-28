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
    public List<Loot> physicalRecipes;
    public List<Loot> gunpowderRecipes;
    public List<Loot> explosiveRecipes;
    public List<Loot> weapons;
    private int rareDropChanceIncrease = 30;
    private Dictionary<LootListType, List<Loot>> lootMap;
    private LootListType lootList;

    void Start()
    {
        lootMap = new Dictionary<LootListType, List<Loot>>();
        lootMap.Add(LootListType.PhysicalRecipe, physicalRecipes);
        lootMap.Add(LootListType.GunpowderRecipe, gunpowderRecipes);
        lootMap.Add(LootListType.ExplosiveRecipe, explosiveRecipes);
        lootMap.Add(LootListType.Weapon, weapons);
    }

    public GameObject generateRandomLoot(LootListType lootList, int chestRarityRange)
    {
        List<Loot> lootListType = new List<Loot>();
        foreach (Loot loot in lootMap[lootList])
        {
            lootListType.Add(new Loot(loot));
        }

        if (chestRarityRange > 0)
        {
            foreach (Loot loot in lootListType)
            {
                if (loot.dropRarity <= chestRarityRange)
                {
                    loot.dropRarity += rareDropChanceIncrease;
                }
            }
            lootListType = lootListType.OrderByDescending(i => i.dropRarity).ToList();
            foreach (Loot loot in lootListType)
            {
                print(loot.name + " rarity = " + loot.dropRarity);
            }
        }
        else
        {
            print("Chest rarity is neutral");
        }

        int total = 0;
        int randomNumber = 0;

        //tally the total weight
        foreach (Loot loot in lootListType)
        {
            total += loot.dropRarity;
        }

        //draw a random number between 0 and the total weight (100)
        randomNumber = UnityEngine.Random.Range(0, total);
        print("Random number is: " + randomNumber);

        for (int i = 0; i < lootListType.Count; i++)
        {
            if (randomNumber <= lootListType[i].dropRarity)
            {
                return lootListType[i].item;
            }
            else
            {
                randomNumber -= lootListType[i].dropRarity;
            }
        }

        foreach (Loot loot in lootListType)
        {
            int lootWeight = loot.dropRarity;
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
        this.name = obj.name;
        this.item = obj.item;
        this.dropRarity = obj.dropRarity;
    }
    public string name;
    public GameObject item;
    public int dropRarity;
}

public enum LootListType { PhysicalRecipe, GunpowderRecipe, ExplosiveRecipe, Weapon }
