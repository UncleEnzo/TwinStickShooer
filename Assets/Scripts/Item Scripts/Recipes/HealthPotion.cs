using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Inventory/HealthPotionRecipe")]
public class HealthPotion : RecipeComponent
{
    //attributes to buff
    public float healthIncrease = 1f;

    public override void useItem()
    {
        base.useItem();
        bool useEffect = checkRequirements();

        if (useEffect)
        {
            RecipeItemManager recipeItemManager = FindObjectOfType<RecipeItemManager>();
            PlayerHealth playerHealth = FindObjectOfType<Player>().GetComponent<PlayerHealth>();

            recipeItemManager.useRecipeComponents(greenRequirement, purpleRequirement, blackRequirement);
            playerHealth.heal(healthIncrease);
        }
    }
}
