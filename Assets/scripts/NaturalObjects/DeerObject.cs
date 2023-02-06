using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerObject : NaturalObject
{
    // Start is called before the first frame update
    void Start()
    {
        this.greenValue = 6.0f;
        //this.currentModel = Prefabs[0];
        this.UpdateObject();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.P))
        {

            //this.UpdateObject();
            this.UpdateState();
            this.UpdateObject();
        }*/
    }
}
