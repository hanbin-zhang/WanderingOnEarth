using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;
using Photon.Pun;

public abstract class BaseEvent { 
    public void NotifyMaster(string className, BaseMessage msg)
    {
        PhotonView view = GameObjectTracker.StateSynchronizer.GetComponent<PhotonView>();
        Debug.Log("notigfy master"+className);
        view.RPC(nameof(sychronizeState.notifyStateMachineRPC), RpcTarget.MasterClient, className);
    }
}

[Serializable]
public abstract class BaseMessage
{
    public T Of<T>() where T : BaseMessage => (T)this;
}
