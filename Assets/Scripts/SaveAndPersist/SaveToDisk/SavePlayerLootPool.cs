using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavePlayerLootPool
{
    //Note: Needs to be strings with the names of items, Unity doesn't serialize GameObjects :(
    public List<string> PhysicalRecipeLootPool = new List<string>();
    public List<string> GunPowderRecipeLootPool = new List<string>();
    public List<string> ExplosiveRecipeLootPool = new List<string>();
    public List<string> WeaponLootPool = new List<string>();

    //New Game method constructor, Overwrites all save game data
    public SavePlayerLootPool(LootLedger LootLedger)
    {
        UpdateList(PhysicalRecipeLootPool, LootLedger.StartingPhysicalRecipes);
        UpdateList(GunPowderRecipeLootPool, LootLedger.GunpowderRecipes);
        UpdateList(ExplosiveRecipeLootPool, LootLedger.ExplosiveRecipes);
        UpdateList(WeaponLootPool, LootLedger.StartingWeapons);
    }

    //Should be saving this THROUGH PersistentGameData whenever you make a transaction with Vendor
    public SavePlayerLootPool(PersistentGameData PersistentGameData)
    {
        //perform an add if any object is missing in the persistent version
        // PlayerLootPool = ;
    }

    private void UpdateList(List<string> SavedList, List<Loot> lootList)
    {
        SavedList.Clear();
        foreach (Loot loot in lootList)
        {
            SavedList.Add(loot.item.name);
        }
    }
}
