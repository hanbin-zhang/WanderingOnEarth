using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deer : LiveObject
{
    
    public override bool IsEvolveConditionSatisfied(out string reason)
    {
        reason = "";
        return true;
    }

    public override bool IsPlantable(Vector3 pos, out string reason)
    {
        reason = "";
        return true;
    }
}
