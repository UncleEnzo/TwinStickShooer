using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Key", menuName = "Inventory/Key")]
public class Keys : Item
{
    public override void useItem()
    {
        base.useItem();
        //Note: This item cannot be used, it is just automatically picked up
    }
}