﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;

    public override void interact()
    {
        base.interact();
        itemPickUp();
    }

    void itemPickUp()
    {
        print("pick up " + item.name);
        FindObjectOfType<Player>().isItemPickUp = false;
        bool wasPickedUp = Inventory.instance.Add(item);
        if (wasPickedUp)
        {
            Destroy(gameObject);
        }

    }
}
