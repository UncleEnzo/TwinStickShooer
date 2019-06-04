using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftItemManager : MonoBehaviour
{
    #region Singleton
    public static CraftItemManager instance;

    void Awake()
    {
        instance = this;
    }
    #endregion
    List<CraftComponent> greenComponentsInInventory;  //Make this a list as well
    List<CraftComponent> purpleComponentsInInventory;
    List<CraftComponent> blackComponentsInInventory;

    void Start()
    {
        greenComponentsInInventory = new List<CraftComponent>();
        purpleComponentsInInventory = new List<CraftComponent>();
        blackComponentsInInventory = new List<CraftComponent>();
    }


}
