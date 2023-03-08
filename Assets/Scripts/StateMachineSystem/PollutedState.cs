using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class PollutedState : BaseState
{
    public PollutedState() => stateLabel = StateLabel.POLLUTED;


    public override void Handle(StateProperty stateProperty, BaseMessage msg)
    {
        if (msg is OnPlantEvent.OnPlantMessage)
        {
            
            OnPlantEvent.OnPlantMessage plantMsg = msg.Of<OnPlantEvent.OnPlantMessage>();
            stateProperty.greenValue++;
            Debug.Log($"这是一个onplant事件，pos位置是{plantMsg.pos}, green value是{stateProperty.greenValue}");
            if (stateProperty.greenValue > 5) stateProperty.SetState(StateLabel.NORMAL);
        }
        else if (msg is OnWaterEvent.OnWaterMessage)
        {
            OnWaterEvent.OnWaterMessage waterMsg = (OnWaterEvent.OnWaterMessage)msg;
            Debug.Log($"这是一个onwater事件，没有pos");            
        }    
    }
}

