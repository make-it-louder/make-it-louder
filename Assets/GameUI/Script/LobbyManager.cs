using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UltimateClean;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public GameObject RoomListContainer;
    RoomListManager roomListManager;


    private void Start()
    {
        if (RoomListContainer == null)
        {
            Debug.LogError("RoomListContainer is null");
        }
        roomListManager = new RoomListManager(this, RoomListContainer);
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

        if (PhotonNetwork.IsConnected)
        {
            StartCoroutine(JoinLobby());
            return;
        }
        else
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private System.Collections.IEnumerator JoinLobby()
    {
        float startTime = Time.time;
        Debug.Log("Call JoinLobby");
        yield return new WaitUntil(() => PhotonNetwork.IsConnectedAndReady || Time.time - startTime > 10.0f);
        yield return new WaitUntil(() => PhotonNetwork.NetworkClientState == ClientState.ConnectedToMasterServer || Time.time - startTime > 10.0f);
        if (Time.time - startTime > 10.0f)
        {
            Debug.LogError("Failed to join lobby: Timeout");
            yield break;
        }
        if (PhotonNetwork.InLobby || PhotonNetwork.NetworkClientState == ClientState.JoiningLobby)
        {
            yield break;
        }
        else
        {
            PhotonNetwork.JoinLobby();
        }
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        StartCoroutine(JoinLobby());
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
        Debug.Log($"OnRoomListUpdate");
        foreach (RoomInfo roomInfo in roomList)
        {
            Debug.Log($"\t{roomInfo}");
        }
        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList)
            {
                roomListManager.Remove(roomInfo);
            }
            else
            {
                if (roomListManager.Contains(roomInfo))
                {
                    roomListManager.Remove(roomInfo);
                    roomListManager.Add(roomInfo);
                }
                else
                {
                    roomListManager.Add(roomInfo);
                }
            }
        }
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }
    public void JoinSingleRoom()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 1;
        options.IsOpen = false;
        options.IsVisible = false;
        PhotonNetwork.CreateRoom(null, options);
    }
    public void CreateRoom(string roomName, string password)
    {
        RoomOptions roomOptions = new RoomOptions();
        if (password != null)
        {
            roomOptions.CustomRoomProperties = new Hashtable() { { "password", Room.Encrypt(password) } };
            roomOptions.CustomRoomPropertiesForLobby = new string[] { "password" };
        }
        roomOptions.MaxPlayers = 20;
        Debug.Log("MaxPlayer had set to 20. Please change it when UI changes");
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"({returnCode})Failed to create room: {message}");
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Created room");
        //GetComponent<SceneTransition>().PerformTransition();
        PhotonNetwork.LoadLevel("MakeItLouder");
    }
    public override void OnJoinedRoom()
    {
        
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Cannot join room due to {message}");
    }
}
