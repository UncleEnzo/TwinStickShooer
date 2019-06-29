using System.Collections;
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
        bool wasPickedUp = Inventory.Instance.AddItem(item);
        if (wasPickedUp)
        {
            gameObject.SetActive(false);
        }
    }
}
