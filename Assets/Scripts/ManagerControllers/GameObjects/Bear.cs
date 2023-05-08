using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : LiveObject
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
        
        int regionalTreeCount = Manager.GameObjectManager
            .GetRegionalGameObjects<Tree>(pos).Count;
        if (regionalTreeCount < 3)
        {
            reason = $"Insufficient plants. Bear needs 3 trees";
            return false;
        }
        return true;
    }
}
