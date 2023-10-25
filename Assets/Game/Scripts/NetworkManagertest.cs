using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NicknameManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField nicknameInputField;
    public GameObject playerPrefab;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public void OnConnectButtonClicked()
    {
        if (!string.IsNullOrEmpty(nicknameInputField.text))
        {
            PhotonNetwork.NickName = nicknameInputField.text;
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 6 }, null); // 랜덤한 방에 접속
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Game/Scenes/chatting"); // 게임씬으로 전환
        }
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a random room. Creating a new room...");
        PhotonNetwork.CreateRoom(null);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 게임 씬이 로드된 경우
        if (scene.buildIndex == 1)
        {
            GameObject spawnedPlayer = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(Random.Range(-5f, 5f), 1, Random.Range(-5f, 5f)), Quaternion.identity);
            Debug.Log(spawnedPlayer.name);
            spawnedPlayer.name = "me";
            GridCamera2D camera = GameObject.Find("Main Camera").GetComponent<GridCamera2D>();
            camera.follows = spawnedPlayer;
        }
    }

    private void OnDestroy()
    {
        // 게임 오브젝트가 파괴될 때 이벤트에서 메서드를 제거
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
