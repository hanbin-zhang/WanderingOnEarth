using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeObject : NaturalObject
{
    // Start is called before the first frame update
    void Start()
    {
        this.greenValue = 1.0f;
        this.UpdateObject();
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

            Debug.Log(GreenValueJudger(6.0f));
        }
    }
}
