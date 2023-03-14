using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Movable : MonoBehaviour
{
    
    private bool readyToRotate;
    private bool readyToSpeed;
    private float speed;

    public float minRotateAngle;
    public float maxRotateAngle;
    public float minSpeed;
    public float maxSpeed;

    public float escapeAngle;

    private bool newObj = true;

    private Rigidbody rb;
    public Vector3 centerOfMass;

    // Start is called before the first frame update
    void Start()
    {
        readyToSpeed= true;
        Invoke(nameof(resetRotation), Random.Range(5f, 10f));
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        //if (PlayerPlanting.preview && newObj) return;
       
        transform.position += transform.forward * speed;
       
        if (readyToRotate)
        {
            float rotateAngle = Random.Range(minRotateAngle, maxRotateAngle);
            transform.Rotate(Quaternion.Euler(0, rotateAngle, 0).eulerAngles);
            readyToRotate = false;
            Invoke(nameof(resetRotation), Random.Range(5f, 10f));
        }
        if (readyToSpeed)
        {
            speed = Random.Range(minSpeed, maxSpeed);
            readyToSpeed = false;
            Invoke(nameof(resetSpeed), Random.Range(5f, 10f));
        }

        newObj = false;

    }
    private void resetRotation()
    {
        readyToRotate = true;
    }

    private void resetSpeed()
    {
        readyToSpeed= true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag != "Ground")
        {
            transform.Rotate(Quaternion.Euler(0, escapeAngle, 0).eulerAngles);
            //transform.rotation *= Quaternion.Euler(0, escapeAngle, 0);           
        }
    }
}
