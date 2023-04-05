using UnityEngine;

public class TreeObject : NaturalObject
{
    public override string CheckPlaceCondtion()
    {   
        return null;
    }

    public override string CheckUpdateCondition(StateProperty stateProperty)
    {   
        switch (CurrentState)
        {
            case 0:
                int bushNumber = 0;
                GameObjectTracker.GetRegionObjNumber(transform.position).TryGetValue(nameof(BushObject), out bushNumber);
                if (bushNumber < 3)
                {
                    string message = "need 3 bush to proceed to next state";
                    Debug.Log(message);
                    return message;
                }
                break;
        }
        
        return null ;
    }

    public override string GetDerivedClassName()
    {
        return GetType().Name;
    }


}
