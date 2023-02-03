using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region Private Serializable Fields
    
    [SerializeField]
    private byte maxPlayersPerRoom = 4;
    
    //The Ui Panel to let the user enter name, connect and play
    [SerializeField]
    private GameObject controlPanel;
    
    //The UI Label to inform the user that the connection is in progress
    [SerializeField]
    private GameObject progressLabel;
    
    #endregion
    
    #region Private Fields

    //This client's version number.
    string gameVersion = "1";
    
    // Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
    bool isConnecting;

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
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    #endregion


    #region Public Methods

    // starting point to connect to Photon Cloud
    public void Connect()
    {
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);
        // check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // first and foremost connect to Photon Online Server.
            // keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
            isConnecting = PhotonNetwork.ConnectUsingSettings();
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
        if (isConnecting)
        {
            // join a potential existing room. else called back with OnJoinRandomFailed()
            PhotonNetwork.JoinRandomRoom();
            isConnecting = false;
        }
    }
    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed(): No random room available");
        PhotonNetwork.CreateRoom(null, new RoomOptions{MaxPlayers = maxPlayersPerRoom});
    }

    public override void OnJoinedRoom()
    {
        //only load if is the first player, else rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            int Room = Random.Range(1,5);
            Debug.LogFormat("We load the World {0}", Room);
            // Load the Room Level.
            PhotonNetwork.LoadLevel("World " + Room);
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
        Debug.LogWarningFormat(
            "OnDisconnected(): Called after disconnecting from the Photon server with reason {0}", cause);
        isConnecting = false;
    }

    #endregion
}
