using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class sychronizeState : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        GameObjectTracker.StateSynchronizer = this.gameObject;
    }

    [PunRPC]
    public void NotifyServerLandPrep(Vector3 pos)
    {
        Manager.EventController.Get<OnLandPrepEvent>()?.Notify(pos);
    }

    [PunRPC]
    public void ServerStatePropertyCallback(string serializedStateP)
    {
        Manager.StateController.DeserializeStatesProperty(serializedStateP);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(ServerStatePropertyCallback), newPlayer, Manager.StateController.SerializeStatesProperty());
        }
    }
}
