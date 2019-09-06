using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
    public override void interact()
    {
        base.interact();
        if (item.itemType == ItemType.Physical)
        {
            if (Inventory.Instance.getPhysicalCount() < 100)
            {
                itemPickUp();
            }
        }
        else if (item.itemType == ItemType.GunPowder)
        {
            if (Inventory.Instance.getGunpowderCount() < 100)
            {
                itemPickUp();
            }
        }
        else if (item.itemType == ItemType.Explosive)
        {
            if (Inventory.Instance.getExplosiveCount() < 100)
            {
                itemPickUp();
            }
        }
        else
        {
            itemPickUp();
        }
    }

    void itemPickUp()
    {
        Inventory.Instance.AddItem(item);
        gameObject.SetActive(false);
    }
}
