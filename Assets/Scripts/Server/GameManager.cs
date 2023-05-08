using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Random = System.Random;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Reflection;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region Public Fields
    
    //[Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;
    public GameObject Treasure;
    public GameObject SpaceShipPrefeb;

    public static GameManager Instance;

    private GameObject Player;
    private GameObject SpaceShip;
    private GameObject LeftPlayer;

    #endregion

    void Awake() {
        Manager.Init(this);
    }

    void Start()
    {
        //In case we started this demo with the wrong scene being active, simply load the menu scene
        if (PhotonNetwork.CurrentRoom == null)
        {
            Debug.Log("You are not in the room, returning back to Lobby");
            SceneManager.LoadScene(0);
            return;
        }

        Random r = new Random();
        int init_x = r.Next(100, 400);
        int init_z = r.Next(100, 400);
        //We're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
        //Player = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(init_x, 80f, init_z), Quaternion.identity, 0);
        Player = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(100f, 80f, 400f), Quaternion.identity, 0);
        SpaceShip = Instantiate(SpaceShipPrefeb, new Vector3(init_x, 80f, init_z), Quaternion.identity);
        Player.name = PhotonNetwork.NickName;
        //PlayerUI.
        Instance = this;

        for (int i = 0; i < 6; i++)
        {
            Instantiate(this.Treasure, new Vector3(r.Next(10, 490), 100f, r.Next(10, 490)), Quaternion.identity);
        }
    }

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
            Debug.LogFormat("MasterClient: {0} someone joined", PhotonNetwork.IsMasterClient);
            /*LeftPlayer = GameObject.Find("Player(Clone)");
            Debug.Log("find the player cloned");
            Debug.Log(LeftPlayer);
            LeftPlayer.name = other.NickName;
        */}

    }
    
    //Called when a remote player left the room or became inactive. Check otherPlayer.IsInactive.
    public override void OnPlayerLeftRoom(Player other)
    {
        // seen when other disconnects
        if (PhotonNetwork.IsMasterClient)
        {
            LeftPlayer = GameObject.Find("Player(Clone)");
            PhotonNetwork.Destroy(LeftPlayer);
            Debug.LogFormat("delete player"); 
        }
        
        Debug.LogFormat("OnPlayerLeftRoom() {0} left the room", other.NickName); 

        if (PhotonNetwork.IsMasterClient)
        {
            // called before OnPlayerLeftRoom
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0} someone is leaving", PhotonNetwork.IsMasterClient);
        }
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
