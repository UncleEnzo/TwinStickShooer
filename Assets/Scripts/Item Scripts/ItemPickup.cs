using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Potions potions;

    public override void interact()
    {
        base.interact();
        itemPickUp();
    }

    void itemPickUp()
    {
        print("Picked up " + potions.name);
        bool wasPickedUp = Inventory.instance.Add(potions);
        if (wasPickedUp)
        {
            Destroy(gameObject);
        }
    }
}
