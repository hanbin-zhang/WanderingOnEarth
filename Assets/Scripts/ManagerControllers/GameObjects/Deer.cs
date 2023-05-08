using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deer : LiveObject
{
    
    public override bool IsEvolveConditionSatisfied(out string reason)
    {
        reason = "";
        return true;
    }

    public override bool IsPlantable(Vector3 pos, out string reason)
    {
        reason = "";

        if (Manager.StateController.GetRegionalStateProperty(pos).state == StateLabel.POLLUTED)
        {
            reason = "Polluted area. Press O to start planting";

            return false;
        }

        int regionalBushCount = Manager.GameObjectManager
            .GetRegionalGameObjects<Bush>(pos).Count;

        if (regionalBushCount < 3)
        {
            reason = $"Insufficient plants. Deer needs 3 bushes";
            return false;
        }

        return true;
    }
}
