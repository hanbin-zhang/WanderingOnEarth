using UnityEngine;

public class SafeState : BaseState
{
    public SafeState() => stateLabel = StateLabel.SAFE;
    public override void Handle(StateProperty stateProperty, BaseMessage msg)
    {
        Debug.Log("������SAFE״̬��");

        switch (msg)
        {
            case OnPlantEvent.OnPlantMessage:
                OnPlantEvent.OnPlantMessage plantMsg = msg.Of<OnPlantEvent.OnPlantMessage>();

                if (plantMsg.name == "TreeMain") stateProperty.treeNumber++;
                

                Debug.Log($"����һ��onplant�¼���posλ����{plantMsg.pos}, tree Number��{stateProperty.treeNumber}");
                break;
            case OnLeftMouseDownEvent.OnWaterMessage:
                OnLeftMouseDownEvent.OnWaterMessage waterMsg = (OnLeftMouseDownEvent.OnWaterMessage)msg;
                Debug.Log($"����һ��onwater�¼���û��pos");
                break;
            case OnLandPrepEvent.OnLandPrepMessage:
                OnLandPrepEvent.OnLandPrepMessage prepLandMsg = msg.Of<OnLandPrepEvent.OnLandPrepMessage>();
                //stateProperty.greenValue++;
                Debug.Log($"����һ��prepland�¼���posλ����{prepLandMsg.pos}");
                break;
        }
    }

    
}
