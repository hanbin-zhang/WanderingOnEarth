using UnityEngine;

public class SafeState : BaseState
{
    public SafeState() => stateLabel = StateLabel.SAFE;
    public override void Handle(StateProperty stateProperty, BaseMessage msg)
    {

        if (msg is OnPlantEvent.OnPlantMessage) {

        }

        if (msg is OnWaterEvent.OnWaterMessage) {

        }

        if (msg is OnLandPrepEvent.OnLandPrepMessage) {

        }

    }

    
}
