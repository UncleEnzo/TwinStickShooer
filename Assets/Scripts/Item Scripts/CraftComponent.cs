using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftComponent", menuName = "Inventory/CraftComponent")]
public class CraftComponent : Item
{
    public override void useItem()
    {
        base.useItem();
        //Use the consumable
        //Remove it from the inventory
    }
}