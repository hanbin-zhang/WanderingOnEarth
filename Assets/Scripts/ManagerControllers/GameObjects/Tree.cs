using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : LiveObject
{
    public override bool IsEvolveConditionSatisfied(out string reason) {
        reason = "";
        if (Manager.GameObjectManager.Get<Bush>().Count <= 2) {
            reason = $"Tree evolves need 3 bushes, currently have {Manager.GameObjectManager.Get<Bush>().Count}";
            return false;
        }
        return true;
    }

    public override bool IsPlantable(Vector3 pos, out string reason) {
        reason = "";        
        
        if (Manager.StateController.GetRegionalStateProperty(pos).state == StateLabel.POLLUTED)
        {
            reason = "Polluted area. Press O to start planting";

            return false;
        }
        if (Manager.GameObjectManager.Get<Bush>().Count <= 2) {
            reason = $"Tree requires 3 bushes to grow";
            return true;
        }
        return true;
    }



}
