﻿using System;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class PowerUpController : MonoBehaviour
{
    //STUFF TO POTENTIALLY TAKE OUT
    // public Text recipeNameUIText;
    // public Text stackCountUIText;
    // public Text recipeTimerUI;
    public static Dictionary<PowerUp, float> activeEffects = new Dictionary<PowerUp, float>();

    private static List<PowerUp> keys = new List<PowerUp>();

    void Start()
    {
        //endRecipeUIUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        HandleEffects();
    }

    private static void HandleEffects()
    {
        bool changed = false;
        if (activeEffects.Count > 0)
        {
            foreach (PowerUp powerup in keys)
            {
                if (activeEffects[powerup] > 0)
                {
                    activeEffects[powerup] -= Time.deltaTime;
                }
                else
                {
                    changed = true;
                    activeEffects.Remove(powerup);
                    powerup.End();
                    powerup.currentStack = 0;
                }
            }
        }
        if (changed)
        {
            keys = new List<PowerUp>(activeEffects.Keys);
        }
    }

    public void ActivatePowerUp(PowerUp powerup)
    {
        if ((!activeEffects.ContainsKey(powerup)) && (powerup.currentStack < powerup.stackCap))
        {
            powerup.Start();
            powerup.currentStack = 1;
            powerup.currentStack++;
            activeEffects.Add(powerup, powerup.duration);
            print(powerup.currentStack);
        }
        else if (activeEffects.ContainsKey(powerup) && (powerup.currentStack < powerup.stackCap))
        {
            powerup.Start();
            powerup.currentStack++;
            activeEffects[powerup] = powerup.duration;
            print(powerup.currentStack);
        }
        else
        {
            print("You have reached the maximum stack. The effect will not stack any more, although the timer will reset");
            print(powerup.currentStack);
            activeEffects[powerup] = powerup.duration;
        }
        keys = new List<PowerUp>(activeEffects.Keys);
    }
}