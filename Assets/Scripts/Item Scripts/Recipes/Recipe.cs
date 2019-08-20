using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Inventory/Recipe")]
public class Recipe : Item
{
    [SerializeField]
    protected PowerUp powerUp;
    public int physicalRequirement = 1;
    public int gunPowderRequirement = 1;
    public int explosiveRequirement = 1;

    public override void useItem()
    {
        base.useItem();
        bool hasEnoughComponents = checkRequirements();
        bool satisfiesConditions = PowerUpConditions.Instance.checkConditions(powerUp);
        usePowerUp(hasEnoughComponents, satisfiesConditions);
    }
    private bool checkRequirements()
    {
        bool physicalCheck = RecipeItemManager.Instance.checkRequirements(Inventory.Instance.physicalCraftComponents, physicalRequirement);
        bool gunPowderCheck = RecipeItemManager.Instance.checkRequirements(Inventory.Instance.gunpowderCraftComponents, gunPowderRequirement);
        bool explosiveCheck = RecipeItemManager.Instance.checkRequirements(Inventory.Instance.explosiveCraftComponents, explosiveRequirement);
        if (!physicalCheck || !gunPowderCheck || !explosiveCheck)
        {
            Debug.Log("Not enough components to use recipe.");
            return false;
        }
        else
        {
            return true;
        }
    }

    private void usePowerUp(bool hasEnoughComponents, bool satisfiesConditions)
    {
        if (hasEnoughComponents && satisfiesConditions)
        {
            RecipeItemManager.Instance.useRecipeComponents(physicalRequirement, gunPowderRequirement, explosiveRequirement);
            PowerUpController.Instance.ActivatePowerUp(powerUp);
            PowerUpUIDrawer.Instance.AddIcon(powerUp, this);
        }
    }
}