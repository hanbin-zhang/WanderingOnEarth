using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region Private Serializable Fields
    
    [SerializeField]
    private byte maxPlayersPerRoom = 1;
    
    #endregion
    
    #region Private Fields

    //This client's version number.
    string gameVersion = "1";

    #endregion

    #region MonoBehaviour CallBacks
    // called on GameObject by Unity during early initialization phase.
    
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
    }

    #endregion


    #region Public Methods

    // starting point to connect to Photon Cloud
    public void Connect()
    {
        // check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // first and foremost connect to Photon Online Server.
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }
    #endregion

    #region MonoBehaviourPunCallbacks Callbacks  
    //Called when the client is connected to the Master Server and ready for matchmaking and other tasks
    public override void OnConnectedToMaster()
    {
        Debug.Log(
            "OnConnectedToMaster(): Called when the client is connected to the Master Server and ready for matchmaking and other tasks");
        // join a potential existing room. else called back with OnJoinRandomFailed()
        PhotonNetwork.JoinRandomRoom();
    }
    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed(): No random room available,Calling: PhotonNetwork.CreateRoom");
        PhotonNetwork.CreateRoom(null, new RoomOptions{MaxPlayers = maxPlayersPerRoom});
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom(): this client is in a room.");
    }
    
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat(
            "OnDisconnected(): Called after disconnecting from the Photon server with reason {0}", cause);
    }
    

    #endregion
}
