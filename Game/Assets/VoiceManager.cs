using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice;
using Photon.Voice.PUN;
using Photon.Pun;

public class VoiceManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public override void OnJoinedRoom()
    {
        PunVoiceClient.Instance.ConnectAndJoinRoom();
    }
    public override void OnLeftRoom()
    {
        PunVoiceClient.Instance.Disconnect();
    }
}
