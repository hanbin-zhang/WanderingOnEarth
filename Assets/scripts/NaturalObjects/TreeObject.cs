using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TreeObject : NaturalObject
{
    public override string CheckPlaceCondtion(StateProperty stateProperty)
    {
        return null;
    }

    public override string GetDerivedClassName()
    {
        return GetType().Name;
    }
}
