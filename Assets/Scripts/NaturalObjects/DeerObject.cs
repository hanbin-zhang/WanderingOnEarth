using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class DeerObject : NaturalObject
{
    public override void AddSpecificCache()
    {
        GameObjectTracker.DeerCounnt += 1;
    }

    public override bool CheckPlaceCondtion()
    {
        return true;
    }
}