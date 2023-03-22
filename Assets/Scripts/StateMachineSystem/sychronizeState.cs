using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class sychronizeState : MonoBehaviour
{
    private void Start()
    {
        GameObjectTracker.StateSynchronizer = this.gameObject;
    }
    [PunRPC]
    public void notifyStateMachineRPC(string typeName, BaseMessage msg)
    {
        switch (typeName)
        {
            case nameof(OnPlantEvent):
                OnPlantEvent.OnPlantMessage plantMsg = msg.Of<OnPlantEvent.OnPlantMessage>();
                Manager.EventController.Get<OnPlantEvent>()?.Notify(plantMsg.pos, plantMsg.rotation, plantMsg.name);
                break;
            case nameof(OnLandPrepEvent):
                OnLandPrepEvent.OnLandPrepMessage landMsg = msg.Of<OnLandPrepEvent.OnLandPrepMessage>();
                Manager.EventController.Get<OnLandPrepEvent>()?.Notify(landMsg.pos);
                break;
            case nameof(OnLeftMouseDownEvent):
                OnLeftMouseDownEvent.OnWaterMessage leftMsg = msg.Of<OnLeftMouseDownEvent.OnWaterMessage>();
                Manager.EventController.Get<OnLeftMouseDownEvent>()?.Notify();
                break;
            case nameof(OnWaterEvent):
                OnWaterEvent.OnWaterMessage waterMsg = msg.Of<OnWaterEvent.OnWaterMessage>();
                Manager.EventController.Get<OnWaterEvent>()?.Notify();
                break;
            default:
                Debug.Log($"Incorrect event type:{typeName}");
                break;
        }

    }
}
