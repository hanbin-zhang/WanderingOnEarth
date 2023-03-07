using System;
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
                OnPlantHandler(stateProperty, msg);
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

    private void OnPlantHandler(StateProperty stateProperty, BaseMessage msg)
    {
        OnPlantEvent.OnPlantMessage plantMsg = msg.Of<OnPlantEvent.OnPlantMessage>();
        Debug.Log($"这是一个onplant事件，pos位置是{plantMsg.pos}, green value是{stateProperty.greenValue}");


        NaturalObject instance = (NaturalObject)Activator.CreateInstance(Type.GetType(plantMsg.NaObjName));
        string placeCond = instance.CheckPlaceCondtion(stateProperty);
        if (placeCond is null)
        {
            GameObject gameObject = Photon.Pun.PhotonNetwork.Instantiate(plantMsg.name, plantMsg.pos, plantMsg.rotation);
            NaObjManager.Register(gameObject.GetComponent<NaturalObject>());
        }
        else Debug.Log(placeCond);
    }
}

