﻿using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class NormalState : BaseState
{
    public NormalState() => stateLabel = StateLabel.NORMAL;

    public override StateLabel Handle(StateProperty stateProperty, BaseMessage msg)
    {
        switch (msg)
        {
            case OnPlantEvent.OnPlantMessage:
                OnPlantEvent.OnPlantMessage plantMsg = msg.Of<OnPlantEvent.OnPlantMessage>();
                //stateProperty.greenValue++;
                Debug.Log($"you can not plant things on the polluted land, 这是一个onplant事件，pos位置是{plantMsg.pos}, green value是{stateProperty.greenValue}");
                GameObject gameObject = plantMsg.NaturalObject.gameObject;
                Debug.Log($"TIME NOw {Time.time}");
                Debug.Log($"Time update {plantMsg.NaturalObject.nextUpdateTime}");
                Debug.Log($"{plantMsg.NaturalObject.currentState}");
                NaObjManager.Register(plantMsg.NaturalObject);
                //if (stateProperty.greenValue > 5) return StateLabel.NORMAL;
                break;
            case OnWaterEvent.OnWaterMessage:
                OnWaterEvent.OnWaterMessage waterMsg = (OnWaterEvent.OnWaterMessage)msg;
                Debug.Log($"这是一个onwater事件，没有pos");
                break;
            case OnLandPrepEvent.OnLandPrepMessage:
                OnLandPrepEvent.OnLandPrepMessage prepLandMsg = msg.Of<OnLandPrepEvent.OnLandPrepMessage>();
                //stateProperty.greenValue++;
                Debug.Log($"这是一个prepland事件，pos位置是{prepLandMsg.pos}");
                break;
        }
        return stateLabel;
    }
}

