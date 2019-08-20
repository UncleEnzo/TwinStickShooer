using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpConditions : MonoBehaviour
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
    public static PowerUpConditions Instance;

    public bool checkConditions(PowerUp powerup)
    {
        bool conditionsSatisfied = true;
        if (!conditionStackAndTimerMaxed(powerup))
        {
            return !conditionsSatisfied;
        }
        if (powerup.name == TagsAndLabels.HealthPotionRecipe)
        {
            if (!MaxHealthCondition())
            {
                return !conditionsSatisfied;
            }
        }
        return conditionsSatisfied;
    }

    private bool conditionStackAndTimerMaxed(PowerUp powerup)
    {
        if (PowerUpController.Instance.activeEffects.ContainsKey(powerup)
         && (powerup.currentStack == powerup.stackCap)
         && powerup.duration == PowerUpController.Instance.activeEffects[powerup])
        {
            print("Power up is already at max stacks and time.");
            return false;
        }
        return true;
    }

    private bool MaxHealthCondition()
    {
        if (Player.Instance.health >= Player.Instance.totalHealth)
        {
            print("Player is already at max health");
            return false;
        }
        return true;
    }
}
