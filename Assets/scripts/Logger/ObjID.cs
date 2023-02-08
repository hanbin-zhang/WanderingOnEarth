using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjID : MonoBehaviour
{
    public int ID;

    // Start is called before the first frame update
    void Start()
    {
        ID = GetInstanceID();
    }

}
