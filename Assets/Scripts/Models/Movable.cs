using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    // Start is called before the first frame update
    void Start()
    {
        readyToSpeed= true;
        Invoke(nameof(resetRotation), Random.Range(5f, 10f));
        
    }

    // Update is called once per frame
    void Update()
    {
        // go straight 
        
        transform.position = transform.position + transform.forward * speed;

        if (readyToRotate)
        {
            float rotateAngle = Random.Range(minRotateAngle, maxRotateAngle);
            transform.rotation = Quaternion.Euler(0, rotateAngle, 0);
            readyToRotate = false;
            Invoke(nameof(resetRotation),Random.Range(5f,10f));
        }
        if (readyToSpeed)
        {
            speed = Random.Range(minSpeed, maxSpeed);
            readyToSpeed= false;
            Invoke(nameof(resetSpeed), Random.Range(5f, 10f));
        }

    }
    private void resetRotation()
    {
        readyToRotate= true;
    }

    private void resetSpeed()
    {
        readyToSpeed= true;
    }

}
