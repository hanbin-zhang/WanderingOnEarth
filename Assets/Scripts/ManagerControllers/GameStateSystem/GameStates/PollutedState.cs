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
        if (msg is OnPlantEvent.OnPlantMessage) {
            OnPlantEvent.OnPlantMessage plantMsg = msg as OnPlantEvent.OnPlantMessage;
        }

        if (msg is OnWaterEvent.OnWaterMessage) {
            OnWaterEvent.OnWaterMessage waterMsg = msg as OnWaterEvent.OnWaterMessage;
        }

        if (msg is OnLandPrepEvent.OnLandPrepMessage) {
            OnLandPrepEvent.OnLandPrepMessage prepLandMsg = msg as OnLandPrepEvent.OnLandPrepMessage;
            stateProperty.SetState(StateLabel.NORMAL);
        }
       
               
    }
}

