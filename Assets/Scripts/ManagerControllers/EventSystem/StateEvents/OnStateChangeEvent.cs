using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class OnStateChangeEvent : BaseEvent
{
    public interface OnStateChangeListener
    {
        public void OnEvent(OnStateChangeMessage msg);
    }

    public class OnStateChangeMessage : BaseMessage
    {
        public StateLabel stateBefore;
        public StateLabel stateAfter;
        public StateProperty stateProperty;
    }

    private List<OnStateChangeListener> listeners = new List<OnStateChangeListener>();
    private List<Action<OnStateChangeMessage>> actions = new List<Action<OnStateChangeMessage>>();

    public void AddListener(OnStateChangeListener listener)
    {
        listeners.Add(listener);
    }

    public void AddListener(Action<OnStateChangeMessage> action)
    {
        actions.Add(action);
    }

    public void RemoveListener(OnStateChangeListener listener)
    {
        listeners.Remove(listener);
    }

    public void RemoveListener(Action<OnStateChangeMessage> action)
    {
        actions.Remove(action);
    }

    public void Notify(StateLabel stateBefore, StateLabel stateAfter, StateProperty property)
    {
        OnStateChangeMessage msg = new OnStateChangeMessage 
        { 
            stateBefore = stateBefore,
            stateAfter = stateAfter,
            stateProperty = property,
        };

        listeners.ForEach((x) => x.OnEvent(msg));
        actions.ForEach((x) => x.Invoke(msg));
    }

    public void Clear()
    {
        listeners.Clear();
        actions.Clear();
    }

}