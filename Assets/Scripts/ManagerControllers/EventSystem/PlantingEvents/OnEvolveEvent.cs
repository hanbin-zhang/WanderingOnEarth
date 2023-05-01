using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEvolveEvent : BaseEvent
{
    public interface OnEvolveListener
    {
        public void OnEvent(OnEvolveMessage msg);
    }

    public class OnEvolveMessage : BaseMessage
    {

    }

    private List<OnEvolveListener> listeners = new List<OnEvolveListener>();
    private List<Action<OnEvolveMessage>> actions = new List<Action<OnEvolveMessage>>();

    public void AddListener(OnEvolveListener listener)
    {
        listeners.Add(listener);
    }

    public void AddListener(Action<OnEvolveMessage> action)
    {
        actions.Add(action);
    }

    public void RemoveListener(OnEvolveListener listener)
    {
        listeners.Remove(listener);
    }

    public void RemoveListener(Action<OnEvolveMessage> action)
    {
        actions.Remove(action);
    }

    public void Notify(float greenValue)
    {
        OnEvolveMessage msg = new OnEvolveMessage();
        listeners.ForEach((x) => x.OnEvent(msg));
        actions.ForEach((x) => x.Invoke(msg));
    }

    public void Clear()
    {
        listeners.Clear();
        actions.Clear();
    }
}
