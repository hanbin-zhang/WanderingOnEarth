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
        switch (msg)
        {
            case OnPlantEvent.OnPlantMessage:
                OnPlantEvent.OnPlantMessage plantMsg = msg.Of<OnPlantEvent.OnPlantMessage>();
                //stateProperty.greenValue++;
                Debug.Log($"you can not plant things on the polluted land, 这是一个onplant事件，pos位置是{plantMsg.pos}, green value是{stateProperty.treeNumber}");
                //if (stateProperty.greenValue > 5) stateProperty.SetState(StateLabel.NORMAL);
                break;
            case OnLeftMouseDownEvent.OnWaterMessage:
                OnLeftMouseDownEvent.OnWaterMessage waterMsg = (OnLeftMouseDownEvent.OnWaterMessage)msg;
                Debug.Log($"这是一个onwater事件，没有pos");
                break;
            case OnLandPrepEvent.OnLandPrepMessage:
                OnLandPrepEvent.OnLandPrepMessage prepLandMsg = msg.Of<OnLandPrepEvent.OnLandPrepMessage>();
                //stateProperty.greenValue++;
                Debug.Log($"这是一个prepland事件，pos位置是{prepLandMsg.pos}");
                stateProperty.SetState(StateLabel.NORMAL);
                break;
        }
               
    }
}

