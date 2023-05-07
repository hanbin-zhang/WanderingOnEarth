using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RemotePlantingAdapter : GameObjectOperationAdapter
{
    public void Destroy(EventController eventController, GameObject gameObject)
    {
        PhotonNetwork.Destroy(gameObject);
    }

    public GameObject Instantiate(EventController eventController, GameObject gameObject, Vector3 pos, Quaternion rotation)
    {
        GameObject obj = PhotonNetwork.Instantiate(gameObject.name, pos, rotation);
        return obj;
    }
}
