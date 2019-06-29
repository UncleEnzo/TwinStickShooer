using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PowerUp
{
    [SerializeField]
    public string name;
    [Header("Make duration -10 if you want onetime use powerup")]
    [SerializeField]
    public float duration;
    [Header("Stack cap is always how many you want plus one.")]
    [SerializeField]
    public int stackCap;
    public int currentStack = 0;
    [SerializeField]
    public UnityEvent startAction;
    [SerializeField]
    public UnityEvent endAction;

    public void Start()
    {
        if (startAction != null)
        {
            startAction.Invoke();
        }
    }
    public void End()
    {
        if (endAction != null)
        {
            endAction.Invoke();
        }
    }
}
