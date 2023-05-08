using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : LiveObject
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
        
        int regionalFlowerCount = Manager.GameObjectManager
            .GetRegionalGameObjects<Flower>(pos).Count;
        if (regionalFlowerCount < 3)
        {
            reason = $"Insufficient plants. Pig needs 3 flowers";
            return false;
        }
        return true;
    }
}
