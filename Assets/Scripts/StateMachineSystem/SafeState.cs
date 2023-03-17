using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeState : BaseState
{
    public SafeState() => stateLabel = StateLabel.SAFE;
    public override void Handle(StateProperty stateProperty, BaseMessage msg)
    {
        Debug.Log("现在是SAFE状态！");

        switch (msg)
        {
            case OnPlantEvent.OnPlantMessage:
                OnPlantEvent.OnPlantMessage plantMsg = msg.Of<OnPlantEvent.OnPlantMessage>();


                /*GameObject gameObject = PhotonNetwork.Instantiate(plantMsg.name, plantMsg.pos, plantMsg.rotation);
                NaturalObject naturalObject = gameObject.GetComponent<NaturalObject>();
                if (naturalObject.CheckUpdateCondition(stateProperty) is null)
                {
                    stateProperty.EvolvingNaObjs.Add(naturalObject);
                    NaObjManager.Register(naturalObject);
                }
                else
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
                        stateProperty.PendingNaObjs.RemoveAt(i);
                    }
                }
*/
                if (plantMsg.name == "TreeMain") stateProperty.treeNumber++;
                

                Debug.Log($"这是一个onplant事件，pos位置是{plantMsg.pos}, tree Number是{stateProperty.treeNumber}");
                break;
            case OnLeftMouseDownEvent.OnWaterMessage:
                OnLeftMouseDownEvent.OnWaterMessage waterMsg = (OnLeftMouseDownEvent.OnWaterMessage)msg;
                Debug.Log($"这是一个onwater事件，没有pos");
                break;
            case OnLandPrepEvent.OnLandPrepMessage:
                OnLandPrepEvent.OnLandPrepMessage prepLandMsg = msg.Of<OnLandPrepEvent.OnLandPrepMessage>();
                //stateProperty.greenValue++;
                Debug.Log($"这是一个prepland事件，pos位置是{prepLandMsg.pos}");
                break;
        }
    }

    
}
