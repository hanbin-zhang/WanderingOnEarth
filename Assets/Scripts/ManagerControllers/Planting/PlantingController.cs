using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlantingController
{

    private GameObjectOperationAdapter adapter;

    private EventController eventController;

    public PlantingController(EventController eventController, GameObjectOperationAdapter adapter) {
        this.eventController = eventController;
        this.adapter = adapter;
    }

    public GameObject Plant(LiveObject liveObject, Vector3 pos, Quaternion rotation) {              
        return adapter.Instantiate(eventController, liveObject.gameObject, pos, rotation);              
    }

    public void Collect(GameObject gameObject) {
        adapter.Destroy(eventController, gameObject);
    }
    
}

public interface GameObjectOperationAdapter {
    public GameObject Instantiate(EventController eventController, GameObject gameObject, Vector3 pos, Quaternion rotation);
    
    public void Destroy(EventController eventController, GameObject gameObject);


}



