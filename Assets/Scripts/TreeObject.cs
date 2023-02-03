using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TreeObject : PlantObject
{   
    //initialize params for this class
    public void InitializeParam(int currentState,
                      string createBy,
                      Vector3 translation,
                      Quaternion rotation)
    {
        // load object configs from config files
        ObjectData config = ObjConfigParser.OCP("Assets/configs/treeObjConfig.json");

        // set the attributes
        this.stateLimit = config.stateLimit;

        this.currentState = currentState;
        this.createBy = createBy;
        this.position = translation;
        this.rotation = rotation;

        this.createTime = DateTime.Now;

        
        for (int i = 0; i < config.stateLimit; i++)
        {
            this.modelPaths.Add(i, config.States[i].ModelPath);
            //this.modelScales.Add(i, config.States[i].ScalingFactor);
            this.modelScales.Add(i, new Vector3(config.States[i].ScalingFactor[0],
                config.States[i].ScalingFactor[1],
                config.States[i].ScalingFactor[2]));

        }

        LoadModelGameObj();
    }
}
