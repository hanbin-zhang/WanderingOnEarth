using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EarthPlant : MonoBehaviour
{
    public Transform earth;
    public GameObject tree;
    
    public float sceneWidth;
    public float sceneHeight;

    private float earthRadius;

    // Start is called before the first frame update
    void Start()
    {
        earthRadius = GetComponent<SphereCollider>().radius;
        
        foreach (Vector3 pos in PlayerPlanting.plantTrees)
        {
            Vector3 transformPosition = EarthPosition(pos);
            Instantiate(tree, transformPosition, EarthRotation(transformPosition), earth);
        }            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector3 EarthPosition(Vector3 pos)
    {        
        float px = pos.x;
        float py = -pos.z;
        float phi = Mathf.PI * (2 * px / sceneWidth - 1);
        float theta = Mathf.PI * py / sceneHeight;
        Vector3 earthPos = new Vector3(
            earthRadius * Mathf.Sin(theta) * Mathf.Sin(phi),
            earthRadius * Mathf.Cos(theta),
            earthRadius * Mathf.Sin(theta) * Mathf.Cos(phi));
        return earthPos;
    }

    private Quaternion EarthRotation(Vector3 pos)
    {
        Vector3 dir = earth.position - pos;
        return Quaternion.LookRotation(dir.normalized)*Quaternion.Euler(-90,0,0);
    }
}
