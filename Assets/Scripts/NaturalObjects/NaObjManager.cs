using System.Collections.Generic;
using UnityEngine;

public class NaObjManager : MonoBehaviour
{
    public static List<NaturalObject> evolvingNaObjsQueue = new();
    private static readonly object evolvingLock = new();

    private void Start()
    {
        if (!Photon.Pun.PhotonNetwork.IsMasterClient)
        {
            this.enabled = false;
        }
    }

    public static void Register(NaturalObject naturalObject)
    {
        lock (evolvingLock)
        {
            int index = evolvingNaObjsQueue.BinarySearch(naturalObject, new NaturalObjectComparer());
            if (index < 0) index = ~index;
            evolvingNaObjsQueue.Insert(index, naturalObject);
        }
    }

    private void Update()
    {
        lock (evolvingLock)
        {
            float currentTime = Time.time;
            while (evolvingNaObjsQueue.Count > 0 && evolvingNaObjsQueue[0].GetUpdateTime() <= currentTime)
            {
                NaturalObject naturalObject = evolvingNaObjsQueue[0];


                if (naturalObject == null)
                {
                    evolvingNaObjsQueue.RemoveAt(0);
                    continue;
                }

                naturalObject.UpdateState();
                naturalObject.UpdateObject();

                // update remote
                Photon.Pun.PhotonView remoteView = naturalObject.GetComponent<Photon.Pun.PhotonView>();
                remoteView.RPC(nameof(NaturalObject.UpdateState), Photon.Pun.RpcTarget.Others);
                remoteView.RPC(nameof(NaturalObject.UpdateObject), Photon.Pun.RpcTarget.Others);

                evolvingNaObjsQueue.RemoveAt(0);

                StateProperty stateProperty = Manager.Instance.StateController
                        .GetStateProperty(naturalObject.transform.position);
                lock (stateProperty)
                {
                    if (naturalObject.currentState < naturalObject.Models.Count - 1)
                    {


                        if (naturalObject.CheckUpdateCondition(stateProperty) is null)
                        {
                            int index = evolvingNaObjsQueue.BinarySearch(naturalObject, new NaturalObjectComparer());
                            if (index < 0) index = ~index;
                            evolvingNaObjsQueue.Insert(index, naturalObject);
                        }
                        else
                        {
                            stateProperty.PendingNaObjs.Add(naturalObject);
                        }

                    }
                    else stateProperty.EvolvedNaObjs.Add(naturalObject);
                }
            }
        }
    }

    private class NaturalObjectComparer : IComparer<NaturalObject>
    {
        public int Compare(NaturalObject a, NaturalObject b)
        {
            if (a.GetUpdateTime() < b.GetUpdateTime()) return -1;
            if (a.GetUpdateTime() > b.GetUpdateTime()) return 1;
            return 0;
        }
    }
}