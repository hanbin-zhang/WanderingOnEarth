using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransformEarth : MonoBehaviour
{

    private float spd = 5f;
    private float scale = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //Input.GetAxis("MouseX")获取鼠标移动的X轴的距离
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");
            Vector3 rot = new Vector3(y, -x, 0);
            transform.Rotate(spd * rot, Space.World);
        }

        if(Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            scale += Input.GetAxis("Mouse ScrollWheel");
            transform.localScale = new Vector3(scale, scale, scale);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("PlayScene");
        }
    }
}
