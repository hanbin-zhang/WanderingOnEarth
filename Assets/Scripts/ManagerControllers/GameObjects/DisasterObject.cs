using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public abstract class DisasterObject : BaseObject
{
    public int Disasterlife;

    [HideInInspector]
    public SceneObjectManager sceneManager;

    [HideInInspector]
    public int lifeCounter;

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient) this.enabled = false;
    }

    public void StartDisasterLifeCycle(SceneObjectManager objectManager)
    {
        Invoke(nameof(DisasterLifeCycle), 1f);
        sceneManager = objectManager;

        Vector3 randomPos = sceneManager.RandomTerrainPosition(3f);

        Debug.Log($"random pos {randomPos}");
        transform.position = randomPos;
    }

    public abstract void DisasterLifeCycle();

    [PunRPC]
    public void SetGObjActive(bool state)
    {
        gameObject.SetActive(state);
    }
}