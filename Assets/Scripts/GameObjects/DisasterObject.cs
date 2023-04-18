using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public abstract class DisasterObject : BaseObject
{
    public int Disasterlife;

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient) this.enabled = false;
    }

    public void StartDisasterLifeCycle()
    {
        Invoke(nameof(DisasterLifeCycle), 1f);
    }

    public abstract void DisasterLifeCycle();

    [PunRPC]
    public void SetGObjActive(bool state)
    {
        gameObject.SetActive(state);
    }
}