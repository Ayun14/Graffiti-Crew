using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Subject : MonoBehaviour
{
    private readonly List<ObserverBase> observers = new();

    public event Action OnNotify;

    public void AttachObserver(ObserverBase observer) // 붙인다
    {
        if (null == observer) return;

        if (false == observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }

    public void DetachObserver(ObserverBase observer) // 뗀다
    {
        if (null == observer) return;

        if (observers.Contains(observer))
        {
            observers.Remove(observer);
        }
    }

    protected void NotifyObservers() // 알린다
    {
        OnNotify?.Invoke();
    }

    protected void ClearObservers()
    {
        if (observers.Count > 0)
        {
            observers.Clear();
        }
    }
}
