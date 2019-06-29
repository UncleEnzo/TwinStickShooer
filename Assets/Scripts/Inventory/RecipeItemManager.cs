using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeItemManager : MonoBehaviour
{
    #region Singleton
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
    #endregion

    public static RecipeItemManager Instance;

    public bool checkRequirements(ItemType craftItemType, int craftItemRequirement)
    {
        bool hasComponent = false;
        bool meetsRequiredCount = false;
        foreach (InventorySlot slot in Inventory.Instance.inventorySlots)
        {
            if (craftItemRequirement <= 0)
            {
                hasComponent = true;
                meetsRequiredCount = true;
            }
            else if (slot.firstItem != null && slot.firstItem.itemType == craftItemType)
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
        foreach (InventorySlot slot in Inventory.Instance.inventorySlots)
        {
            removeNumOfItems(ItemType.Physical, greenRequirement, slot);
            removeNumOfItems(ItemType.GunPowder, purpleRequirement, slot);
            removeNumOfItems(ItemType.Explosive, blackRequirement, slot);
        }
    }
    private void removeNumOfItems(ItemType craftItemType, int requirement, InventorySlot slot)
    {
        if (slot.firstItem != null && slot.firstItem.itemType == craftItemType)
        {
            if (requirement > 0)
            {
                for (int i = 0; i != requirement; i++)
                {
                    Inventory.Instance.RemoveItem(slot.firstItem);
                }
            }
        }
    }
}
