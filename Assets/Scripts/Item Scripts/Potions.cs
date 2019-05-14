using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Potions", menuName = "Inventory/Potions" )]
public class Potions : Item
{
    public int healthModifier;
    public int damageModifier;

    public override void useItem()
    {
        base.useItem();
        //Use the consumable
        //Remove it from the inventory
    }
}

public enum PotionType
{

}