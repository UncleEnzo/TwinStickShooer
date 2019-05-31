using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public CraftComponent craftComponent;

    public override void interact()
    {
        base.interact();
        itemPickUp();
    }

    void itemPickUp()
    {
        print("Picked up " + craftComponent.name);
        bool wasPickedUp = Inventory.instance.Add(craftComponent);
        if (wasPickedUp)
        {
            Destroy(gameObject);
        }
    }
}
