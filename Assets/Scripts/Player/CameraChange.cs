
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraChange : MonoBehaviour
{
    public GameObject deleteCamera;
    private Dictionary<string, GameObject> dict;


    // Update is called once per frame
    void Update()
    {
        LoadingObj();

    }

    private void LoadingObj()
    {
        dict = new Dictionary<string, GameObject>();
        var activeScene = SceneManager.GetActiveScene();
        var rootGos = activeScene.GetRootGameObjects();

        foreach (var rootGo in rootGos)
        {
            if (rootGo.name == "Player(Clone)")
            {
                deleteCamera.SetActive(false);
            }
        }
    }
}
