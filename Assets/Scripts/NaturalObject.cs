using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

// a abstract class for object that can be placed and evolved as turn goes
abstract public class NaturalObject : MonoBehaviour
{
    public GameObject modelGameObject;
    public DateTime createTime;
    public int currentState;
    public int stateLimit;
    public string createBy;
    public Vector3 position;
    public Quaternion rotation;
    public Dictionary<int, string> modelPaths = new();
    public Dictionary<int, Vector3> modelScales = new();

    // update the state of a object
    // throw ArgumentOutOfRangeException when beyond range
    public void UpdateState()
    {
        // if not reach the max state plus 1
        // states are ints from 0 to the max state
        if (currentState+1 < stateLimit)
        {
            currentState += 1;
        }
        else throw new ArgumentOutOfRangeException("already reach maximum state");
    }

    // set the state to the target state
    // throw ArgumentOutOfRangeException when beyond range
    public void SetState(int targetState)
    {
        // set a state
        // if beyond range throw exception

        if (targetState < stateLimit || targetState >= 0)
        {
            currentState = targetState;
        }
        else throw new ArgumentOutOfRangeException("invalid target state" + targetState);
    }

    // Destroy the model game object
    private void DestroyModelGameObj()
    {
        if (modelGameObject is not null)
        {
            Destroy(modelGameObject);
            modelGameObject = null;
        }
        
    }

    // load the model
    public void LoadModelGameObj()
    {   
        // try to destroy the old model
        DestroyModelGameObj();

        
        GameObject modelObject = AssetDatabase.LoadAssetAtPath<GameObject>(this.modelPaths[currentState]);

        modelGameObject = Instantiate(modelObject);
        modelGameObject.transform.position = position;
        modelGameObject.transform.rotation = rotation;

        // need to remove, current place holder
        modelGameObject.transform.localScale = this.modelScales[currentState];
    }
}
