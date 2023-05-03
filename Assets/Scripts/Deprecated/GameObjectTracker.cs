using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public static class GameObjectTracker
{
    public static List<NaturalObject> gameObjects = new();
    // ADD to this list when player collect it
    public static Dictionary<int, string> collected = new();
    // add to this list after the achivement window show it
    public static List<int> updatedcollected = new();
    
    // record the name of each object
    public static Dictionary<string, int> objectCount = new();
    public static List<GameObject> playerObjects = new();
    public static BoundaryManager boundaryManager;
    public static GameObject StateSynchronizer;

    public static Dictionary<string, int>[,] NaObjNumberTrackers;
    public static Dictionary<string, int> GetRegionObjNumber(Vector3 position)
    {
        int x = (int)Mathf.Clamp(position.x / Manager.StateController.regionSize, 0, Manager.StateController.nColumns - 1);
        int y = (int)Mathf.Clamp(position.z / Manager.StateController.regionSize, 0, Manager.StateController.nRows - 1);

        // check if the dictionary is null and instantiate it if needed
        if (NaObjNumberTrackers == null)
        {
            NaObjNumberTrackers = new Dictionary<string, int>[Manager.StateController.nRows, Manager.StateController.nColumns];
            for (int i = 0; i < Manager.StateController.nRows; i++)
            {
                for (int j = 0; j < Manager.StateController.nColumns; j++)
                {
                    NaObjNumberTrackers[i, j] = new Dictionary<string, int>();
                }
            }
        }

        return NaObjNumberTrackers[y, x];
    }

    public static void AddNaObj(Vector3 position, string className)
    {
        Dictionary<string, int> regionNaObj = GetRegionObjNumber(position);
        if (regionNaObj.ContainsKey(className))
        {
            regionNaObj[className]++;
        }
        else
        {
            regionNaObj.Add(className, 1);
        }

    }

    public static int GetNaObj(Vector3 position, string className)
    {
        Dictionary<string, int> regionNaObj = GetRegionObjNumber(position);
        if (regionNaObj.ContainsKey(className))
        {
            return regionNaObj[className];
        }
        else
        {
            return 0;
        }

    }
}