using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public abstract class LiveObject : BaseObject, IPunInstantiateMagicCallback
{

    public new static LiveObject From(GameObject obj) => obj.GetComponent<LiveObject>();

    public static bool TryFrom(GameObject gameObject, out LiveObject liveObject) =>
        gameObject.TryGetComponent<LiveObject>(out liveObject);
    
  
    public List<float> growingTime;

    public List<float> greenValue;

    [HideInInspector]
    private GameObject currentModel;

    [HideInInspector]
    public int age = 0;

    [HideInInspector]
    public float currentGreenValue;

    [HideInInspector]
    private List<GameObject> children = new List<GameObject>();

    [HideInInspector]
    private bool waitEvolveConditionSatisfied = false;
    
    new protected void Awake() {
        base.Awake();

        foreach (Transform child in transform)  {
            children.Add(child.gameObject);
        }

        if (children.Count != growingTime.Count + 1) {
            throw new UnityException("[LiveObject] Growing time should be matched with models");
        }

        if (children.Count != greenValue.Count) {
            throw new UnityException("[LiveObject] Green values should be matched with models");
        }
        
        if (growingTime.Count > 0 && (photonView.IsMine || !photonView.IsOwnerActive)) {
            Invoke(nameof(Evolve), growingTime[age]);
        }
        SetModel(age);
    }

    [PunRPC]
    public void SetModel(int index) {
        foreach (GameObject child in children)  {
            child.SetActive(false);
        }
        currentModel = children[index];
        currentModel.SetActive(true);
        currentGreenValue = greenValue[index];
        age = index;
        Manager.EventController.Get<OnEvolveEvent>()?.Notify(currentGreenValue);
    }

    private void Evolve() {
        if (!(age < growingTime.Count)) return;
        if (IsEvolveConditionSatisfied(out var reason)) {
            if (waitEvolveConditionSatisfied) {
                waitEvolveConditionSatisfied = false;
                Invoke(nameof(Evolve), growingTime[age]);
                return;
            }
            age++;
            SetModel(age);
            photonView.RPC(nameof(SetModel), RpcTarget.Others, age);
            PhotonNetwork.SendAllOutgoingCommands();
            if (age < growingTime.Count) {
                Invoke(nameof(Evolve), growingTime[age]);
            }
        } else {
            Debug.Log($"[LiveObject] Unable to evolve: {reason}");
            waitEvolveConditionSatisfied = true;
            Invoke(nameof(Evolve), 1f);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        if (PhotonNetwork.IsMasterClient) {
            photonView.RPC(nameof(SetModel), newPlayer, age);
            PhotonNetwork.SendAllOutgoingCommands();
        }
    }
    
    public abstract bool IsPlantable(Vector3 pos, out string reason);

    public abstract bool IsEvolveConditionSatisfied(out string reason);

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        // Debug.LogError($"SentServerTime {info.SentServerTime} \n \n \n SentServerTimestamp{info.SentServerTimestamp}");
    }
}
