using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthPlant : MonoBehaviour
{
    public Transform earth;
    public GameObject tree;
    public float earthRadius;
     
    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos1 = new Vector3(0,0,0);
        //Instantiate(tree, EarthPosition(pos1), Quaternion.identity, earth);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*private Vector3 EarthPosition(Vector3 pos)
    {
        

       // return newPos;
    }*/
}
