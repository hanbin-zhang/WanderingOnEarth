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
                OnPlantEvent.OnPlantMessage plantMsg = msg.Of<OnPlantEvent.OnPlantMessage>();
                Debug.Log($"you can not plant things on the polluted land, 这是一个onplant事件，pos位置是{plantMsg.pos}, green value是{stateProperty.greenValue}");

                GameObject gameObject = Photon.Pun.PhotonNetwork.Instantiate(plantMsg.name, plantMsg.pos, plantMsg.rotation);
                NaturalObject naturalObject = gameObject.GetComponent<NaturalObject>();
                if (naturalObject.CheckUpdateCondition(stateProperty) is null) {
                    stateProperty.EvolvingNaObjs.Add(naturalObject);
                    NaObjManager.Register(naturalObject);
                } else
                {
                    stateProperty.PendingNaObjs.Add(naturalObject);
                }

                stateProperty.AddCount(naturalObject.GetDerivedClassName());

                for (int i = stateProperty.PendingNaObjs.Count - 1; i >= 0; i--)
                {
                    if (stateProperty.PendingNaObjs[i].CheckUpdateCondition(stateProperty) is null)
                    {
                        stateProperty.EvolvingNaObjs.Add(stateProperty.PendingNaObjs[i]);
                        stateProperty.PendingNaObjs[i].SetNewUpdateTime();
                        NaObjManager.Register(stateProperty.PendingNaObjs[i]);
                        stateProperty.EvolvingNaObjs.RemoveAt(i);
                    }
                }

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

