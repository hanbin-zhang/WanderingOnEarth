using System.Collections.Generic;
using UnityEngine;

public class NaObjManager : MonoBehaviour
{
    public static List<NaturalObject> evolvingNaObjs = new();
    private static readonly object evolvingLock = new();

    public static void Register(NaturalObject naturalObject)
    {
        lock (evolvingLock)
        {
            int index = evolvingNaObjs.BinarySearch(naturalObject, new NaturalObjectComparer());
            if (index < 0) index = ~index;
            evolvingNaObjs.Insert(index, naturalObject);
        }
    }

    private void Update()
    {
        lock (evolvingLock)
        {
            float currentTime = Time.time;
            while (evolvingNaObjs.Count > 0 && evolvingNaObjs[0].GetUpdateTime() <= currentTime)
            {
                NaturalObject naturalObject = evolvingNaObjs[0];
                

                naturalObject.UpdateState();
                naturalObject.UpdateObject();

                // update remote
                /*Photon.Pun.PhotonView remoteView = naturalObject.GetComponent<Photon.Pun.PhotonView>();
                remoteView.RPC(nameof(NaturalObject.UpdateObject), Photon.Pun.RpcTarget.Others);*/

                evolvingNaObjs.RemoveAt(0);

                if (naturalObject.currentState < naturalObject.Models.Count-1)
                {
                    int index = evolvingNaObjs.BinarySearch(naturalObject, new NaturalObjectComparer());
                    if (index < 0) index = ~index;
                    evolvingNaObjs.Insert(index, naturalObject);
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