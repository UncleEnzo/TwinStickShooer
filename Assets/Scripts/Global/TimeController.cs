using System.Runtime.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController
{
    private const bool INITIAL_PAUSE_STATE = false;
    private static TimeController INSTANCE = new TimeController();
    private float storedTimeScale = 1f;
    static TimeController()
    {
        Time.timeScale = INITIAL_PAUSE_STATE ? 0f : 1f;
    }
    public static void TogglePauseOn()
    {
        Time.timeScale = 0f;
    }

    public static void SetTimeScale(float timeScale)
    {
        INSTANCE.storedTimeScale = timeScale;
        Time.timeScale = INSTANCE.storedTimeScale;
    }

    public static void TogglePauseOff()
    {
        Time.timeScale = INSTANCE.storedTimeScale;
    }
}

