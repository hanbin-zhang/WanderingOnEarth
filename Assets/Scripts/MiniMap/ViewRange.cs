using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewRange : MonoBehaviour
{
    public Transform Camera;
    public Transform Player; 
    
    // Start is called before the first frame update
    void Start()
    {
        this.transform.parent = Camera;
    }

    // Update is called once per frame
    void Update()
    {

        this.transform.position = new Vector3(Camera.position.x, Player.position.y + 15f, Camera.position.z);
        Vector3 direction =  Quaternion.Euler(new Vector3(0, Camera.eulerAngles.y, 0)) * Vector3.forward;
        transform.position += direction.normalized * 130f;
        Quaternion newRotation = Quaternion.Euler(90f, Camera.eulerAngles.y+45f, 0f);
        transform.rotation = newRotation;
    }
}
