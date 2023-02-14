using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerObject : NaturalObject
{
    public override bool CheckPlaceCondtion()
    {
        return true;
    }

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {

            //this.UpdateObject();
            this.UpdateState();
            this.UpdateObject();
        }
    }
}