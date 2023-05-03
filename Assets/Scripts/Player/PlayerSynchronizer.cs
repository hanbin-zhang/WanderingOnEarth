using UnityEngine;
using Photon.Pun;
using StarterAssets;
using Photon.Realtime;

public class PlayerSynchronizer : MonoBehaviourPunCallbacks, IPunObservable
{

    //List of the scripts that should only be active for the local player (ex. PlayerController, MouseLook etc.)
    public MonoBehaviour[] localScripts;
    //List of the GameObjects that should only be active for the local player (ex. Camera, AudioListener etc.)
    public GameObject[] localObjects;
    //Values that will be synced over network
    Vector3 remotePosition;
    Quaternion remoteRotation;

    [HideInInspector]
    private Vector3 movement;

    [HideInInspector]
    private float moveSpeed;

    [HideInInspector]
    private ThirdPersonController controller;

    [HideInInspector]
    private bool animGrounded;

    [HideInInspector]
    private bool animJump;
    
    [HideInInspector]
    private bool animFreeFall;

    [HideInInspector]
    private float animSpeed;

    [HideInInspector]
    private float animMotionSpeed;

    void Awake() {
        Manager.GameObjectManager.AddPlayer(gameObject);
        if (photonView.IsMine) {
            gameObject.tag = "Player";
        } else {
            foreach (var localScript in localScripts) {
                localScript.enabled = false;
            }
            foreach (var localObject in localObjects) {
                localObject.SetActive(false);
            }
        }
        if (TryGetComponent<ThirdPersonController>(out controller)) {
            moveSpeed = controller.MoveSpeed + 2;
            controller.remoteMode = !photonView.IsMine;
        } else {
            throw new UnityException("[PlayerSync] Should be binded with ThirdPersonController");
        }
    }


    void Update() {
        if (!photonView.IsMine) {
            // if new player enter room, drop from high position
            Vector3 lastPosition = transform.position;           
            if (!animJump) transform.position = new Vector3(lastPosition.x, remotePosition.y, lastPosition.z);
            transform.position = Vector3.MoveTowards(transform.position, remotePosition, Time.deltaTime * moveSpeed);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, remoteRotation, Time.deltaTime * 360);
            movement = transform.position - lastPosition;
            if (controller._hasAnimator) {
                controller._animator.SetBool(controller._animIDGrounded, animGrounded);
                controller._animator.SetBool(controller._animIDJump, animJump);
                controller._animator.SetBool(controller._animIDFreeFall, animFreeFall);
                controller._animator.SetFloat(controller._animIDSpeed, animSpeed);
                controller._animator.SetFloat(controller._animIDMotionSpeed, animMotionSpeed);
            }
        }
    }

    [PunRPC]
    public void InitTransformFromOthers(Vector3 position, Quaternion rotation){
        transform.position = position;
        transform.rotation = rotation;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        // If a new player enters the room
        // the other player will sync position through RPC call
        photonView.RPC(nameof(InitTransformFromOthers), newPlayer, transform.position, transform.rotation);
        PhotonNetwork.SendAllOutgoingCommands();
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(controller._animator.GetBool(controller._animIDGrounded));
            stream.SendNext(controller._animator.GetBool(controller._animIDJump));
            stream.SendNext(controller._animator.GetBool(controller._animIDFreeFall));
            stream.SendNext(controller._animator.GetFloat(controller._animIDSpeed));
            stream.SendNext(controller._animator.GetFloat(controller._animIDMotionSpeed));
          
        } else {
            remotePosition = (Vector3)stream.ReceiveNext();
            remoteRotation = (Quaternion)stream.ReceiveNext();
            animGrounded = (bool)stream.ReceiveNext();
            animJump = (bool)stream.ReceiveNext();
            animFreeFall = (bool)stream.ReceiveNext();
            animSpeed = (float)stream.ReceiveNext();
            animMotionSpeed = (float)stream.ReceiveNext();
        
            float lag = Mathf.Abs((float) (PhotonNetwork.Time - info.SentServerTime));
            remotePosition += movement * lag;
        }
    }

}
