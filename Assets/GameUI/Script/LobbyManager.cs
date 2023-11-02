using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public GameObject RoomListContainer;
    RoomListManager roomListManager;
    private void Awake()
    {
        FirebaseManager.Profile profile;
        profile = RecordManager.Instance?.UserProfile;
        if (profile == null)
        {
            PhotonNetwork.NickName = "Player";
        }
        else
        {
            PhotonNetwork.NickName = profile.username;
        }
        PhotonNetwork.AutomaticallySyncScene = true;
        if (RoomListContainer == null)
        {
            Debug.LogError("RoomListContainer is null");
        }
        roomListManager = new RoomListManager(RoomListContainer);
        PhotonNetwork.ConnectUsingSettings();
    }
    
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        PhotonNetwork.JoinLobby();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        if (cause == DisconnectCause.ApplicationQuit)
        {
            return;
        }
        Debug.LogError($"Disconnected from server due to {cause}");
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList)
            {
                roomListManager.Remove(roomInfo);
            }
            else
            {
                roomListManager.Add(roomInfo);
            }
        }
    }

    public override void OnJoinedRoom()
    {
        
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Cannot join room due to {message}");
    }
}
