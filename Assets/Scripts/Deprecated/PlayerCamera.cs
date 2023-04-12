using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PlayerCamera : MonoBehaviour
{
   /* public GameObject player;
    public float sensX;
    public float sensY;


    [HideInInspector] private float xRotation;
    [HideInInspector] private float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {

        CursorManage();

    }

    private void CursorManage()
    {
        // get cursor
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Cursor.visible) return;

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        xRotation = Mathf.Clamp(xRotation - mouseY, -50f, 50f);
        yRotation += mouseX;

        // rotate camera
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

        // rotate player
        player.transform.rotation = Quaternion.Euler(0, yRotation, 0);

    }*/

}