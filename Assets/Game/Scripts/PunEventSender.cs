using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
public class PunEventSender
{
    public static void SendToAll(byte code, IPunSerializable serializable)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(code, serializable.PunSerialize(), raiseEventOptions, SendOptions.SendReliable);
    }
    public static void SendToMasterClient(byte code, IPunSerializable serializable)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
        PhotonNetwork.RaiseEvent(code, serializable.PunSerialize(), raiseEventOptions, SendOptions.SendReliable);
    }
}
