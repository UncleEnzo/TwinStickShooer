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
        usePowerUp(hasEnoughComponents);
    }
    private bool checkRequirements()
    {
        bool physicalCheck = RecipeItemManager.Instance.checkRequirements(ItemType.Physical, physicalRequirement);
        bool gunPowderCheck = RecipeItemManager.Instance.checkRequirements(ItemType.GunPowder, gunPowderRequirement);
        bool explosiveCheck = RecipeItemManager.Instance.checkRequirements(ItemType.Explosive, explosiveRequirement);
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

    private void usePowerUp(bool hasEnoughComponents)
    {
        if (hasEnoughComponents)
        {
            RecipeItemManager.Instance.useRecipeComponents(physicalRequirement, gunPowderRequirement, explosiveRequirement);
            PowerUpController.Instance.ActivatePowerUp(powerUp);
            PowerUpUIDrawer.Instance.AddIcon(powerUp, this);
        }
    }
}