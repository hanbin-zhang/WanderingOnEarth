using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TreeObject : NaturalObject
{
    public override void AddSpecificCache()
    {
        Debug.Log(this.GetType().Name);
        if (GameObjectTracker.objectCount.ContainsKey(this.GetType().Name))
        {
            GameObjectTracker.objectCount[this.GetType().Name]++;
        } else GameObjectTracker.objectCount[this.GetType().Name] = 1;
    }

    public override bool CheckPlaceCondtion()
    {
        return true;
    }
}
