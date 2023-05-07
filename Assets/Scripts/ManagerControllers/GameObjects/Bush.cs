using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : LiveObject
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
        return true;
    }
}
