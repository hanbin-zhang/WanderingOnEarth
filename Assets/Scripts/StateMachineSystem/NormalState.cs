using UnityEngine;

public class NormalState : BaseState
{
    public NormalState() => stateLabel = StateLabel.NORMAL;

    public override void Handle(StateProperty stateProperty, BaseMessage msg)
    {
        switch (msg)
        {
            case OnPlantEvent.OnPlantMessage:
                OnPlantEvent.OnPlantMessage plantMsg = msg.Of<OnPlantEvent.OnPlantMessage>();
                
                if (plantMsg.name == "TreeMain") stateProperty.treeNumber++;
                if (GameObjectTracker.GetNaObj(plantMsg.pos, nameof(TreeObject)) > 5)
                {

                    /* stateProperty.SetState(StateLabel.SAFE);
                     GameObjectTracker.boundaryManager.InstantiateBoundary(stateProperty);*/
                    Manager.EventController.Get<OnStateChangeEvent>()?.Notify(plantMsg.pos, stateProperty.label, StateLabel.SAFE, false);
                }
                Debug.Log($"这是一个onplant事件，pos位置是{plantMsg.pos}, tree Number是{stateProperty.treeNumber}");
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
        
    }
}

