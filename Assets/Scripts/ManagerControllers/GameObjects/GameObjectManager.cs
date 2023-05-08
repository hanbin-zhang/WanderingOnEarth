using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameObjectManager
{
    private EventController eventController;
    private StateController stateController;

    private Dictionary<string, List<GameObject>> globalGameObjects = new Dictionary<string, List<GameObject>>();
    private List<Dictionary<string, List<GameObject>>> regionalGameObjects = new List<Dictionary<string, List<GameObject>>>();

    private List<GameObject> players = new List<GameObject>();

    private Dictionary<string, int> stocks = new Dictionary<string, int>();
    
    public GameObjectManager(EventController eventController, StateController stateController) {
        this.eventController = eventController;
        this.stateController = stateController;
        foreach (var _ in stateController.StatesProperty) {
            regionalGameObjects.Add(new Dictionary<string, List<GameObject>>());
        }
    }

    private List<GameObject> InternalGet(string name) {
        // if (globalGameObjects[name] == null){
        //     globalGameObjects[name].Remove()
        //     return null;
        // }
        if (!globalGameObjects.ContainsKey(name)) {
           globalGameObjects[name] = new List<GameObject>();
        }
        return globalGameObjects[name];
    }

    private List<GameObject> InternalRegionalGet(int index, string name) {
        if (!regionalGameObjects[index].ContainsKey(name)) {
           regionalGameObjects[index][name] = new List<GameObject>();
        }
        return regionalGameObjects[index][name];
    }

    public Dictionary<string, List<GameObject>> GetAll()
    {
        return globalGameObjects;
    }

    public void Add(GameObject obj) {
        InternalGet(obj.name).Add(obj);
        int region = Manager.StateController.GetRegionalIndex(obj.transform.position);
        InternalRegionalGet(region, obj.name).Add(obj);
    }

    public void Remove(GameObject obj)
    {       
        if (obj != null){
            InternalGet(obj.name).Remove(obj);
            int region = Manager.StateController.GetRegionalIndex(obj.transform.position);
            InternalRegionalGet(region, obj.name).Remove(obj);
        }       
    }

    public List<GameObject> Get(string name) {
        return InternalGet(name);
    }

    public List<GameObject> Get<T>() where T : LiveObject {
        return Get(typeof(T).Name);
    }

    public List<GameObject> GetRegionalGameObjects(int region, string name) {
        return InternalRegionalGet(region, name);
    }

    public List<GameObject> GetRegionalGameObjects<T>(int region) where T : LiveObject {
        return GetRegionalGameObjects(region, typeof(T).Name);
    }

    public List<GameObject> GetRegionalGameObjects(StateProperty stateProperty, string name) {
        return InternalRegionalGet(stateProperty.index, name);
    }

    public List<GameObject> GetRegionalGameObjects<T>(StateProperty stateProperty) where T : LiveObject {
        return GetRegionalGameObjects(stateProperty.index, typeof(T).Name);
    }

    public List<GameObject> GetRegionalGameObjects(Vector3 pos, string name) {
        int region = Manager.StateController.GetRegionalIndex(pos);
        return InternalRegionalGet(region, name);
    }

    public List<GameObject> GetRegionalGameObjects<T>(Vector3 pos) where T : LiveObject {
        return GetRegionalGameObjects(pos, typeof(T).Name);
    }

    public void AddPlayer(GameObject player) {
        players.Add(player);
    }

    public List<GameObject> Players => players;
    public List<GameObject> GetPlayers() => players;

    public int GetGlobalGreenValue() {
        float greenValue = 0;
        foreach (var kvp in globalGameObjects) {
            foreach (GameObject gameObject in kvp.Value) {
                if (gameObject == null){
                    Remove(gameObject);
                }else{
                    if (LiveObject.TryFrom(gameObject, out var liveObject)) {
                        greenValue += liveObject.currentGreenValue;
                    }
                }
            }
        }
        return (int)Math.Round(greenValue);
    }

    public int GetRegionalGreenValue(Vector3 pos)
    {
        float greenValue = 0;
        int regionalIndex = Manager.StateController.GetRegionalIndex(pos);
        foreach (var kvp in regionalGameObjects[regionalIndex])
        {
            foreach (GameObject gameObject in kvp.Value)
            {
                if (gameObject is null){
                    Remove(gameObject);
                }else{
                    if (LiveObject.TryFrom(gameObject, out var liveObject)) {
                        greenValue += liveObject.currentGreenValue;
                    }
                }
            }
        }
        return (int)Math.Round(greenValue);
    }

  

    public override string ToString() {
        string output = "[GameObjectManager]\n";
        foreach (KeyValuePair<string, List<GameObject>> kvp in globalGameObjects) {
            output += $"[GameObject] {kvp.Key} \n[Count] {kvp.Value.Count} \n[";
            kvp.Value.ForEach((gameObject) => {
                if (gameObject.TryGetComponent<PhotonView>(out var obj))
                {
                    output += $"{obj.ViewID} ";
                }                
            });
            output += "]\n======================================\n";
        }
        return output;
    }

    public void MyDebug(Action<string> action) {
        action.Invoke(ToString());
    }
}
