using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public InventorySlot inventorySlot { get; set; }
    public CraftItemType craftItemType;
    public virtual void useItem()
    {
        //Use the item
        //Something may happen
        //Use this method to turn craftables into Pots/GunPowder/Explosives
        //Use this method to use potions/GunPowders/Explosives

        Debug.Log("Using " + name);
    }
}
public class InventoryEventArgs : EventArgs
{
    public InventoryEventArgs(Item item)
    {
        Item = item;
    }
    public Item Item;
}
public enum CraftItemType { Physical, GunPowder, Explosive, None }