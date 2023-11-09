using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UltimateClean;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnedPlayer;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private Vector3 spawnPoint;
    [SerializeField]
    private FixedUIController controller;
    [SerializeField]
    private AcheivementManager acvmtController;
    [SerializeField]
    private Popup clearPopup;
    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Not Connected");
            Debug.Log(PhotonNetwork.NetworkClientState.ToString());
            return;
        }
        else
        {
            StartCoroutine(OnNetwork());
        }
    }
    
    private IEnumerator OnNetwork()
    {
        yield return new WaitUntil(() => PhotonNetwork.InRoom);
        spawnedPlayer = PhotonNetwork.Instantiate(
              $"player/{playerPrefab.name}",
              spawnPoint + new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(-0.5f, 0.5f), -0.1f),
              Quaternion.identity
          );
        GridCamera2D camera = GameObject.Find("Main Camera").GetComponent<GridCamera2D>();
        camera.follows = spawnedPlayer;
        spawnedPlayer.GetComponent<PlayerNickname>().SetNickname(PhotonNetwork.LocalPlayer.NickName);

        SoundEventManager soundEventManager = GameObject.Find("SoundEventManager").GetComponent<SoundEventManager>();
        spawnedPlayer.GetComponentInChildren<MicInputManager>().SoundEventManager = soundEventManager;

        PlayerMove2D playerMoveBehavior = spawnedPlayer.GetComponent<PlayerMove2D>();

        TMP_Text jumpCountText = GameObject.Find("JumpCount")?.GetComponent<TMP_Text>();
        TMP_Text playTimeText = GameObject.Find("PlayTime")?.GetComponent<TMP_Text>();
        PlayerMove2D playerMove2D = spawnedPlayer.GetComponent<PlayerMove2D>();

        ChatManager chatManager = GameObject.Find("ChatManager")?.GetComponent<ChatManager>();
        if (chatManager != null)
        {
            chatManager.playerMove2D = playerMove2D;
        }

        playerMoveBehavior.jumpCountText = jumpCountText;
        playerMoveBehavior.playTimeText = playTimeText;
        playerMoveBehavior.clearPopup = clearPopup;
        controller.player = spawnedPlayer;
        acvmtController.player = spawnedPlayer;
        yield break;
    }
}
