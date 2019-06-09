using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpAction : MonoBehaviour
{
    //Needs to calculate the amount of times it is called so that when it ends,
    //the end action can cancel the amount of a recipe was used
    //without disabling the effects of other recipes
    float healingIncrease = 1f;

    public void HealStartAction()
    {
        print("Triggered HP Up");
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        playerHealth.localPlayerData.health += healingIncrease;
        FindObjectOfType<PlayerHUBController>().updateDisplayHubHealth(playerHealth.localPlayerData.health);
    }

    float speedUpIncrease = 5f;
    int speedUpStackCount = 0;

    public void HighSpeedStartAction()
    {
        print("Triggered HighSpeedRecipe");
        speedUpStackCount += 1;
        FindObjectOfType<Player>().speed += speedUpIncrease;
    }

    public void HighSpeedEndAction()
    {
        print("HighSpeedRecipe Expired");
        float speedReduction = speedUpIncrease * speedUpStackCount;
        FindObjectOfType<Player>().speed -= speedReduction;
        speedUpStackCount = 0;
    }

    float bulletSpeedIncrease = 5f;
    int bulletSpeedStackCount = 0;

    public void BulletSpeedStartAction()
    {
        print("Triggered Bullet Speed Recipe");
        bulletSpeedStackCount += 1;
        GunProperties[] gunProperties = FindObjectsOfType<GunProperties>();
        foreach (GunProperties gunProperty in gunProperties)
        {
            gunProperty.bulletSpeed += bulletSpeedIncrease;
        }
    }

    public void BulletSpeedEndAction()
    {
        print("Bullet Speed Recipe Expired");
        float bulletSpeedReduction = bulletSpeedIncrease * bulletSpeedStackCount;
        GunProperties[] gunProperties = FindObjectsOfType<GunProperties>();
        foreach (GunProperties gunProperty in gunProperties)
        {
            gunProperty.bulletSpeed -= bulletSpeedReduction;
        }
        bulletSpeedStackCount = 0;
    }
}
