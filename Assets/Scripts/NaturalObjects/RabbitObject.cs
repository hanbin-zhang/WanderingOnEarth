using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitObject : NaturalObject
{
    public override string CheckPlaceCondtion()
    {
        return null;
    }

    public override string GetDerivedClassName()
    {
        return GetType().Name;
    }
    
}
