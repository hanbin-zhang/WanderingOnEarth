﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class OnPlantEvent : BaseEvent
{
    //private EventLabel eventLabel = EventLabel.ON_PLANT;

    public interface OnPlantListener
    {
        public void OnEvent(OnPlantMessage msg);
    }

    public class OnPlantMessage : BaseMessage
    {
        public OnPlantMessage() => eventLabel = EventLabel.ON_PLANT;
        public Vector3 pos { get; set; }
        public Quaternion rotation { get; set; }
    }

    private List<OnPlantListener> listeners = new List<OnPlantListener>();
    private List<Action<OnPlantMessage>> actions = new List<Action<OnPlantMessage>>();

    public void AddListener(OnPlantListener listener)
    {
        listeners.Add(listener);
    }

    public void AddListener(Action<OnPlantMessage> action)
    {
        actions.Add(action);
    }

    public void RemoveListener(OnPlantListener listener)
    {
        listeners.Remove(listener);
    }

    public void RemoveListener(Action<OnPlantMessage> action)
    {
        actions.Remove(action);
    }

    public void Notify(Vector3 pos)
    {
        var msg = new OnPlantMessage()
        {
            pos = pos,
            rotation = Quaternion.identity,
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

