using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
public class CharacterHeightItemManager : MonoBehaviourPun
{
    [SerializeField]
    private Image imageSlot;
    private CharacterHeightTracker characterHeightTracker;
    [SerializeField]
    private TextMeshProUGUI description;
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private PlayerPrefabManager playerPrefabManager;
    [Range(0, 1)]
    public float progress;
    private RectTransform rectTransform;
    Rect parentRect;
    void Awake()
    {
        characterHeightTracker = FindFirstObjectByType<CharacterHeightTracker>();
        transform.parent = characterHeightTracker.transform.Find("content");
        parentRect = transform.parent.GetComponent<RectTransform>().rect;
        rectTransform = GetComponent<RectTransform>();
        progress = 0.0f;
    }
    public void SetTarget(GameObject target)
    {
        this.target = target;
        this.playerPrefabManager = target.GetComponentInChildren<PlayerPrefabManager>();
        photonView.RPC("SyncSetTarget", RpcTarget.OthersBuffered, target.GetComponent<PhotonView>().ViewID);
    }
    [PunRPC]
    void SyncSetTarget(int ViewID, PhotonMessageInfo info)
    {
        GameObject target = PhotonView.Find(ViewID).gameObject;
        this.target = target;
        this.playerPrefabManager = target.GetComponentInChildren<PlayerPrefabManager>();
    }
    
    private float prevHeight = 0;
    private string prevNickname = "";
    private CharacterPrefabNames prevPrefabName = CharacterPrefabNames.C00;
    private float Height
    {
        get { return target.transform.position.y; }
    }
    private string Nickname
    {
        get { return target.name; }
    }
    private CharacterPrefabNames prefabName
    {
        get { return playerPrefabManager.CharacterPrefabName; }
    }
    void Update()
    {
        if (target == null) return;
        if (prevHeight != Height)
        {
            float height = Height;
            prevHeight = height;
            progress = (height - characterHeightTracker.minHeight) /
                        (characterHeightTracker.maxHeight - characterHeightTracker.minHeight);
            rectTransform.anchoredPosition =
              new Vector2(0, Mathf.Clamp(parentRect.height * progress, 0.0f, parentRect.height));
            description.text = $"{Nickname}: {Height: 0.0}m 등반중!";
        }
        if (prevNickname != Nickname)
        {
            prevNickname = Nickname;
            description.text = $"{Nickname}: {Height: 0.0}m 등반중!";
        }
        if (prevPrefabName != prefabName)
        {
            prevPrefabName = prefabName;
            imageSlot.sprite = characterHeightTracker.sprites[(int)prefabName];
        }
        
    }
}