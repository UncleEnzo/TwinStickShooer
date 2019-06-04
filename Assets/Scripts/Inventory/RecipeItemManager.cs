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
        inventory = GetComponent<Inventory>();
    }

    public bool checkRequirements(int craftItemType, int craftItemRequirement)
    {
        bool hasComponent = false;
        bool meetsRequiredCount = false;
        foreach (InventorySlot slot in inventory.inventorySlots)
        {
            if (slot.firstItem != null && (int)slot.firstItem.craftItemType == craftItemType)
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
            if (slot.firstItem != null && (int)slot.firstItem.craftItemType == 0)
            {
                for (int i = 0; i == greenRequirement; i++)
                {
                    inventory.RemoveItem(slot.firstItem);
                }

            }
            if (slot.firstItem != null && (int)slot.firstItem.craftItemType == 1)
            {
                for (int i = 0; i == purpleRequirement; i++)
                {
                    inventory.RemoveItem(slot.firstItem);
                }
            }
            if (slot.firstItem != null && (int)slot.firstItem.craftItemType == 2)
            {
                for (int i = 0; i == blackRequirement; i++)
                {
                    inventory.RemoveItem(slot.firstItem);
                }
            }
        }
    }
}
