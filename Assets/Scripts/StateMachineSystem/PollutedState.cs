using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class PollutedState : BaseState
{
    public PollutedState() => stateLabel = StateLabel.POLLUTED;


    public override void Handle(StateController stateController, BaseMessage msg)
    {
        switch (msg.eventLabel)
        {
            case EventLabel.ON_PLANT:
                //Vector3 plantMsgPos = ((OnPlantEvent.OnPlantMessage)msg).pos;
                OnPlantEvent.OnPlantMessage plantMsg = msg.Of<OnPlantEvent.OnPlantMessage>();
                stateController.setRegionStateLabel(plantMsg.pos, StateLabel.NORMAL);
                Debug.Log($"这是一个onplant事件，pos位置是{plantMsg.pos}");
                break;

            case EventLabel.ON_WATER:
                OnWaterEvent.OnWaterMessage waterMsg = (OnWaterEvent.OnWaterMessage)msg;
                Debug.Log($"这是一个onwater事件，没有pos");
                break;

            default:
                break;
        }
    }
}

