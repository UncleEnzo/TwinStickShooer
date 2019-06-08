using System;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PowerUpController : MonoBehaviour
{
    public List<PowerUp> powerUps;
    public Dictionary<PowerUp, float> activePowerUps = new Dictionary<PowerUp, float>();
    private List<PowerUp> keys = new List<PowerUp>();
    public Text recipeNameUIText;
    public Text stackCountUIText;
    public Text recipeTimerUI;

    void Start()
    {
        endRecipeUIUpdate();
    }
    // Update is called once per frame
    void Update()
    {
        //Note: May need to change this so that it only works in scenes and not menus
        HandleActivePowerUps();
    }

    public void HandleActivePowerUps()
    {
        //bool checks if we made a change to the dictionary
        bool changed = false;

        //if the number of active powerups is greater than 0
        if (activePowerUps.Count > 0)
        {
            //for each powerup in our list of keys(that we added to avoid a dictionary exception)
            foreach (PowerUp powerUp in keys)
            {
                //if the powerup in the list has a duration over 0
                //Reduce the duration
                if (activePowerUps[powerUp] > 0)
                {
                    //NOTE: This is where you should update the UI timer
                    recipeUIUpdate(powerUp, (int)activePowerUps[powerUp]);
                    activePowerUps[powerUp] -= Time.deltaTime;
                }
                //if we are out of time on a powerup,
                //remove the powerup from list, and end the powerup ability
                else
                {
                    changed = true;
                    activePowerUps.Remove(powerUp);
                    endRecipeUIUpdate();
                    powerUp.End();
                }
            }
        }

        if (changed)
        {
            //Takes the keys list we initialized in the beginning
            //and refreshes it with the updated list of powerups 
            keys = new List<PowerUp>(activePowerUps.Keys);
        }
    }

    public void ActivatePowerUp(PowerUp powerUp)
    {
        if (!activePowerUps.ContainsKey(powerUp))
        {
            powerUp.Start();
            activePowerUps.Add(powerUp, powerUp.duration);
        }
        else
        {
            //Stacks powerup effect
            powerUp.Start();

            //resets the powerup duration if it is used again
            activePowerUps[powerUp] = powerUp.duration;
        }

        //Iterating over keys to avoid iterating through dictionary and removing, which causes exceptions
        keys = new List<PowerUp>(activePowerUps.Keys);
    }

    private void recipeUIUpdate(PowerUp powerUp, int recipeTimer)
    {
        recipeNameUIText.text = powerUp.name;
        //stackCountUIText.text = "Stacks: " + pow;
        recipeTimerUI.text = "Timer: " + recipeTimer.ToString();
    }

    private void endRecipeUIUpdate()
    {
        recipeTimerUI.text = "";
        stackCountUIText.text = "";
        recipeNameUIText.text = "";
    }
}
