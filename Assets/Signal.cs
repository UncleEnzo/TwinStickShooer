using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Signal : ScriptableObject
{
    public List<SignalListener> listeners = new List<SignalListener>();
    public void Raise()
    {
        //going through listener backwards to make sure that if something is removed, it doesn't cause an out of range exception
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnSignalRaised();
        }
    }

    public void RegisterListener(SignalListener listener)
    {
        listeners.Add(listener);
    }

    public void DeregisterListener(SignalListener listener)
    {
        listeners.Remove(listener);
    }
}
