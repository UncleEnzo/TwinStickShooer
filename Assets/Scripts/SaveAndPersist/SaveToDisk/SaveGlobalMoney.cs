using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveGlobalMoney
{
    public int money;

    public SaveGlobalMoney()
    {
        money = 0;
    }

    public SaveGlobalMoney(int currentmoney)
    {
        money = currentmoney;
    }
}
