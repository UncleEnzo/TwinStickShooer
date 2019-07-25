using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavePlayerLootPool
{
    //Note: Needs to be strings with the names of items, Unity doesn't serialize GameObjects :(
    public List<Loot> PhysicalRecipeLootPool = new List<Loot>();
    public List<Loot> GunPowderRecipeLootPool = new List<Loot>();
    public List<Loot> ExplosiveRecipeLootPool = new List<Loot>();
    public List<Loot> WeaponLootPool = new List<Loot>();

    //RESETS ALL GAME DATA TO NEW GAME DATA.
    public SavePlayerLootPool(NewGameData NewGameData)
    {
        PhysicalRecipeLootPool.Clear();

        // foreach (Loot loot in NewGameData.StartingPhysicalRecipes)
        // {
        //     PhysicalRecipeLootPool.Add(loot);
        // }
        PhysicalRecipeLootPool = new List<Loot>(NewGameData.StartingPhysicalRecipes);

        if (GunPowderRecipeLootPool != null)
        {
            GunPowderRecipeLootPool.Clear();
        }
        GunPowderRecipeLootPool = new List<Loot>(NewGameData.StartingGunpowderRecipes);

        if (ExplosiveRecipeLootPool != null)
        {
            ExplosiveRecipeLootPool.Clear();
        }
        ExplosiveRecipeLootPool = new List<Loot>(NewGameData.StartingExplosiveRecipes);

        if (WeaponLootPool != null)
        {
            WeaponLootPool.Clear();
        }
        WeaponLootPool = new List<Loot>(NewGameData.StartingWeapons);
    }


    //Should be saving this THROUGH PersistentGameData whenever you make a transaction with Vendor
    public SavePlayerLootPool(PersistentGameData PersistentGameData)
    {
        //perform an add if any object is missing in the persistent version
        // PlayerLootPool = ;
    }
}
