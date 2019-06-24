using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Coin", menuName = "Inventory/Coin")]
public class Coins : Item
{
    public override void useItem()
    {
        base.useItem();
        //Note: This item cannot be used, it is just automatically picked up
    }
}
