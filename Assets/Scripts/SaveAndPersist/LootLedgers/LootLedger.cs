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
    public List<Loot> StartingPhysicalRecipes;
    public List<Loot> StartingGunpowderRecipes;
    public List<Loot> StaringExplosiveRecipes;
    public List<Loot> StartingWeapons;

    //On awake, this creates a dictionary of everything item in the lists
    void Awake()
    {
        SaveToLootLedgerDict(PhysicalRecipes);
        SaveToLootLedgerDict(GunpowderRecipes);
        SaveToLootLedgerDict(ExplosiveRecipes);
        SaveToLootLedgerDict(Weapons);
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
}
