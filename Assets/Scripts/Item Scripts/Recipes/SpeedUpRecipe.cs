using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Inventory/SpeedUpRecipe")]
public class SpeedUpRecipe : RecipeComponent
{
    public bool useEffect = false;
    public override void useItem()
    {
        base.useItem();
        bool useEffect = checkRequirements();

        if (useEffect)
        {
            RecipeItemManager recipeItemManager = FindObjectOfType<RecipeItemManager>();
            recipeItemManager.useRecipeComponents(greenRequirement, purpleRequirement, blackRequirement);
            FindObjectOfType<PowerUpController>().ActivatePowerUp(powerUp);
        }
    }
    public void SetPowerUp(PowerUp powerUp)
    {
        this.powerUp = powerUp;
        this.name = powerUp.name;
    }
}
