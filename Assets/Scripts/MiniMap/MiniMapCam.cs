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
        transform.SetPositionAndRotation(new Vector3(targetCamera.position.x, defaultPosY, targetCamera.position.z), Quaternion.Euler(90, targetCamera.eulerAngles.y, 0));
    }
}