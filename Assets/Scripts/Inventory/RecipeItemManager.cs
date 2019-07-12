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

    public bool checkRequirements(List<Item> craftItemList, int craftItemRequirement)
    {
        bool hasComponent = false;
        bool meetsRequiredCount = false;
        if (craftItemRequirement <= 0)
        {
            hasComponent = true;
            meetsRequiredCount = true;
        }
        else if (craftItemList != null)
        {
            hasComponent = true;
            if (craftItemList.Count >= craftItemRequirement)
            {
                meetsRequiredCount = true;
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
        removeNumOfItems(Inventory.Instance.physicalCraftComponents, Inventory.Instance.physicalComponent, greenRequirement);
        removeNumOfItems(Inventory.Instance.gunpowderCraftComponents, Inventory.Instance.gunpowderCompontent, purpleRequirement);
        removeNumOfItems(Inventory.Instance.explosiveCraftComponents, Inventory.Instance.explosiveComponent, blackRequirement);
    }
    private void removeNumOfItems(List<Item> craftItemList, Item componentType, int requirement)
    {
        if (craftItemList != null)
        {
            if (requirement > 0)
            {
                for (int i = 0; i != requirement; i++)
                {
                    Inventory.Instance.RemoveItem(componentType);
                }
            }
        }
    }
}
