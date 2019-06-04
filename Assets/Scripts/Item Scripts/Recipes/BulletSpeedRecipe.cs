using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Inventory/BulletSpeedRecipe")]
public class BulletSpeedRecipe : RecipeComponent
{
    //attributes to buff
    public float bulletSpeed = 1f;

    public override void useItem()
    {
        base.useItem();
        bool useEffect = checkRequirements();
        if (useEffect)
        {
            RecipeItemManager recipeItemManager = FindObjectOfType<RecipeItemManager>();
            recipeItemManager.useRecipeComponents(greenRequirement, purpleRequirement, blackRequirement);

            //Create better way of finding the GunProperties in weaponholder
            GameObject WeaponHolder = GameObject.Find("WeaponHolder");
            GunProperties[] gunProperties = WeaponHolder.GetComponentsInChildren<GunProperties>();
            foreach (GunProperties gunProperty in gunProperties)
            {
                gunProperty.bulletSpeed += bulletSpeed;
            }
        }
        //Add timer and diminishing effect once it expires
    }
}
