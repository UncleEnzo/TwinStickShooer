using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpUIDrawer : MonoBehaviour
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
    public static PowerUpUIDrawer Instance;
    public bool timerPaused = true;
    public GameObject powerUpIcon;
    public Text powerupTimerPause;
    private GameObject powerUpIconPanel;
    private Dictionary<PowerUp, PowerUpUIInfo> powerUps = new Dictionary<PowerUp, PowerUpUIInfo>();
    private List<PowerUp> keys = new List<PowerUp>();

    void Start()
    {
        powerupTimerPause = GameObject.Find("Canvas").transform.Find("PowerUpTimersPaused").GetComponent<Text>();
        powerupTimerPause.text = "";
        powerUpIconPanel = GameObject.Find("Canvas").transform.Find("PowerUpPanel").gameObject;
    }

    void Update()
    {
        UpdateTimers();
    }

    public void AddIcon(PowerUp powerup, Item item)
    {
        if (!powerUps.ContainsKey(powerup))
        {
            powerup.currentStack = 0; //Resets it DO NOT REMOVE THIS
            powerup.currentStack++;
            GameObject icon = Instantiate<GameObject>(powerUpIcon);
            GameObject powerupSprite = icon.transform.GetChild(0).GetChild(1).gameObject;
            powerupSprite.GetComponent<Image>().sprite = item.icon;
            PowerUpUIInfo info = new PowerUpUIInfo(icon, powerup);
            powerUps.Add(powerup, info);
            icon.transform.SetParent(powerUpIconPanel.transform);
            icon.SetActive(true);

            //sets the stack counter to false
            icon.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            icon.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            if (powerup.currentStack < powerup.stackCap)
            {
                powerup.currentStack++;//add an if to compare limits so that it's gets to max
            }
            powerUps[powerup].maxDuration = powerup.duration;
            powerUps[powerup].timeLeft = powerup.duration;
            stackCounter(powerUps[powerup].icon, powerup);
        }
        keys = new List<PowerUp>(powerUps.Keys);
    }

    public void RemoveIcon(PowerUp powerup)
    {
        powerUps[powerup].icon.SetActive(false);
        powerUps.Remove(powerup);
    }

    private void UpdateTimers()
    {
        bool changed = false;
        if (powerUps.Count > 0)
        {
            foreach (PowerUp effect in keys)
            {
                PowerUpUIInfo powerupInfo = powerUps[effect];

                //prevents one time use items, like health potions from showing up
                if (powerupInfo.timeLeft == -10)
                {
                    RemoveIcon(effect);
                    changed = true;
                }

                if (powerupInfo.timeLeft > 0)
                {
                    powerupInfo.icon.transform.GetChild(2).GetComponent<Image>().enabled = false;
                    if (timerPaused)
                    {
                        powerupTimerPause.text = "Timers Paused";
                    }
                    if (!timerPaused)
                    {
                        powerupTimerPause.text = "";
                        powerupInfo.timeLeft -= Time.deltaTime;
                    }
                }
                else
                {
                    powerupInfo.icon.transform.GetChild(2).GetComponent<Image>().enabled = true;
                }
                if (powerUps.ContainsKey(effect))
                {
                    //math to calculate percentage of current time left and apply it to the radial dial
                    powerupInfo.icon.transform.GetChild(0).GetComponent<Image>().fillAmount = powerupInfo.timeLeft / powerUps[effect].maxDuration;
                }
            }
        }
        if (changed)
        {
            keys = new List<PowerUp>(powerUps.Keys);
        }
    }

    public void CleanExpiredTimers()
    {
        bool changed = false;
        foreach (PowerUp effect in keys)
        {
            PowerUpUIInfo powerupInfo = powerUps[effect];
            if (powerupInfo.timeLeft <= 0)
            {
                RemoveIcon(effect);
                changed = true;
            }
        }

        if (changed)
        {
            keys = new List<PowerUp>(powerUps.Keys);
        }
    }

    private void stackCounter(GameObject icon, PowerUp powerup)
    {
        icon.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        icon.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
        icon.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "x" + (powerup.currentStack);
    }
}
