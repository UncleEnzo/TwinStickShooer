using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootLedger : MonoBehaviour
{
    public static Color32 PhysicalColor = new Color32(62, 248, 50, 235);
    public static Color32 GunPowderColor = new Color32(253, 44, 216, 235);
    public static Color32 ExplosiveColor = new Color32(0, 0, 0, 235);
    public static Color32 WeaponColor = new Color32(202, 204, 206, 235);

    [Header("Loot Ledger")]
    public List<Loot> PhysicalRecipes;
    public List<Loot> GunpowderRecipes;
    public List<Loot> ExplosiveRecipes;
    public List<Loot> Weapons;
    public List<Loot> CraftComponents;
    public static Dictionary<string, Loot> LootLedgerDict = new Dictionary<string, Loot>();

    [Header("New Game Data")]
    public List<GameObject> StartingPhysicalRecipes;
    public List<GameObject> StartingGunpowderRecipes;
    public List<GameObject> StartingExplosiveRecipes;
    public List<GameObject> StartingWeapons;
    //new game vendor loot pools
    public List<GameObject> PhysicalVendor;
    public List<GameObject> GunpowderVendor;
    public List<GameObject> ExplosiveVendor;
    public List<GameObject> WeaponVendor;
    public Dictionary<VendorType, List<string>> NewGameVendorItemPool = new Dictionary<VendorType, List<string>>();
    public Dictionary<LootListType, List<string>> NewGamePlayerItemPool = new Dictionary<LootListType, List<string>>();

    //On awake, this creates a dictionary of everything item in the lists
    void Awake()
    {
        //Creates the static game object dictionary for global use
        SaveToLootLedgerDict(PhysicalRecipes);
        SaveToLootLedgerDict(GunpowderRecipes);
        SaveToLootLedgerDict(ExplosiveRecipes);
        SaveToLootLedgerDict(Weapons);
        SaveToLootLedgerDict(CraftComponents);

        //Creates the new game vendor dictionary
        SaveNewGameDict(VendorType.Physical, PhysicalVendor);
        SaveNewGameDict(VendorType.Gunpowder, GunpowderVendor);
        SaveNewGameDict(VendorType.Explosive, ExplosiveVendor);
        SaveNewGameDict(VendorType.Weapon, WeaponVendor);

        //Creates the new game player dictionary
        SaveNewGameDict(LootListType.PhysicalRecipe, StartingPhysicalRecipes);
        SaveNewGameDict(LootListType.GunpowderRecipe, StartingGunpowderRecipes);
        SaveNewGameDict(LootListType.ExplosiveRecipe, StartingExplosiveRecipes);
        SaveNewGameDict(LootListType.Weapon, StartingWeapons);
    }

    private void SaveToLootLedgerDict(List<Loot> itemList)
    {
        foreach (Loot loot in itemList)
        {
            if (!LootLedgerDict.ContainsKey(loot.item.name))
            {
                LootLedgerDict.Add(loot.item.name, loot);
            }
        }
    }

    private void SaveNewGameDict(LootListType listType, List<GameObject> itemList)
    {
        List<string> itemListToSave = new List<string>();
        foreach (GameObject item in itemList)
        {
            itemListToSave.Add(item.name);
        }
        NewGamePlayerItemPool.Add(listType, itemListToSave);
    }

    private void SaveNewGameDict(VendorType listType, List<GameObject> itemList)
    {
        List<string> itemListToSave = new List<string>();
        foreach (GameObject item in itemList)
        {
            itemListToSave.Add(item.name);
        }
        NewGameVendorItemPool.Add(listType, itemListToSave);
    }
}
