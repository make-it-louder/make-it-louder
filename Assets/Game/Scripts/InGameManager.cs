using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UltimateClean;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class InGameManager : MonoBehaviour
{
    private GameObject spawnedPlayer;
    private GameObject heightManager;
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
    [SerializeField]
    private CharacterListManager characterListManager;
    [SerializeField]
    private VolumeBarManager volumeBarManager;
    [SerializeField]
    private AudioMixerGroup bgVolume;
    [SerializeField]
    private AudioMixerGroup effect;
    [SerializeField]
    private AudioMixerGroup other;

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
        

        FirebaseManager.Profile profile = RecordManager.Instance.UserProfile;
        Debug.Log($"Player character: ({(CharacterPrefabNames)profile.e_avatar})");
        spawnedPlayer.GetComponentInChildren<PlayerPrefabManager>().CharacterPrefabName = (CharacterPrefabNames)profile.e_avatar;
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
        heightManager = PhotonNetwork.Instantiate(
                    $"player/item",
                    Vector3.zero, Quaternion.identity
                );
        heightManager.GetComponent<CharacterHeightItemManager>().SetTarget(spawnedPlayer);
        playerMoveBehavior.jumpCountText = jumpCountText;
        playerMoveBehavior.playTimeText = playTimeText;
        playerMoveBehavior.clearPopup = clearPopup;

        characterListManager.Mine = spawnedPlayer;
        controller.player = spawnedPlayer;
        acvmtController.player = spawnedPlayer;

        volumeBarManager.micInputManager = spawnedPlayer.GetComponentInChildren<MicInputManager>();


        var defaultBgVolume = PlayerPrefs.GetFloat("Background", 0.5f);
        var defaultEffectVolume = PlayerPrefs.GetFloat("Effect", 0.5f);
        var defaultOtherMicVolume = PlayerPrefs.GetFloat("OtherPlayer", 0.5f);


        bgVolume.audioMixer.SetFloat("BgVolume", calcLogDB(defaultBgVolume));
        effect.audioMixer.SetFloat("FXVolume", calcLogDB(defaultEffectVolume));
        other.audioMixer.SetFloat("OtherMicVolume", calcLogDB(defaultOtherMicVolume));
        yield break;
    }
    private float calcLogDB(float value)
    {
        return Mathf.Log10(value) * 20;
    }

}
