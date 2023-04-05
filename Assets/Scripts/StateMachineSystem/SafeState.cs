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
