using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
    public GameObject[] unlockableButtons; // 클릭 가능한 버튼 배열
    public GameObject[] lockedButtons; // 잠금 버튼 배열

    

    // Start is called before the first frame update
    void Start()
    {
        LoadCharacterStatusAsync();
    }

    public void LoadCharacterStatusAsync()
    {
        // FirebaseManager의 싱글톤 인스턴스에서 프로필 데이터를 비동기적으로 가져옵니다.
        FirebaseManager.Profile profile = RecordManager.Instance.UserProfile;

        if (profile != null && profile.avatars != null)
        {
            // 각 캐릭터의 상태에 따라 버튼 활성화/비활성화
            for (int i = 0; i < profile.avatars.Count; i++)
            {
                bool isUnlocked = profile.avatars[i];
                unlockableButtons[i].SetActive(isUnlocked); // 잠금 해제 상태면 활성화
                lockedButtons[i].SetActive(!isUnlocked); // 잠금 상태면 활성화
            }
        }
        else
        {
            Debug.LogError("프로필 데이터를 불러오는 데 실패했습니다.");
        }
    }
}
