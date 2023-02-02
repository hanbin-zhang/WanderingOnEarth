using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NaObjManager : MonoBehaviour
{
    public List<NaturalObject> objects = new();

    public void AddObject(NaturalObject obj)
    {
        objects.Add(obj);
    }

    public void RemoveObject(NaturalObject obj)
    {
        objects.Remove(obj);
    }

    private static NaObjManager instance;
    public static NaObjManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TreeObject treeObject = gameObject.AddComponent<TreeObject>();
            treeObject.InitializeParam(0,
                "aaa",
                new Vector3(0, 4, 0),
                Quaternion.identity);
            Instance.AddObject(treeObject);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            objects[0].SetState(0);
            objects[0].LoadModelGameObj();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            objects[0].SetState(1);
            objects[0].LoadModelGameObj();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            objects[0].SetState(2);
            objects[0].LoadModelGameObj();
        } else if (Input.GetKeyDown(KeyCode.T))
        {
            ObjectData config = ObjConfigParser.OCP("Assets/configs/treeObjConfig.json");
            Debug.Log(config.objectName);
        }
    }
}
