using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDisasterCollide : BaseEvent
{
    public interface OnDisasterCollideListener
    {
        public void OnEvent(OnDisasterCollideMessage msg);
    }

    public class OnDisasterCollideMessage : BaseMessage
    {
        string DisasterName { get; set; }

    }

    private List<OnDisasterCollideListener> listeners = new();
    private List<Action<OnDisasterCollideMessage>> actions = new();

    public void AddListener(OnDisasterCollideListener listener)
    {
        listeners.Add(listener);
    }

    public void AddListener(Action<OnDisasterCollideMessage> action)
    {
        actions.Add(action);
    }

    public void RemoveListener(OnDisasterCollideListener listener)
    {
        listeners.Remove(listener);
    }

    public void RemoveListener(Action<OnDisasterCollideMessage> action)
    {
        actions.Remove(action);
    }

    public void Notify()
    {
        OnDisasterCollideMessage msg = new();
        listeners.ForEach((x) => x.OnEvent(msg));
        actions.ForEach((x) => x.Invoke(msg));
    }

    public void Clear()
    {
        listeners.Clear();
        actions.Clear();
    }
}
