using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class DeerObject : NaturalObject
{
    public override bool CheckPlaceCondtion()
    {
        if (GameObjectTracker.objectCount.ContainsKey(typeof(TreeObject).Name))
        {
            return GameObjectTracker.objectCount[typeof(TreeObject).Name] >= 3;
        } else return false;
    }
    public override string GetDerivedClassName()
    {
        return GetType().Name;
    }
}