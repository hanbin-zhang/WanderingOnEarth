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
            reason = "It's polluted, cannot put anything";

            return false;
        }

        int regionalTreeCount = Manager.GameObjectManager
            .GetRegionalGameObjects<Tree>(pos).Count;

        if (regionalTreeCount < 3)
        {
            reason = $"Need 3 tree to plant, currently have {regionalTreeCount}";
            return false;
        }

        return true;
    }
}
