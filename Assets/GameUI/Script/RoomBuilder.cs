using ExitGames.Client.Photon;
using Photon.Realtime;
using TMPro;
using UnityEngine;

class RoomBuilder: MonoBehaviour
{
    private string roomName;
    private byte[] hashedPw;
    private int curPlayer;
    private int maxPlayer;

    public RoomBuilder(RoomInfo roominfo)
    {
        roomName = roominfo.Name;
        if (roominfo.CustomProperties.ContainsKey("password"))
        {
            Hashtable hashtable = roominfo.CustomProperties;
            hashedPw = (byte[])hashtable["password"];
        }
        else
        {
            this.hashedPw = null;
        }
        curPlayer = roominfo.PlayerCount;
        maxPlayer = roominfo.MaxPlayers;
    }
    public GameObject Build(Transform parent)
    {
        GameObject result = Instantiate(Resources.Load<GameObject>("Room/Room"), parent);
        Debug.Log(result);
        Transform IconBackground = result.transform.Find("IconBackground");
        Debug.Log(IconBackground);
        Transform NameText = IconBackground.Find("NameText");
        Debug.Log(NameText);
        GameObject TMPHeadline = NameText.Find("HeadlineText")?.gameObject;
        Debug.Log(TMPHeadline);
        GameObject LockIcon = IconBackground.Find("Lock")?.gameObject;
        Debug.Log(LockIcon);
        GameObject TMPClientCnt = IconBackground.Find("ClientCnt")?.gameObject;
        Debug.Log(TMPClientCnt);
        if (TMPHeadline != null)
        {
            TMPHeadline.GetComponent<TMP_Text>().text = roomName;
        }
        if (LockIcon != null)
        {
            LockIcon.SetActive(hashedPw != null);
        }
        if (TMPClientCnt != null)
        {
            TMPClientCnt.GetComponent<TMP_Text>().text = $"{curPlayer} / {maxPlayer}";
        }
        result.name = roomName;
        Room room = result.AddComponent<Room>();
        room.RoomName = roomName;
        room.hashedPw = hashedPw;
        return result;
    }
}