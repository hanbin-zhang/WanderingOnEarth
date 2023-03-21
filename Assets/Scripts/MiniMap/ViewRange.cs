using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewRange : MonoBehaviour
{
    public Transform Camera;
    public Transform Player;
    public GameObject plane;
    private float planeShiftDistance;
    
    // Start is called before the first frame update
    void Start()
    {
        // this.transform.parent = Camera;
        planeShiftDistance = DistanceToVertex(plane);
    }

    // Update is called once per frame
    void Update()
    {

        this.transform.position = new Vector3(Player.position.x, Player.position.y + 15f, Player.position.z);
        Vector3 direction =  Quaternion.Euler(new Vector3(0, Camera.eulerAngles.y, 0)) * Vector3.forward;
        transform.position += direction.normalized * planeShiftDistance;
        Quaternion newRotation = Quaternion.Euler(90f, Camera.eulerAngles.y+45f, 0f);
        transform.rotation = newRotation;
    }

    public float DistanceToVertex(GameObject plane)
    {

        // Calculate the distance between the center and the vertex
        float distance = plane.transform.localScale[0] * plane.transform.localScale[0] * 2.0f;
        distance = Mathf.Sqrt(distance) / 2.0f;

        return distance;
    }
}
