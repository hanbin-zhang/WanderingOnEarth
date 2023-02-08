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
        this.transform.transform.position = new Vector3
        (
         transform.transform.position.x,
         transform.transform.position.y + 4,
         transform.transform.position.z
        );

        this.UpdateObject();
    }

    // Update is called once per frame
    void Update()
    {

    }
}