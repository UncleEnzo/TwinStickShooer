using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveGlobalMoney
{
    public int money;

    public SaveGlobalMoney(PersistentGameData PersistentGameData)
    {
        money = PersistentGameData.currentMoney;
    }
}
