using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class MoveSynchronizer : MonoBehaviourPunCallbacks, IPunObservable
{
  
    private Vector3 remotePosition;
    private Quaternion remoteRotation;

    [HideInInspector]
    private float moveSpeed;

    [HideInInspector]
    private Vector3 movement;

    void Awake() {
        TransferOwnershipToMaster();
    }


    private void TransferOwnershipToMaster() {
        if (photonView.IsMine && !PhotonNetwork.IsMasterClient) {
            photonView.TransferOwnership(PhotonNetwork.MasterClient);
        }
     
        if (TryGetComponent<Movable>(out var movable)) {
            movable.enabled = PhotonNetwork.IsMasterClient;
            this.moveSpeed = movable.moveSpeed + 2f;
        }
        
    }

    void FixedUpdate() {
        if (!PhotonNetwork.IsMasterClient) {
            Vector3 lastPosition = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, remotePosition, Time.fixedDeltaTime * moveSpeed);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, remoteRotation, Time.fixedDeltaTime * 100);
            movement = transform.position - lastPosition;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        } else {
            remotePosition = (Vector3)stream.ReceiveNext();
            remoteRotation = (Quaternion)stream.ReceiveNext();
            float lag = Mathf.Abs((float) (PhotonNetwork.Time - info.SentServerTime));
            remotePosition += movement * lag;
        }
    }

    [PunRPC]
    public void InitTransformFromMaster(Vector3 position, Quaternion rotation) {
        transform.position = position;
        transform.rotation = rotation;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        // If a new player enters the room
        // the master client will send the position and rotation through RPC call
        if (PhotonNetwork.IsMasterClient) {
            photonView.RPC(nameof(InitTransformFromMaster), newPlayer, transform.position, transform.rotation);
            PhotonNetwork.SendAllOutgoingCommands();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        // If the leaving player is the master client
        // the new master client should take over the movement
        if (TryGetComponent<Movable>(out var movable)) {
            movable.enabled = PhotonNetwork.IsMasterClient;
        }
        
    }
}
