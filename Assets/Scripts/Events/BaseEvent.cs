using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.Pun;


public abstract class BaseEvent { 
    public void NotifyMaster(string className, BaseMessage msg)
    {
        PhotonView view = GameObjectTracker.StateSynchronizer.GetComponent<PhotonView>();
        view.RPC(nameof(sychronizeState.notifyStateMachineRPC), RpcTarget.MasterClient, className, msg);
    }
}

public abstract class BaseMessage
{
    public T Of<T>() where T : BaseMessage => (T)this;
}
