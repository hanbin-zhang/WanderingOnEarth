using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCam : MonoBehaviour
{
    public Transform targetCamera;
    public Transform playerBody;

    float defaultPosY;

    // Start is called before the first frame update
    void Start()
    {
        defaultPosY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Apply position
        // Apply rotation
        transform.SetPositionAndRotation(new Vector3(playerBody.position.x, defaultPosY, playerBody.position.z), Quaternion.Euler(90, 0, 0));
        //transform.position = new Vector3(playerBody.position.x, defaultPosY, playerBody.position.z);
    }
}