using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;


public class OnLandPrepEvent : BaseEvent
{
    
    public interface OnLandPrepListener
    {
        public void OnEvent(OnLandPrepMessage msg);
    }

    public class OnLandPrepMessage : BaseMessage
    {  
        public Vector3 pos { get; set; }
    }

    private List<OnLandPrepListener> listeners = new();
    private List<Action<OnLandPrepMessage>> actions = new();

    public void AddListener(OnLandPrepListener listener)
    {
        listeners.Add(listener);
    }

    public void AddListener(Action<OnLandPrepMessage> action)
    {
        actions.Add(action);
    }

    public void RemoveListener(OnLandPrepListener listener)
    {
        listeners.Remove(listener);
    }

    public void RemoveListener(Action<OnLandPrepMessage> action)
    {
        actions.Remove(action);
    }

    public void Notify(Vector3 pos)
    {
        var msg = new OnLandPrepMessage()
        {
            pos = pos
        };

        /*if (!PhotonNetwork.IsMasterClient)
        {
            PhotonView view = GameObjectTracker.StateSynchronizer.GetComponent<PhotonView>();
            
            view.RPC(nameof(sychronizeState.NotifyServerLandPrep), RpcTarget.MasterClient, pos);
        }*/
        Debug.Log("Onlandprp notified");
        listeners.ForEach((x) => x.OnEvent(msg));
        actions.ForEach((x) => x.Invoke(msg));
    }

    public void Clear()
    {
        listeners.Clear();
        actions.Clear();
    }


}


