using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNickname : MonoBehaviourPunCallbacks
{

    public void SetNickname(string newNickname)
    {
        photonView.RPC("RPC_SetNickname", RpcTarget.AllBuffered, newNickname);
    }

    [PunRPC]
    private void RPC_SetNickname(string newNickname)
    {
        GetComponentInChildren<TMP_Text>().text = newNickname;
        gameObject.name = newNickname;
    }
}

