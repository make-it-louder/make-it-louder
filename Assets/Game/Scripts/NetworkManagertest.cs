using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField nicknameInputField;
    public GameObject playerPrefab;

    public List<RoomInfo> myList { get; set; }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        myList = new List<RoomInfo>();
    }
    public void OnConnectButtonClicked()
    {
        if (!string.IsNullOrEmpty(nicknameInputField.text))
        {
            Connect(nicknameInputField.text);
        }
    }

    public void Connect(string nickname)
    {
        PhotonNetwork.NickName = nickname;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        //PhotonNetwork.JoinLobby();
        PhotonNetwork.JoinOrCreateRoom("Roomrr", new RoomOptions { MaxPlayers = 6 }, null);
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName, null);
    }
    public void CreateRoom(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName,new RoomOptions { MaxPlayers = 6 }, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("IAmMasterClient");
            PhotonNetwork.LoadLevel("Game/Scenes/MakeItLouder"); // ���Ӿ����� ��ȯ
        }
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a random room. Creating a new room...");
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i])) myList.Add(roomList[i]);
                else myList[myList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (myList.IndexOf(roomList[i]) != -1) myList.RemoveAt(myList.IndexOf(roomList[i]));
        }

        RoomManager roomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
        if (roomManager != null) roomManager.UpdateRoomList();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded");
        Debug.Log($"Scene name: {scene.name}, InRoom: {PhotonNetwork.InRoom}");
        // ���� ���� �ε�� ���
        if (scene.name == "MakeItLouder")
        {
            StartCoroutine(OnMakeItLouderLoaded());
        }
    }

    private void OnDestroy()
    {
        // ���� ������Ʈ�� �ı��� �� �̺�Ʈ���� �޼��带 ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    IEnumerator OnMakeItLouderLoaded() {
        yield return new WaitUntil(() => PhotonNetwork.InRoom);
        GameObject spawnedPlayer = PhotonNetwork.Instantiate(
            "player/" + playerPrefab.name,
            new Vector3(Random.Range(15f, 19f), -3f, 0),
            Quaternion.identity
        );

        GridCamera2D camera = GameObject.Find("Main Camera").GetComponent<GridCamera2D>();
        camera.follows = spawnedPlayer;
        spawnedPlayer.GetComponent<PlayerNickname>().SetNickname(PhotonNetwork.LocalPlayer.NickName);

        SoundEventManager soundEventManager = GameObject.Find("SoundEventManager").GetComponent<SoundEventManager>();
        spawnedPlayer.GetComponentInChildren<MicInputManager>().SoundEventManager = soundEventManager;

        PlayerMove2D playerMoveBehavior = spawnedPlayer.GetComponent<PlayerMove2D>();

        TMP_Text jumpCountText = GameObject.Find("JumpCount").GetComponent<TMP_Text>();
        TMP_Text playTimeText = GameObject.Find("PlayTime").GetComponent<TMP_Text>();
        PlayerMove2D playerMove2D = spawnedPlayer.GetComponent<PlayerMove2D>();

        GameObject.Find("ChatManager").GetComponent<ChatManager>().playerMove2D = playerMove2D;

        playerMoveBehavior.jumpCountText = jumpCountText;
        playerMoveBehavior.playTimeText = playTimeText;

       yield break;
    }
}
