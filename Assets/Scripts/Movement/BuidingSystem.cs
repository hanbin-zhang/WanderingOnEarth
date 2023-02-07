using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class BuidingSystem : MonoBehaviour
{
    public GameObject player;
    public GameObject[] objs;
    public GameObject selectedObj;

    public Transform shootingPoint;
    public Transform orientation;

    private Vector3 pos;
    private RaycastHit hit;

    public GameObject[] crossHairs;

    public float treeRadius;


    //public bool canPlace = true;
    private bool plantable;

    private void Update()
    {

        /*if (selectedObj != null)
        {
            Debug.Log(selectedObj);
            selectedObj.transform.position = pos;

            if (Input.GetMouseButtonDown(0))
            {
                PlaceObject();
                //BuildObject(selectedObj);
            }
        }*/

        Legal();
        //Vector3 newPosition = shootingPoint.position + player.transform.forward * 2;
        Vector3 rayOrigin = shootingPoint.position + orientation.forward * 2;
        //Debug.Log(rayOrigin);
        //Debug.Log(transform.position);
        if (Input.GetMouseButtonDown(0) && plantable == true)
        {
            Instantiate(selectedObj, rayOrigin, orientation.rotation);
        }

        // animal
        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hitInfo))
            {
                if (hitInfo.transform.tag == "Obstacle")
                {
                    Destroy(hitInfo.transform.gameObject);
                }
            }
        }
    }

    /*public void PlaceObject()
    {
        // subtract money
        selectedObj = null;
    }

    private void FixedUpdate()
    {
        Vector3 rayOrigin = shootingPoint.position + orientation.forward * 2;
        Ray ray = new Ray(rayOrigin, Vector3.down);
        if (Physics.Raycast(ray, out hit, 1000))
        {
            pos = hit.point;
            // layermask ?

        }
    }

    void BuildObject(GameObject obj)
    {
        Vector3 rayOrigin = shootingPoint.position + orientation.forward * 2;
        if (Physics.Raycast(rayOrigin, Vector3.down, 10))
        {
            Instantiate(obj, rayOrigin, orientation.rotation);
        }
        //Debug.DrawRay(rayOrigin, Vector3.forward, Color.red);
       // Debug.Log(rayOrigin);
    }

    public void SelectObject(int index)
    {
        Debug.Log(selectedObj);
        //Debug.Log("1111111111");
        selectedObj = Instantiate(objs[index], pos, orientation.rotation);
    }
*/



    private void Legal()
    {
        //float radius = 1.5f;
        Vector3 rayOrigin1 = shootingPoint.position + orientation.forward * 2;
        Vector3 rayOrigin2 = new Vector3(rayOrigin1.x, 100, rayOrigin1.z + treeRadius * 2.0f);
        Vector3 rayOrigin3 = new Vector3(rayOrigin1.x - treeRadius, 100, rayOrigin1.z + treeRadius);
        Vector3 rayOrigin4 = new Vector3(rayOrigin1.x + treeRadius, 100, rayOrigin1.z + treeRadius);
        Vector3 rayOrigin5 = new Vector3(rayOrigin1.x, 100, rayOrigin1.z + treeRadius);
        rayOrigin1.y = 100;

        Vector3[] rayOrigins = { rayOrigin1, rayOrigin2, rayOrigin3, rayOrigin4 };
        Color[] colors = { Color.cyan, Color.blue, Color.green, Color.black };
        Debug.DrawRay(rayOrigins[3], Vector3.down, colors[3]);
        Debug.DrawRay(rayOrigins[0], Vector3.down, colors[0]);
        Debug.DrawRay(rayOrigins[1], Vector3.down, colors[1]);
        Debug.DrawRay(rayOrigins[2], Vector3.down, colors[2]);



        int count = 0;
        for (int i = 0; i< rayOrigins.Length ; i++)
        {
            //Vector3 down = new Vector3 (rayOrigins[i].x, rayOrigins[i].y, rayOrigins[i].z);
            //down.z = -1;

            Ray ray = new Ray(rayOrigins[i], Vector3.down);
            //Debug.DrawLine(rayOrigins[i], Vector3.down, Color.red);
            Debug.DrawRay(rayOrigins[i], Vector3.down, Color.green);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000))
            {


                if (hit.collider.gameObject.name == "RealGroundPlane")
                {

                    count++;
                }
            }
            //Debug.Log(count);
            //Debug.Log(hit.collider.gameObject.name);
        }
        
        if (count == rayOrigins.Length)
        {

            for (int i = 0; i < crossHairs.Length; i++)
            {
                crossHairs[i].GetComponent<Image>().color = Color.green;

            }
            
            plantable = true;

        }
        else
        {
            for (int i = 0; i < crossHairs.Length; i++)
            {
                crossHairs[i].GetComponent<Image>().color = Color.red;
                
            }
            
            plantable = false;

        }
    }
}
