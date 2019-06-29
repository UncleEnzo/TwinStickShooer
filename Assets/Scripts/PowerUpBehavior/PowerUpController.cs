using System;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class PowerUpController : MonoBehaviour
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
    public static PowerUpController Instance;
    public static Dictionary<PowerUp, float> activeEffects = new Dictionary<PowerUp, float>();

    public bool timerPaused = true;

    private static List<PowerUp> keys = new List<PowerUp>();

    // Update is called once per frame
    void Update()
    {
        HandleEffects();
    }

    private void HandleEffects()
    {
        if (activeEffects.Count > 0)
        {
            foreach (PowerUp powerup in keys)
            {
                if (activeEffects[powerup] > 0)
                {
                    if (!timerPaused)
                    {
                        activeEffects[powerup] -= Time.deltaTime;
                    }
                }
            }
        }
    }

    public void CleanExpiredTimers()
    {
        bool changed = false;
        if (activeEffects.Count <= 0)
        {
            foreach (PowerUp powerup in keys)
            {
                activeEffects.Remove(powerup);
                powerup.End();
                powerup.currentStack = 0;
                changed = true;
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
