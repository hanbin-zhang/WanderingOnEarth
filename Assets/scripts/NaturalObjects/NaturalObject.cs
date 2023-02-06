using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NaturalObject : MonoBehaviour
{

    public List<GameObject> Prefabs;

    public int blockID = 0;
    public float greenValue = 0; 
    public int currentState = 0;
    public GameObject currentModel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (currentModel != null)
        {
            currentModel.transform.tr = this.transform;
        }*/
    }

    public void UpdateObject()
    {   
        
        if (currentModel != null)
        {
            Destroy(currentModel);
        }

        currentModel = Instantiate(Prefabs[currentState], transform.position, transform.rotation);
        currentModel.transform.parent= transform;
        //currentModel.transform.localPosition = this.transform.localPosition;
    }

    public void UpdateState()
    {
        if (currentState < Prefabs.Count - 1)
        {
            currentState++;
        }
        else throw new ArgumentOutOfRangeException("maximum states");
    }

    public void SetState(int targetState)
    {
        if (targetState <= Prefabs.Count - 1 || targetState >= 0)
        {
            currentState = targetState;
        }
        else throw new ArgumentOutOfRangeException("invalid states");
    }

    public bool GreenValueJudger(float greenThreshold)
    {
        float sum = 0;
        NaturalObject[] naturalObjects = FindObjectsOfType<NaturalObject>();
        foreach (NaturalObject naturalObject in naturalObjects)
        {
            sum += naturalObject.greenValue * (naturalObject.currentState + 1.0f);
        }
        if (sum >= greenThreshold) { return true; }
        else { return false; }
    }
}
