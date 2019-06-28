using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public InventorySlot inventorySlot { get; set; }
    public ItemType itemType;
    public virtual void useItem()
    {
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
public enum ItemType { Physical, GunPowder, Explosive, Recipe, Key, Coin }
public enum RecipeType { None, PhysicalRecipe, GunPowderRecipe, ExplosiveRecipe }