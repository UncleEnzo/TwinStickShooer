using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftComponent", menuName = "Inventory/CraftComponent")]
public class CraftComponent : Item
{
    public override void useItem()
    {
        base.useItem();
        //todo need to create a method in RecipeComponent and have it call CraftComponent.useItem, to remove the item it needs to make the recipe 
        //Need to remove from inventory when used
    }
}