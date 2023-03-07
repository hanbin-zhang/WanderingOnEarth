using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class DeerObject : NaturalObject
{
    public override string CheckPlaceCondtion(StateProperty stateProperty)
    {
        if (GameObjectTracker.objectCount.ContainsKey(typeof(TreeObject).Name))
        {
             if (GameObjectTracker.objectCount[typeof(TreeObject).Name] < 3)
            {
                return "need 3 trees";
            };
        } else return "need 3 trees";

        return null;
    }
    public override string GetDerivedClassName()
    {
        return GetType().Name;
    }
}