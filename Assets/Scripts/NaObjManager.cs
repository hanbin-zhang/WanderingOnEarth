using System.Collections.Generic;
using UnityEngine;

// a class that keep a track of all the object created in a block
// bind this script to a empty GameObject and manipulate everything here.

public class NaObjManager : MonoBehaviour
{   
    // list of object in this block
    public List<NaturalObject> objects = new();
    // list of number of each object in this block and their corresponding green value
    public Dictionary<string, int> objNumber = new();
    public Dictionary<string, int> greenValues = new();

    // add a NaturalObject
    public void AddObject(NaturalObject obj)
    {
        objects.Add(obj);
        objNumber[obj.GetType().Name] += 1;
    }

    // remove a NaturalObject
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

        // load the green values for each object
        // check the file path and file format as you can find in the configs folder
        StatisticsParams statsParamS = StatisticsParser.SP("Assets/configs/objStatisticsConfig.json");

        foreach (StatisticsParam param in statsParamS.statisticsParams)
        {
            objNumber.Add(param.objectName, 0);
            greenValues.Add(param.objectName, param.greenValue);
        }
    }

    // test code 
    // you can remove if you want
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
            StatisticsParams statsParamS = StatisticsParser.SP("Assets/configs/objStatisticsConfig.json");
            Debug.Log(statsParamS.statisticsParams[0]);
        }
    }
}
