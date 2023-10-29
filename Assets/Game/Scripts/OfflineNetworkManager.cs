using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OfflineNetworkManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Awake()
    {
        PhotonNetwork.OfflineMode = true;
        PhotonNetwork.JoinRoom("alone");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log($"Debug Mode: Connected via OfflineMode");
    }
}
