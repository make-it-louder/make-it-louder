using Photon.Pun;
using Photon.Realtime;
using System.Collections;
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
        Debug.Log("OnConnectedToMaster");
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 6 }, null); // ������ �濡 ����
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("IAmMasterClient");
            PhotonNetwork.LoadLevel(1); // ���Ӿ����� ��ȯ
        }
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a random room. Creating a new room...");
        PhotonNetwork.CreateRoom(null);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded");
        Debug.Log($"Scene name: {scene.name}, InRoom: {PhotonNetwork.InRoom}");
        // ���� ���� �ε�� ���
        if (scene.buildIndex == 1)
        {
            StartCoroutine(OnScene1Loaded());
        }
    }

    private void OnDestroy()
    {
        // ���� ������Ʈ�� �ı��� �� �̺�Ʈ���� �޼��带 ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    IEnumerator waitForLevelLoad()
    {
        if (PhotonNetwork.LevelLoadingProgress < 1)
        {
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator OnScene1Loaded() {
        yield return new WaitUntil(() => PhotonNetwork.InRoom);
        GameObject spawnedPlayer = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(Random.Range(-5f, 5f), 1, Random.Range(-5f, 5f)), Quaternion.identity);
        GridCamera2D camera = GameObject.Find("Main Camera").GetComponent<GridCamera2D>();
        camera.follows = spawnedPlayer;
        yield break;
    }
}
