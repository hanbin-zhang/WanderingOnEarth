using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Photon.Pun;
using Newtonsoft.Json;

public abstract class NaturalObject : MonoBehaviour, IPunObservable
{

    public List<GameObject> Models;
    public Vector3 localShift;
    public float growTime;
    [HideInInspector] public int blockID = 0;
    public float baseGreenValue = 0;
    [HideInInspector] public float CreatedAt;
    [HideInInspector] public int currentState = 0;
    [HideInInspector] public GameObject currentModel;
    [HideInInspector] public int parentWorldObjID;
    [HideInInspector] public int currentWorldID;
    [HideInInspector] public int updateModelCommand = 0;
    [HideInInspector] public float nextUpdateTime;
    private StateLabel stateLabel;
    private Dictionary<string, int> NaObjNums;
    public abstract string GetDerivedClassName();

    private void Awake()
    {
        CreatedAt = Time.time;
        nextUpdateTime = CreatedAt + growTime;
    }
    void Start()
    {
        currentWorldID = GetInstanceID();
        UpdateObject();
        GameObjectTracker.gameObjects.Add(this);
        AddSpecificCache(GetDerivedClassName());
        PhotonView photonView = gameObject.GetComponent<PhotonView>();
        Debug.Log($"matching{photonView.Owner.ActorNumber}");
        Debug.Log($"matching{PhotonNetwork.LocalPlayer.ActorNumber}");

        if (PhotonNetwork.IsMasterClient &&
            photonView.Owner.ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
        {
            
            Manager.EventController.Get<OnPlantEvent>()?.Notify(transform.position, transform.rotation, gameObject.name.Replace("(Clone)", ""));
        }
    }

    public abstract string CheckUpdateCondition(StateProperty stateProperty);

    public void SetNewUpdateTime()
    {
        nextUpdateTime = Time.time + growTime;
    }

    public float GetUpdateTime()
    {
        return nextUpdateTime;
    }

    public void AddSpecificCache(string className)
    {
        if (GameObjectTracker.objectCount.ContainsKey(className))
        {
            GameObjectTracker.objectCount[className]++;
        }
        else GameObjectTracker.objectCount[className] = 1;
    }

    public float GetCurrentGreenValue()
    {
        return baseGreenValue * (1.0f + currentState);
    }

    [PunRPC]
    public void UpdateObject()
    {
        if (currentModel == null)
        {
            currentModel =  Instantiate(Models[currentState], transform.position+localShift, transform.rotation);
        }
        else
        {
            Vector3 pos = currentModel.transform.position;
            Quaternion rot = currentModel.transform.rotation;
            Destroy(currentModel);
            currentModel = Instantiate(Models[currentState], pos, rot);
        }
    }

    [PunRPC]
    public void UpdateState()
    {
        if (currentState < Models.Count - 1)
        {
            currentState++;
            nextUpdateTime += growTime;
        }
        else Debug.Log("maximum states");
    }
    
    public void RPCUpdateState()
    {
        gameObject.GetComponent<PhotonView>().RPC(nameof(UpdateState), RpcTarget.All);
    }

    public void RPCUpdate() {
        gameObject.GetComponent<PhotonView>().RPC(nameof(UpdateObject), RpcTarget.All);
    }

    public void SetState(int targetState)
    {
        if (targetState <= Models.Count - 1 || targetState >= 0)
        {
            currentState = targetState;
        }
        else Debug.Log("invalid states");
    }

    // return true is certain condition is meet
    // false otherwise
    public abstract string CheckPlaceCondtion();

    // check whether the sum green value of a area meet a certain threshold
    public bool GreenValueJudger(float greenThreshold)
    {
        float sum = 0;
        NaturalObject[] naturalObjects = FindObjectsOfType<NaturalObject>();
        foreach (NaturalObject naturalObject in naturalObjects)
        {
            sum += GetCurrentGreenValue() * (naturalObject.currentState + 1.0f);
        }
        if (sum >= greenThreshold) { return true; }
        else { return false; }
    }

    // check whehter number of a kind of Object meet a certain number

    public bool ObjNumberJudger<T>(int ObjNumber) where T : MonoBehaviour
    {
        T[] naturalObjects = FindObjectsOfType<T>();
        return naturalObjects.Length >= ObjNumber;
    }

    // with one more state constraint for those object has to have a certain state larger than
    // the required state
    public bool ObjNumberJudger<T>(int ObjNumber, int requiredState) where T : MonoBehaviour
    {
        int num = 0;
        T[] naturalObjects = FindObjectsOfType<T>();
        foreach (T naturalObject in naturalObjects)
        {
            NaturalObject no = naturalObject as NaturalObject;
            if (no.currentState >= requiredState) num++;
        }
        return num >= ObjNumber;
    }

    // check with whether number of an object with a certain tag
    // meet the condition
    public bool TagNumberJudger(int ObjNumber, string tagName)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tagName);
        return gameObjects.Length >= ObjNumber;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We are the owner of this GameObject and are sending data to others.
            stream.SendNext(currentState);
        }
        else
        {
            // We are receiving data from the owner of this GameObject.
            currentState = (int)stream.ReceiveNext();
        }
    }
}