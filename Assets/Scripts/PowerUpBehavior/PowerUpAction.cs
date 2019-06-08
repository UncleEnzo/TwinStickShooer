using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpAction : MonoBehaviour
{
    //needs to calculate the amount of times it is called so that when it ends,
    //the end action can cancel the amount of a recipe was used
    //without disabling the effects of other recipes
    float speedIncreaseValue = 5f;
    int activeEffectStackCount = 0;

    public void HighSpeedStartAction()
    {
        print("Triggered HighSpeedRecipe");
        activeEffectStackCount += 1;
        FindObjectOfType<Player>().speed += speedIncreaseValue;
    }

    public void HighSpeedEndAction()
    {
        print("HighSpeedRecipe Expired");
        float speedReduction = speedIncreaseValue * activeEffectStackCount;
        FindObjectOfType<Player>().speed -= speedReduction;
        activeEffectStackCount = 0;
    }
}
