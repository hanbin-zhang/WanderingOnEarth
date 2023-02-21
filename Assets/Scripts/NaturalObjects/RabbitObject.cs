using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitObject : NaturalObject
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
