using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region Photon Callbacks
    
    // Called when the local player left the room. the game's logic can clean up it's internal state.
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
    
    //Called when a remote player entered the room. This Player is already added to the playerlist.
    public override void OnPlayerEnteredRoom(Player other)
    {
        // not seen if you're the player connecting
        Debug.LogFormat("OnPlayerEnteredRoom() {0} joined the room", other.NickName); 

        if (PhotonNetwork.IsMasterClient)
        {
            // called before OnPlayerLeftRoom
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0} someone joined", PhotonNetwork.IsMasterClient); 
            LoadArena();
        }
    }
    
    //Called when a remote player left the room or became inactive. Check otherPlayer.IsInactive.
    public override void OnPlayerLeftRoom(Player other)
    {
        // seen when other disconnects
        Debug.LogFormat("OnPlayerLeftRoom() {0} left the room", other.NickName); 

        if (PhotonNetwork.IsMasterClient)
        {
            // called before OnPlayerLeftRoom
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0} someone is leaving", PhotonNetwork.IsMasterClient); 
            LoadArena();
        }
    }


    #endregion
    
    #region Private Methods
    
    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            return;
        }
        
        int RoomNum = Random.Range(1,5);
        Debug.LogFormat("Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        
        //wraps loading a level asynchronously and pausing network messages during the process.
        PhotonNetwork.LoadLevel("World " + RoomNum);
    }
    
    #endregion
    
    #region Public Methods

    public void LeaveRoom()
    {
        //Leave the current room and return to the Master Server where you can join or create rooms
        PhotonNetwork.LeaveRoom();
    }

    #endregion
}
