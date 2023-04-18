using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public abstract class DisasterObject : BaseObject
{
    public int Disasterlife;

    public void StartDisasterLifeCycle()
    {
        Invoke(nameof(DisasterLifeCycle), 1f);
    }

    public abstract void DisasterLifeCycle();
}