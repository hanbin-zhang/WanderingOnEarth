using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerObject : NaturalObject
{
    public override bool CheckPlaceCondtion()
    {
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //this.currentModel = Prefabs[0];
        this.UpdateObject();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
