using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeObject : NaturalObject
{
    public override bool CheckPlaceCondtion()
    {
        bool cond = true;
        // check the whether the green value is larger than 1.0f
        cond = cond && GreenValueJudger(1.0f);
        // check whether there is at least a deer
        cond = cond && ObjNumberJudger<DeerObject>(1);
        // check whether there is at least a deer with a state at least 1
        cond = cond && ObjNumberJudger<DeerObject>(1, 1);
        // check whether there is at least 1 object tagged "Animal"
        cond = cond && TagNumberJudger(1, "Animal");
        return cond;
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
        if (Input.GetKeyDown(KeyCode.T))
        {

            Debug.Log(ObjNumberJudger<TreeObject>(1, 1));
        }
    }
}
