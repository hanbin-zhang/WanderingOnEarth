using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NaturalObject : MonoBehaviour
{

    public List<GameObject> Models;

    public int blockID = 0;
    public float baseGreenValue = 0; 
    public int currentState = 0;
    public GameObject currentModel;
    public int parentWorldObjID;

    public float GetCurrentGreenValue()
    {
        return baseGreenValue * (1.0f + currentState);
    }

    public void UpdateObject()
    {   
        
        if (currentModel != null)
        {
            Destroy(currentModel);
        }

        currentModel = Instantiate(Models[currentState], transform.position, transform.rotation);
        currentModel.transform.parent= transform;
        //currentModel.transform.localPosition = this.transform.localPosition;
    }

    public void UpdateState()
    {
        if (currentState < Models.Count - 1)
        {
            currentState++;
        }
        else throw new ArgumentOutOfRangeException("maximum states");
    }

    public void SetState(int targetState)
    {
        if (targetState <= Models.Count - 1 || targetState >= 0)
        {
            currentState = targetState;
        }
        else throw new ArgumentOutOfRangeException("invalid states");
    }

    // return true is certain condition is meet
    // false otherwise
    public abstract bool CheckPlaceCondtion();

    // check whether the sum green value of a area meet a certain threshold
    public bool GreenValueJudger(float greenThreshold)
    {
        float sum = 0;
        NaturalObject[] naturalObjects = FindObjectsOfType<NaturalObject>();
        foreach (NaturalObject naturalObject in naturalObjects)
        {
            sum += GetCurrentGreenValue() * (naturalObject.currentState + 1.0f);
        }
        if (sum >= greenThreshold) { return true; }
        else { return false; }
    }

    // check whehter number of a kind of Object meet a certain number

    public bool ObjNumberJudger<T>(int ObjNumber) where T : MonoBehaviour
    {
        T[] naturalObjects = FindObjectsOfType<T>();
        return naturalObjects.Length >= ObjNumber;
    }

    // with one more state constraint for those object has to have a certain state larger than
    // the required state
    public bool ObjNumberJudger<T>(int ObjNumber, int requiredState) where T : MonoBehaviour
    {   
        int num = 0;
        T[] naturalObjects = FindObjectsOfType<T>();
        foreach (T naturalObject in naturalObjects)
        {
            NaturalObject no = naturalObject as NaturalObject;
            if (no.currentState >= requiredState) num++;
        }
        return num >= ObjNumber;
    }

    // check with whether number of an object with a certain tag
    // meet the condition
    public bool TagNumberJudger(int ObjNumber, string tagName)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tagName);
        return gameObjects.Length >= ObjNumber;
    }
}
