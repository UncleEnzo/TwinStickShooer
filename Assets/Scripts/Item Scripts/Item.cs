using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public ItemType itemType;
    public int Cost = 0;
    public virtual void useItem()
    {
        Debug.Log("Using " + name);
    }
}
public enum ItemType { Physical, GunPowder, Explosive, Recipe, Key, Coin }