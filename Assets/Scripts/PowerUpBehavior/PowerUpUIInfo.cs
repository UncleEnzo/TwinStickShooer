using System.Collections;
using UnityEngine;

public class PowerUpUIInfo
{
    public GameObject icon;
    public float defaultDuration;
    public float maxDuration; //required for percent calculation for UI radial
    public float timeLeft; //required for percent calculation for UI radial

    public PowerUpUIInfo(GameObject icon, PowerUp powerup)
    {
        this.icon = icon;
        defaultDuration = powerup.duration;
        maxDuration = powerup.duration;
        timeLeft = powerup.duration;
    }
}
