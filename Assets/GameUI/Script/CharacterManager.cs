using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public GameObject[] characterPrefabs; // 캐릭터 프리팹 리스트
    private int currentCharacterIndex = -1; // 현재 활성화된 캐릭터 인덱스
  


    void Start()
    {
        FirebaseManager.Profile profile = RecordManager.Instance.UserProfile;

        int initChar = profile.e_avatar;
        // 초기 캐릭터 설정 
        Debug.Log(initChar);
        ChangeCharacter(initChar);
    }

    // 캐릭터를 변경하는 메서드
    public void ChangeCharacter(int characterIndex)
    {
        // 다른 캐릭터가 이미 활성화되어 있다면 비활성화
        if (currentCharacterIndex != -1)
        {
            characterPrefabs[currentCharacterIndex].SetActive(false);
        }

        // 새 캐릭터를 활성화
        characterPrefabs[characterIndex].SetActive(true);

        // 현재 캐릭터 인덱스 업데이트
        currentCharacterIndex = characterIndex;

        // Firebase에 아바타 인덱스 업데이트
        UpdateAvatarIndexInFirebase(characterIndex);
    }

    // Firebase에 아바타 인덱스를 업데이트하는 비동기 메서드
    private async void UpdateAvatarIndexInFirebase(int avatarIndex)
    {
  
            Debug.Log(avatarIndex);
            await RecordManager.Instance.UpdateEquipmentAvatar(avatarIndex);
    }
}
