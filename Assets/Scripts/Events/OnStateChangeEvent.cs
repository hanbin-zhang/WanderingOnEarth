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
        public int RowNum { get; set; }
        public int ColNum { get; set; }
        public StateLabel StateLabel { get; set; }
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

    public void Notify(int rowNum, int colNum, StateLabel state)
    {
        OnStateChangeMessage msg = new OnStateChangeMessage 
        { 
            ColNum = colNum,
            RowNum = rowNum,
            StateLabel = state,
        };

        if (!Photon.Pun.PhotonNetwork.IsMasterClient)
        {
        }
        else
        {
            listeners.ForEach((x) => x.OnEvent(msg));
            actions.ForEach((x) => x.Invoke(msg));
        }
    }

    public void Clear()
    {
        listeners.Clear();
        actions.Clear();
    }

}