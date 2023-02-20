using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class DeerObject : NaturalObject
{
    public override bool CheckPlaceCondtion()
    {
        return GameObjectTracker.objectCount[typeof(TreeObject).Name] >= 3;
    }
    public override string GetDerivedClassName()
    {
        return GetType().Name;
    }
}