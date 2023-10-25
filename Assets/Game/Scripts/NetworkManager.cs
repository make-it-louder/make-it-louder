using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected");
        if (PhotonNetwork.OfflineMode == false)
        {
            PhotonNetwork.OfflineMode = true;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.Log($"Disconnected from server due to {cause}");
        }
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        RoomOptions roomOptions = new RoomOptions();
        PhotonNetwork.JoinOrCreateRoom("_base_room",roomOptions, null);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(0.0f, 5.0f), 0), Quaternion.identity);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer} entered room");
    }
}
