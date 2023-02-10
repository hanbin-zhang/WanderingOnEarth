using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Planting : MonoBehaviour
{
    public GameObject crossHair;
    public GameObject tree;
    public GameObject deer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
        Vector3 plantingPosition = transform.position + transform.forward * 2;

        Vector3 plantPoint;

        if (IsValid(plantingPosition, out plantPoint)) {
            
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(tree, plantPoint, transform.rotation);
            }

            if (Input.GetMouseButtonDown(1))
            {
                Instantiate(deer, plantPoint, transform.rotation);
            }
        }

    }

    private bool IsValid(Vector3 rayOrigin1, out Vector3 point)
    {
        rayOrigin1.y = 1000;

        bool valid = false;
        point = default;
        Ray ray = new Ray(rayOrigin1, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            point = hit.point;
            // Check for collisions with existing objects
            Collider[] colliders = Physics.OverlapSphere(point, 1f);
            // terrain is a collider
            valid = hit.collider.gameObject.name == "Terrain1" && colliders.Length <= 1;
        }
       
        crossHair.GetComponent<Image>().color = valid ? Color.green : Color.red;
        
        return valid;
    }
}