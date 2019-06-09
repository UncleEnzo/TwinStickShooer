using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeItemManager : MonoBehaviour
{
    #region Singleton
    public static RecipeItemManager instance;

    void Awake()
    {
        instance = this;
    }
    #endregion

    private Inventory inventory;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    public bool checkRequirements(int craftItemType, int craftItemRequirement)
    {
        bool hasComponent = false;
        bool meetsRequiredCount = false;
        foreach (InventorySlot slot in inventory.inventorySlots)
        {
            if (craftItemRequirement <= 0)
            {
                hasComponent = true;
                meetsRequiredCount = true;
            }
            else if (slot.firstItem != null && (int)slot.firstItem.craftItemType == craftItemType)
            {
                hasComponent = true;
                if (slot.Count >= craftItemRequirement)
                {
                    meetsRequiredCount = true;
                }
            }
        }
        if (hasComponent && meetsRequiredCount)
        {
            return true;
        }
        return false;
    }

    public void useRecipeComponents(int greenRequirement, int purpleRequirement, int blackRequirement)
    {
        foreach (InventorySlot slot in inventory.inventorySlots)
        {
            removeNumOfItems(0, greenRequirement, slot);
            removeNumOfItems(1, purpleRequirement, slot);
            removeNumOfItems(2, blackRequirement, slot);
        }
    }
    private void removeNumOfItems(int craftItemType, int requirement, InventorySlot slot)
    {
        if (slot.firstItem != null && (int)slot.firstItem.craftItemType == craftItemType)
        {
            if (requirement > 0)
            {
                for (int i = 0; i != requirement; i++)
                {
                    inventory.RemoveItem(slot.firstItem);
                }
            }
        }
    }
}
