using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootLedger : MonoBehaviour
{
    [Header("Loot Ledger")]
    public GameObject HealingPotion;
    public List<Loot> PhysicalRecipes;
    public List<Loot> GunpowderRecipes;
    public List<Loot> ExplosiveRecipes;
    public List<Loot> Weapons;
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
    public Dictionary<string, List<string>> NewGameVendorItemPool = new Dictionary<string, List<string>>();


    //On awake, this creates a dictionary of everything item in the lists
    void Awake()
    {
        //Creates the static game object dictionary for global use
        SaveToLootLedgerDict(PhysicalRecipes);
        SaveToLootLedgerDict(GunpowderRecipes);
        SaveToLootLedgerDict(ExplosiveRecipes);
        SaveToLootLedgerDict(Weapons);

        //Creates the new game vendor dictionary
        SaveToNewGameVendorDict(TagsAndLabels.PhysicalVendor, PhysicalVendor);
        SaveToNewGameVendorDict(TagsAndLabels.GunpowderVendor, GunpowderVendor);
        SaveToNewGameVendorDict(TagsAndLabels.ExplosiveVendor, ExplosiveVendor);
        SaveToNewGameVendorDict(TagsAndLabels.WeaponsVendor, WeaponVendor);
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

    private void SaveToNewGameVendorDict(string vendorName, List<GameObject> itemList)
    {
        List<string> itemListToSave = new List<string>();
        foreach (GameObject item in itemList)
        {
            itemListToSave.Add(item.name);
        }
        NewGameVendorItemPool.Add(vendorName, itemListToSave);
    }
}
