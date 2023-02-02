using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class State
{
    public string ModelPath;
    public float[] ScalingFactor;
}

[System.Serializable]
public class ObjectData
{
    public string objectName;
    public int stateLimit;
    public int greenValue;
    public State[] States;
}


public static class ObjConfigParser
{
    public static ObjectData OCP(string configFile)
    {
        string configJson = File.ReadAllText(configFile);
        ObjectData obj = JsonUtility.FromJson<ObjectData>(configJson);
        Debug.Assert(obj.stateLimit == obj.States.Length, "States number unmatched with actuall states in the config file");
        return obj;
    }
} 