using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRarities : MonoBehaviour
{
    public List<ItemDrop> LootTable = new List<ItemDrop>();
    public List<ItemDrop> LootTablePhysical = new List<ItemDrop>();
    public List<ItemDrop> LootTableGunPowder = new List<ItemDrop>();
    public List<ItemDrop> LootTableExplosive = new List<ItemDrop>();
    public List<ItemDrop> LootTableKey = new List<ItemDrop>();
    public List<ItemDrop> LootTableMoney = new List<ItemDrop>();
    public int dropChance;
    void Start()
    {
        foreach (ItemDrop item in LootTable)
        {
            if (item.itemType == ItemType.Physical)
            {
                LootTablePhysical.Add(item);
            }
            if (item.itemType == ItemType.GunPowder)
            {
                LootTableGunPowder.Add(item);
            }
            if (item.itemType == ItemType.Explosive)
            {
                LootTableExplosive.Add(item);
            }
            if (item.itemType == ItemType.Key)
            {
                LootTableKey.Add(item);
            }
            if (item.itemType == ItemType.Coin)
            {
                LootTableMoney.Add(item);
            }
        }
    }

    //Todo Need to calculate loot for each item type >> Start with recipes


    void calculateLoot(List<ItemDrop> LootTableType)
    {
        //Calculates Random loot drop chance
        int calc_dropChance = UnityEngine.Random.Range(0, 101);
        //if drop chance is less then desired drop chance return
        if (calc_dropChance > dropChance)
        {
            Debug.Log("No loot for me");
            return;
        }
        //if random drop chance is less than or equal too drop chance
        if (calc_dropChance <= dropChance)
        {
            int itemDropChance = 0;
            for (int i = 0; i < LootTableType.Count; i++)
            {
                itemDropChance += LootTableType[i].dropRarity;
            }
            Debug.Log("ItemWeight= " + itemDropChance);

            //random value = random range between 0 and item drop chance
            int randomValue = UnityEngine.Random.Range(0, itemDropChance);
            for (int j = 0; j < LootTableType.Count; j++)
            {
                if (randomValue <= LootTableType[j].dropRarity)
                {
                    Instantiate(LootTableType[j].item, transform.position, Quaternion.identity);
                    return;
                }
                randomValue -= LootTableType[j].dropRarity;
                Debug.Log("Random Value Decreased" + LootTableType[j].dropRarity);
            }
        }
    }
}
[Serializable]
public class ItemDrop
{
    public string name;
    public Item item;
    public ItemType itemType;
    public int dropRarity;
}
