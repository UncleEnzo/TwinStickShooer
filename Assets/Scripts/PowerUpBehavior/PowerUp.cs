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
    [SerializeField]
    public float duration;
    [SerializeField]
    public int spriteNum;

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
