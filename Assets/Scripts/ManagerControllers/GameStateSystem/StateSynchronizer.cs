using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class StateSynchronizer : MonoBehaviourPunCallbacks
{
    [HideInInspector]
    private new PhotonView photonView;

    private void Awake() {
        photonView = PhotonView.Get(this);
    }

    private void Start() {
        // Listen the regional state change event
        // when the state is changed
        // sync the state with other players with RPC call
        Manager.EventController.Get<OnStateChangeEvent>()?.AddListener((msg) => {
            photonView.RPC(nameof(SetState), RpcTarget.Others, msg.stateProperty);
        });
    }

    [PunRPC]
    private void SetState(StateProperty stateProperty) {
        // Callee gets a RPC call from the caller
        // sync the local regional state to the caller
        Manager.StateController.GetRegionalStateProperty(stateProperty.index).SetState(stateProperty.state);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        // If a new player enters the room
        // the master client will send ALL regional states through RPC
        if (PhotonNetwork.IsMasterClient) {
            Manager.StateController.StatesProperty.ForEach((stateProperty) => {
                photonView.RPC(nameof(SetState), newPlayer, stateProperty);
            });
            PhotonNetwork.SendAllOutgoingCommands();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        // If the leaving player is the master client
        // the new master client should take over the boundary management
        if (PhotonNetwork.IsMasterClient) {
            GetComponent<BoundaryManager>().Init();
        }
    }
    
}
