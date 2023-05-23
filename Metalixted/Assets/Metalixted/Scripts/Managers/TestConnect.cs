using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestConnect : Photon.Pun.MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        try
        {
            PhotonNetwork.LeaveRoom(true);
        } catch { }
    }

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            Debug.Log("Connecting to server...", this);
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
            PhotonNetwork.NickName = MasterManager.GameSettings.NickName;
            PhotonNetwork.ConnectUsingSettings();
        }
        catch { }
        
    }

    // Update is called once per frame
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon.", this);
        print(PhotonNetwork.LocalPlayer.NickName);
        if(!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();


    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        //base.OnDisconnected(cause);
        Debug.Log("Failed to connect to Photon: " + cause.ToString(), this);
    }

    public override void OnJoinedLobby()
    {
        //base.OnJoinedLobby();
        print("Joined Lobby");
    }
}