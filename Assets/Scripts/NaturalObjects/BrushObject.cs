using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushObject : NaturalObject
{
    public override string CheckPlaceCondtion()
    {
        return null;
    }

    public override string CheckUpdateCondition(StateProperty stateProperty)
    {
        return null ;
    }

    public override string GetDerivedClassName()
    {
        return GetType().Name;
    }
}
