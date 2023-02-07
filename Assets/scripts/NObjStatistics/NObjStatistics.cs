using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NObjStatistics : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(GetSumGreenValue());
        } 
    }

    public float GetSumGreenValue()
    {   
        float sum = 0;
        NaturalObject[] naturalObjects = FindObjectsOfType<NaturalObject>();
        foreach (NaturalObject naturalObject in naturalObjects)
        {
            sum += naturalObject.GetCurrentGreenValue() * (naturalObject.currentState+1.0f);
        }
        return sum;
    }
}


