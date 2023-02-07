using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

using System.Collections;
using TMPro;

// Player name input field. Let the user input his name, will appear above the player in the game.
public class PlayerNameInputField : MonoBehaviour
{
    #region Private Constants

    // Store the PlayerPref Key to avoid typos
    const string PlayerNamePrefKey = "PlayerName";

    #endregion

    #region MonoBehaviour CallBacks
    
    void Start () {

        string defaultName = string.Empty;
        TMP_InputField _inputField = this.GetComponent<TMP_InputField>();
        if (_inputField!=null)
        {
            if (PlayerPrefs.HasKey(PlayerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(PlayerNamePrefKey);
                _inputField.text = defaultName;
            }
        }

        PhotonNetwork.NickName =  defaultName;
    }

    #endregion

    #region Public Methods

    // Sets the name of the player, and save it in the PlayerPrefs for future sessions.
    // param name="value"<The name of the Player>
    public void SetPlayerName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }
        PhotonNetwork.NickName = value;

        PlayerPrefs.SetString(PlayerNamePrefKey,value);
    }

    #endregion
}