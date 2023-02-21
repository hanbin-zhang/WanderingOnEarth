using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushObject : NaturalObject
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
