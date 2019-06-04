using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Inventory/Recipe")]
public class RecipeComponent : Item
{
    //Required Components to use the recipe
    public int greenRequirement = 1;
    public int purpleRequirement = 1;
    public int blackRequirement = 1;

    //Recipe attributes
    public float HealthIncrease;

    public override void useItem()
    {
        base.useItem();
        RecipeItemManager recipeItemManager = FindObjectOfType<RecipeItemManager>();
        bool greenCheck = recipeItemManager.checkRequirements(0, greenRequirement);
        bool purpleCheck = recipeItemManager.checkRequirements(1, purpleRequirement);
        bool blackCheck = recipeItemManager.checkRequirements(2, blackRequirement);
        if (greenCheck && purpleCheck && blackCheck)
        {
            recipeItemManager.useRecipeComponents(greenRequirement, purpleRequirement, blackRequirement);
            //set timer
            //Apply the recipe effects
        }
        else
        {
            Debug.Log("Not enough components to use recipe.");
        }
        //upon use recipe remains
    }


}