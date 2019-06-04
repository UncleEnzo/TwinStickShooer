using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one Instance of Inventory found.");
        }
        instance = this;
    }
    #endregion
    public IList<InventorySlot> inventorySlots = new List<InventorySlot>();
    public Transform itemsParent;
    public event EventHandler<InventoryEventArgs> ItemAdded;
    public event EventHandler<InventoryEventArgs> ItemRemoved;

    public void Start()
    {
        ItemAdded += InventoryScript_ItemAdded;
        ItemRemoved += InventoryScript_ItemRemoved;

        int id = 0;
        foreach (InventorySlot slot in itemsParent.GetComponentsInChildren<InventorySlot>())
        {
            inventorySlots.Add(slot);
            slot.inventoryId = id;
            id++;
        }
    }

    private void InventoryScript_ItemAdded(object sender, InventoryEventArgs e)
    {
        int index = -1;
        foreach (InventorySlot slot in itemsParent.GetComponentsInChildren<InventorySlot>())
        {
            index++;

            if (index == e.Item.inventorySlot.Id)
            {
                slot.icon.sprite = e.Item.icon;
                slot.icon.enabled = true;
                slot.removeButton.interactable = true;

                int itemCount = e.Item.inventorySlot.Count;
                if (itemCount > 1)
                {
                    slot.txtCount.text = itemCount.ToString();
                }
                else
                {
                    slot.txtCount.text = "";
                }
                break;
            }
        }
    }

    private void InventoryScript_ItemRemoved(object sender, InventoryEventArgs e)
    {
        int index = -1;
        foreach (InventorySlot slot in itemsParent.GetComponentsInChildren<InventorySlot>())
        {
            index++;
            if (index == e.Item.inventorySlot.Id)
            {
                int itemCount = e.Item.inventorySlot.Count;
                if (itemCount < 2)
                {
                    slot.txtCount.text = "";
                }
                else
                {
                    slot.txtCount.text = itemCount.ToString();
                }
                if (itemCount == 0)
                {
                    slot.icon.sprite = null;
                    slot.icon.enabled = false;
                    slot.removeButton.interactable = false;
                }
            }
        }
    }
    private InventorySlot findStackableSlot(Item item)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.isStackable(item))
            {
                return slot;
            }
        }
        return null;
    }

    private InventorySlot findNextEmptySlot()
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.isEmpty)
            {
                return slot;
            }
        }
        return null;
    }

    //Add logic to differentiate between stackable and non-stackable
    public bool AddItem(Item item)
    {
        InventorySlot freeSlot = findStackableSlot(item);
        if (freeSlot == null)
        {
            freeSlot = findNextEmptySlot();
        }
        if (freeSlot != null)
        {
            freeSlot.AddItem(item);
            if (ItemAdded != null)
            {
                ItemAdded(this, new InventoryEventArgs(item));
            }
        }
        return true;
    }

    public void RemoveItem(Item item)
    {
        foreach (InventorySlot inventorySlot in inventorySlots)
        {
            if (inventorySlot.Remove(item))
            {
                if (ItemRemoved != null)
                {
                    ItemRemoved(this, new InventoryEventArgs(item));
                }
                break;
            }
        }
    }
}