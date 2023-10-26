using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;

public class PunEventSender
{
    public static void SendToAll(byte code, IPunSerializable serializable)
    {
        Send(code, serializable, ReceiverGroup.All, SendOptions.SendReliable);
    }
    public static void SendToMasterClient(byte code, IPunSerializable serializable)
    {
        Send(code, serializable, ReceiverGroup.MasterClient, SendOptions.SendReliable);
    }
    public static void SendToOthers(byte code, IPunSerializable serializable)
    {
        Send(code, serializable, ReceiverGroup.Others, SendOptions.SendReliable);
    }
    public static void Send(byte code, IPunSerializable serializable, ReceiverGroup receiver, SendOptions sendOptions) {
        Debug.Log($"Sent:Code={code}, Serialized={serializable.PunSerialize()}, Receiver={receiver}");
        PhotonNetwork.RaiseEvent(
            code,
            serializable.PunSerialize(),
            new RaiseEventOptions { Receivers = receiver },
            sendOptions
        );
    }
    public static void Send(byte code, IPunSerializable serializable, int[] receiver, SendOptions sendOptions)
    {
        Debug.Log($"Sent:Code={code}, Serialized={serializable.PunSerialize()}, Receiver={receiver}");
        PhotonNetwork.RaiseEvent(
            code,
            serializable.PunSerialize(),
            new RaiseEventOptions { TargetActors = receiver },
            sendOptions
        );
    }
}