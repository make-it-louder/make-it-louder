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
    private BlinkingObject blinkingObject;
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
        characterPrefabName = (CharacterPrefabNames)characterPrefabID;
        Material newMaterial = CuteBirdPrefabList.Find(characterPrefabName.ToString()).GetChild(0).GetComponent<SkinnedMeshRenderer>().sharedMaterial;
        characterPrefab.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().sharedMaterial = newMaterial;
        blinkingObject.ChangeMaterial(newMaterial);
        SyncChangeCharacterFace(ViewID, (int)characterFacePrefabName);
    }
    private void OnCharacterPrefabChanged(CharacterPrefabNames value)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("SyncChangeCharacterPrefab", RpcTarget.AllBuffered, photonView.ViewID, (int)value);
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
            photonView.RPC("SyncChangeCharacterFace", RpcTarget.AllBuffered, photonView.ViewID, (int)value);
        }
    }

    void Awake()
    {
        CuteBirdPrefabList = Resources.Load<GameObject>("PlayerSettings/CuteBirdPrefab").transform;
        FaceList = Resources.Load<GameObject>("PlayerSettings/Face").transform;
        CharacterFacePrefabName = characterFacePrefabName;
        CharacterPrefabName = characterPrefabName;
        characterPrefab = transform.GetChild(0).gameObject;
        blinkingObject = transform.GetComponentInParent<BlinkingObject>();
    }

    void OnValidate()
    {
        CharacterFacePrefabName = characterFacePrefabNameInspector;
        CharacterPrefabName = characterPrefabNameInspector;
    }
}
