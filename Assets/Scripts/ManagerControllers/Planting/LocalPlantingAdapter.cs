using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LocalPlantingAdapter : GameObjectOperationAdapter
{
    public void Destroy(EventController eventController, GameObject gameObject)
    {
        GameObject.Destroy(gameObject);
    }

    public GameObject Instantiate(EventController eventController, GameObject gameObject, Vector3 pos, Quaternion rotation)
    {
        GameObject obj = GameObject.Instantiate(gameObject, pos, rotation);
        return obj;
    }
}
