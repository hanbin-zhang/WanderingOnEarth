using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenValueStats : MonoBehaviour
{
    private void FixedUpdate()
    {
        float sumGreenValue = 0;
        NaturalObject[] naObjs = FindObjectsOfType<NaturalObject>();
        if (naObjs.Length > 0 || naObjs != null)
        {   
            foreach (NaturalObject obj in naObjs)
            {
                sumGreenValue += obj.GetCurrentGreenValue();
            }
            
        }
        Debug.Log(sumGreenValue);
    }
}
