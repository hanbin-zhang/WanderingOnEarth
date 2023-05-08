using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : LiveObject
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
        
        // int regionalGrassCount = Manager.GameObjectManager
        //     .GetRegionalGameObjects<Grass>(pos).Count;
        // if (regionalGrassCount < 3)
        // {
        //     reason = $"Insufficient plants. Rabbit needs 3 grasses";
        //     return false;
        // }
        return true;
    }
}
