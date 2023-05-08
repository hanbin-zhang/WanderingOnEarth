using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public abstract class LiveObject : BaseObject, IPunInstantiateMagicCallback
{
    private void Start()
    {
        Manager.EventController.Get<OnPlantEvent>()?.Notify(transform.position, transform.rotation, name);
    }

    public new static LiveObject From(GameObject obj) => obj.GetComponent<LiveObject>();

    public static bool TryFrom(GameObject gameObject, out LiveObject liveObject) {
        // if (gameObject.TryGetComponent<PhotonView>(out var obj))
        // {
        //     Debug.LogError(obj.ViewID);
        //     return gameObject.TryGetComponent<LiveObject>(out liveObject);
        // } 
        // liveObject = null;
        // return false;
        return gameObject.TryGetComponent<LiveObject>(out liveObject);
    }
        
    
  
    public List<float> growingTime;

    public List<float> greenValue;
    public List<float> animalHeights;

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
    [HideInInspector]
    private bool evolveComplete = false;
    
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

        /*if (growingTime.Count > 0 && (photonView.IsMine || !photonView.IsOwnerActive)) {
            Invoke(nameof(Evolve), growingTime[age]);
        }*/

        // old version might cause conflict when 3 or more players joined
        // new version change only trigger the update loop in the master client
        if (PhotonNetwork.IsMasterClient)
        {
            Invoke(nameof(Evolve), growingTime[age]);
        }
        // possible upgrade:
        // 1: distributed the update loop to the creater or the object,
        // when the player quit the give make master client restart the loop
        // minimum load
        // 2: individual update, each client holds there own update loop
        // based on the photon view
        // potential higher cost but could be no crucial

        SetModel(age);
    }

    public void InvokeEvolve()
    {
        Invoke(nameof(Evolve), growingTime[age]);
    }

    [PunRPC]
    public void SetModel(int index) {
        foreach (GameObject child in children)  {
            child.SetActive(false);
        }
        // give deer a hight
        CharacterController animal = GetComponent<CharacterController>();
        if (animal != null){
            animal.height = animalHeights[index];
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
            } else {        
                if(evolveComplete == false){
                    OnEvolveComplete();         
                    // each player control their own obj   
                    photonView.RPC(nameof(OnEvolveComplete), RpcTarget.Others);
                    PhotonNetwork.SendAllOutgoingCommands(); 
                }           
            }
        } else {
            Debug.Log($"[LiveObject] Unable to evolve: {reason}");
            waitEvolveConditionSatisfied = true;
            Invoke(nameof(Evolve), 1f);
        }
    }

    [PunRPC]
    public void OnEvolveComplete() {
        evolveComplete = true;
        // discard
        Manager.Invoke(() => {                  
            Manager.GameObjectManager.Remove(gameObject);   
               
            // if (PhotonNetwork.IsMasterClient){              
            //     Manager.Invoke(() => PhotonNetwork.Destroy(gameObject), 3f, this);
            // }    
            PhotonView photonView = PhotonView.Get(this);
            if(photonView.IsMine){
                Manager.Invoke(() => PhotonNetwork.Destroy(gameObject), 5f, this);
            }
            
            //PhotonNetwork.Destroy(gameObject);
            Manager.EventController.Get<OnEvolveEvent>()?.Notify(currentGreenValue);
        }, 10f, this);
    }

    [PunRPC]
    private void RPCRemove(GameObject obj){
        Manager.GameObjectManager.Remove(obj);
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
