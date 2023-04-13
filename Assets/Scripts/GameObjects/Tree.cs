using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : LiveObject
{
    private void Start()
    {
        Manager.EventController.Get<OnPlantEvent>()?.Notify(transform.position, transform.rotation, name);
    }
    public override bool IsEvolveConditionSatisfied(out string reason) {
        reason = "";
        if (Manager.GameObjectManager.Get<Bush>().Count <= 2) {
            reason = $"Tree evolves need 3 bush, currently have {Manager.GameObjectManager.Get<Bush>().Count}";
            return false;
        }
        return true;
    }

    public override bool IsPlantable(Vector3 pos, out string reason) {
        reason = "";        
        
        if (Manager.StateController.GetRegionalStateProperty(pos).state == StateLabel.POLLUTED)
        {
            reason = "It's polluted, cannot put anything";

            return false;
        }
        return true;
    }



}
