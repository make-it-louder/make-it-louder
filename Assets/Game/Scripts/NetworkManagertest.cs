using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
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
        PhotonNetwork.JoinOrCreateRoom("Roomrr", new RoomOptions { MaxPlayers = 6 }, null); // ������ �濡 ����
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("IAmMasterClient");
            PhotonNetwork.LoadLevel(3); // ���Ӿ����� ��ȯ
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
        if (scene.buildIndex == 3)
        {
            StartCoroutine(OnScene3Loaded());
        }
    }

    private void OnDestroy()
    {
        // ���� ������Ʈ�� �ı��� �� �̺�Ʈ���� �޼��带 ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    IEnumerator OnScene3Loaded() {
        yield return new WaitUntil(() => PhotonNetwork.InRoom);
        GameObject spawnedPlayer = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(Random.Range(-5f, 5f), 1, Random.Range(-5f, 5f)), Quaternion.identity);

        GridCamera2D camera = GameObject.Find("Main Camera").GetComponent<GridCamera2D>();
        camera.follows = spawnedPlayer;
        spawnedPlayer.GetComponent<PlayerNickname>().SetNickname(PhotonNetwork.LocalPlayer.NickName);

        SoundEventManager soundEventManager = GameObject.Find("SoundEventManager").GetComponent<SoundEventManager>();
        spawnedPlayer.GetComponentInChildren<MicInputManager>().SoundEventManager = soundEventManager;
        AudioMixer audioMixer = Resources.Load<AudioMixer>("Audio/AudioMixer");
        AudioMixerGroup[] groups = audioMixer.FindMatchingGroups("MyVoice");
        if (groups.Length >= 2)
        {
            Debug.LogError("AudioMixergroup MyVoice is ambiguous");
        }
        spawnedPlayer.GetComponentInChildren<AudioSource>().outputAudioMixerGroup = groups[0];
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
