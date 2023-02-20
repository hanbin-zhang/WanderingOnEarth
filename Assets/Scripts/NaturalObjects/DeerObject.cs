using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class DeerObject : NaturalObject
{
    public override void AddSpecificCache()
    {
        Debug.Log(this.GetType().Name);
        if (GameObjectTracker.objectCount.ContainsKey(this.GetType().Name))
        {
            GameObjectTracker.objectCount[this.GetType().Name]++;
        }
        else GameObjectTracker.objectCount[this.GetType().Name] = 1;
    }

    public override bool CheckPlaceCondtion()
    {
        return GameObjectTracker.objectCount[typeof(TreeObject).Name] >= 3;
    }
}