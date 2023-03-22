using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class OnLeftMouseDownEvent : BaseEvent
{
    public interface OnWaterListener
    {
        public void OnEvent(OnWaterMessage msg);
    }

    public class OnWaterMessage : BaseMessage
    {
        
    }

    private List<OnWaterListener> listeners = new List<OnWaterListener>();
    private List<Action<OnWaterMessage>> actions = new List<Action<OnWaterMessage>>();

    public void AddListener(OnWaterListener listener)
    {
        listeners.Add(listener);
    }

    public void AddListener(Action<OnWaterMessage> action)
    {
        actions.Add(action);
    }

    public void RemoveListener(OnWaterListener listener)
    {
        listeners.Remove(listener);
    }

    public void RemoveListener(Action<OnWaterMessage> action)
    {
        actions.Remove(action);
    }

    public void Notify()
    {
        OnWaterMessage msg = new OnWaterMessage();
        
        if (!Photon.Pun.PhotonNetwork.IsMasterClient)
        {
            NotifyMaster( GetType().Name, msg);
        }else
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