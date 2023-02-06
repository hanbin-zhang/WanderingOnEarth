using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NaturalObject : MonoBehaviour
{

    public List<GameObject> Prefabs;

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
        
    }

    public void UpdateObject()
    {
        if (currentModel != null)
        {
            Destroy(currentModel);
        }

        currentModel = Instantiate(Prefabs[currentState], transform);

        currentModel.transform.localPosition = this.transform.localPosition;
    }

    public void UpdateState()
    {
        if (currentState < Prefabs.Count - 1)
        {
            currentState++;
        }
        else
        {   
            currentState = 0;
        }
    }

}
