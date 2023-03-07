using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TreeObject : NaturalObject
{
    public override string CheckPlaceCondtion()
    {   
        return null;
    }

    public override string CheckUpdateCondition(StateProperty stateProperty)
    {   
        switch (currentState)
        {
            case 0:
                int bushNumber = 0;
                stateProperty.NaObjNums.TryGetValue(nameof(BrushObject), out bushNumber);
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
