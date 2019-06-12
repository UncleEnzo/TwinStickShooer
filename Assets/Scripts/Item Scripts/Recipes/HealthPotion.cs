using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Inventory/HealthPotionRecipe")]
public class HealthPotion : RecipeComponent
{
    public override void useItem()
    {
        base.useItem();
        bool usePowerUp = checkRequirements();

        if (usePowerUp)
        {
            RecipeItemManager recipeItemManager = FindObjectOfType<RecipeItemManager>();
            recipeItemManager.useRecipeComponents(greenRequirement, purpleRequirement, blackRequirement);

            FindObjectOfType<PowerUpController>().ActivatePowerUp(powerUp);
            FindObjectOfType<PowerUpUIDrawer>().AddIcon(powerUp);
        }
    }
}
