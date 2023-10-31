using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerPrefabManager : MonoBehaviourPun
{
    
    private Transform CuteBirdPrefabList;
    private Transform FaceList;


    private GameObject characterPrefab;
    [SerializeField]
    private CharacterPrefabNames characterPrefabNameInspector;
    private CharacterPrefabNames characterPrefabName;
    public CharacterPrefabNames CharacterPrefabName
    {
        get
        {
            return characterPrefabName;
        }
        set
        {
            if (characterPrefabName == value)
            {
                return;
            }
            characterPrefabName = value;
            OnCharacterPrefabChanged(value);
        }
    }

    [PunRPC]
    void SyncChangeCharacterPrefab(int ViewID, int characterPrefabID)
    {
        CharacterPrefabNames characterPrefabName = (CharacterPrefabNames)characterPrefabID;
        GameObject go = PhotonView.Find(ViewID).gameObject;
        if (characterPrefab != null)
        {
            Destroy(characterPrefab);
        }
        characterPrefab = Instantiate(CuteBirdPrefabList.Find(characterPrefabName.ToString()).gameObject, transform);
        SyncChangeCharacterFace(ViewID, (int)characterFacePrefabName);
    }
    private void OnCharacterPrefabChanged(CharacterPrefabNames value)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("SyncChangeCharacterPrefab", RpcTarget.All, photonView.ViewID, (int)value);
        }
    }

    private Material characterFaceMaterial;
    [SerializeField]
    private CharacterFacePrefabNames characterFacePrefabNameInspector;
    private CharacterFacePrefabNames characterFacePrefabName;
    public CharacterFacePrefabNames CharacterFacePrefabName
    {
        get
        {
            return characterFacePrefabName;
        }
        set
        {
            if (characterFacePrefabName == value)
            {
                return;
            }
            characterFacePrefabName = value;
            OnCharacterFacePrefabChanged(value);
        }
    }

    [PunRPC]
    void SyncChangeCharacterFace(int ViewID, int characterFaceID)
    {
        CharacterFacePrefabNames characterFaceName = (CharacterFacePrefabNames)characterFaceID;
        GameObject go = PhotonView.Find(ViewID).transform.GetChild(0).Find("CuteBird_Face").gameObject;
        characterFaceMaterial = FaceList.Find(characterFaceName.ToString()).GetComponent<SkinnedMeshRenderer>().sharedMaterial;
        go.GetComponent<SkinnedMeshRenderer>().sharedMaterial = characterFaceMaterial;
    }

    private void OnCharacterFacePrefabChanged(CharacterFacePrefabNames value)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("SyncChangeCharacterFace", RpcTarget.All, photonView.ViewID, (int)value);
        }
    }

    void Awake()
    {
        CuteBirdPrefabList = Resources.Load<GameObject>("PlayerSettings/CuteBirdPrefab").transform;
        FaceList = Resources.Load<GameObject>("PlayerSettings/Face").transform;
        CharacterFacePrefabName = characterFacePrefabName;
        CharacterPrefabName = characterPrefabName;
        characterPrefab = transform.GetChild(0).gameObject;
    }

    void OnValidate()
    {
        CharacterFacePrefabName = characterFacePrefabNameInspector;
        CharacterPrefabName = characterPrefabNameInspector;
    }
}