using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TreeObject : NaturalObject
{
    public override bool CheckPlaceCondtion()
    {
        return true;
    }

    public override string GetDerivedClassName()
    {
        return GetType().Name;
    }
}
