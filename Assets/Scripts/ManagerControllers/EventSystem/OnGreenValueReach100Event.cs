using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGreenValueReach100Event : BaseEvent
{
    public interface OnGreenValueReach100Listener
    {
        public void OnEvent(OnGreenValueReach100Message msg);
    }

    public class OnGreenValueReach100Message : BaseMessage
    {

    }

    private List<OnGreenValueReach100Listener> listeners = new List<OnGreenValueReach100Listener>();
    private List<Action<OnGreenValueReach100Message>> actions = new List<Action<OnGreenValueReach100Message>>();

    public void AddListener(OnGreenValueReach100Listener listener)
    {
        listeners.Add(listener);
    }

    public void AddListener(Action<OnGreenValueReach100Message> action)
    {
        actions.Add(action);
    }

    public void RemoveListener(OnGreenValueReach100Listener listener)
    {
        listeners.Remove(listener);
    }

    public void RemoveListener(Action<OnGreenValueReach100Message> action)
    {
        actions.Remove(action);
    }

    public void Notify()
    {
        OnGreenValueReach100Message msg = new OnGreenValueReach100Message();
        listeners.ForEach((x) => x.OnEvent(msg));
        actions.ForEach((x) => x.Invoke(msg));
    }

    public void Clear()
    {
        listeners.Clear();
        actions.Clear();
    }
}
