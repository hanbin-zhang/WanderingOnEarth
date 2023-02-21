using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (SceneManager.GetActiveScene().name == "BigEarth")
            {
                SceneManager.LoadScene("PlayScene");
            }
            if(SceneManager.GetActiveScene().name == "PlayScene")
            {
                SceneManager.LoadScene("BigEarth");
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            
        }
    }
}
