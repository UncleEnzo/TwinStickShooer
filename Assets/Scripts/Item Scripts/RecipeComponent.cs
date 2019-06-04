using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeComponent : Item
{
    //Required Components to use the recipe
    public int greenRequirement = 1;
    public int purpleRequirement = 1;
    public int blackRequirement = 1;

    protected bool checkRequirements()
    {
        RecipeItemManager recipeItemManager = FindObjectOfType<RecipeItemManager>();
        bool greenCheck = recipeItemManager.checkRequirements(0, greenRequirement);
        bool purpleCheck = recipeItemManager.checkRequirements(1, purpleRequirement);
        bool blackCheck = recipeItemManager.checkRequirements(2, blackRequirement);
        if (!greenCheck || !purpleCheck || !blackCheck)
        {
            Debug.Log("Not enough components to use recipe.");
            return false;
        }
        else
        {
            return true;
        }
    }
}