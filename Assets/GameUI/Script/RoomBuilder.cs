using ExitGames.Client.Photon;
using Photon.Realtime;
using TMPro;
using UltimateClean;
using UnityEngine;
using Photon.Pun;

class RoomBuilder
{
    private string roomName;
    private byte[] hashedPw;
    private int curPlayer;
    private int maxPlayer;
    private LobbyManager lobbyManager;
    public RoomBuilder(LobbyManager lobbyManager, RoomInfo roominfo)
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
        this.lobbyManager = lobbyManager;
    }

    public GameObject Build(Transform parent)
    {
        GameObject result = GameObject.Instantiate(Resources.Load<GameObject>("Room/Room"), parent);
        Transform IconBackground = result.transform.Find("IconBackground");
        Transform NameText = IconBackground.Find("NameText");
        GameObject TMPHeadline = NameText.Find("HeadlineText")?.gameObject;
        GameObject LockIcon = IconBackground.Find("Lock")?.gameObject;
        GameObject TMPClientCnt = IconBackground.Find("ClientCnt")?.gameObject;
        CleanButton JoinButton = result.transform.Find("EnterButton")?.gameObject.GetComponent<CleanButton>();
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
        room.setHashedPw(hashedPw);
        if (hashedPw == null)
        {
            JoinButton.onClick.AddListener(() =>
            {
                lobbyManager.JoinRoom(room.RoomName);
            });
        }
        else
        {
            JoinButton.onClick.AddListener(() =>
            {
                Debug.Log("Password");
                PopupManager popupmanager = GameObject.FindObjectOfType<Canvas>().GetComponent<PopupManager>();
                LockRoom lockRoom = popupmanager.popups[1].gameObject.GetComponent<LockRoom>();
                lockRoom.Room = room;
                popupmanager.OpenPopup(1);
            });
        }
        return result;
    }
}