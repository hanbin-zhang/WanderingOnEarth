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
        return NaObjNumberTrackers[y, x];
    }
}