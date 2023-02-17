using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaObjMananger : MonoBehaviour
{
    // Lock object for thread synchronization
    private object lockObject = new object();

    // List of all instances of natural objects
    private List<NaturalObject> allInstances = new List<NaturalObject>();

    // Registers a new instance of a natural object
    public void Register(NaturalObject obj)
    {
        lock (lockObject)
        {
            allInstances.Add(obj);
        }
    }

    // Unregisters an instance of a natural object
    public void Unregister(NaturalObject obj)
    {
        lock (lockObject)
        {
            allInstances.Remove(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Update natural objects based on their state
    }
}