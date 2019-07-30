using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavePlayerLootPool
{
    //Note: Unity doesn't serialize GameObjects :(
    public Dictionary<LootListType, List<string>> PlayerLootPoolDict = new Dictionary<LootListType, List<string>>();

    //New Game method constructor, Overwrites all save game data
    public SavePlayerLootPool(LootLedger LootLedger)
    {
        PlayerLootPoolDict.Clear();
        foreach (KeyValuePair<LootListType, List<string>> entry in LootLedger.NewGamePlayerItemPool)
        {
            PlayerLootPoolDict.Add(entry.Key, entry.Value);
        }
    }

    public SavePlayerLootPool(Dictionary<LootListType, List<string>> SaveData)
    {

    }
}
