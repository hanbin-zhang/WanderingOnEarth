using UnityEngine;

public class NormalState : BaseState
{
    public NormalState() => stateLabel = StateLabel.NORMAL;

    public override void Handle(StateProperty stateProperty, BaseMessage msg)
    {
        if (msg is OnPlantEvent.OnPlantMessage) {
            OnPlantEvent.OnPlantMessage plantMsg = msg.Of<OnPlantEvent.OnPlantMessage>();
            
            if (Manager.GameObjectManager.GetRegionalGameObjects<Tree>(stateProperty).Count >= 10) {
                stateProperty.SetState(StateLabel.SAFE);
            }
        }

        if (msg is OnWaterEvent.OnWaterMessage) {
            OnWaterEvent.OnWaterMessage waterMsg = msg as OnWaterEvent.OnWaterMessage;
        }

        if (msg is OnLandPrepEvent.OnLandPrepMessage) {
            OnLandPrepEvent.OnLandPrepMessage prepLandMsg = msg.Of<OnLandPrepEvent.OnLandPrepMessage>();
        }
    
    }
}

