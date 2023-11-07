using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    public Button yourButton; // 할당할 버튼
    public CharacterManager characterManager; // 캐릭터 매니저 참조
    public int characterIndex; // 변경할 캐릭터의 인덱스

    void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        // 버튼 클릭 시 실행할 함수
        characterManager.ChangeCharacter(characterIndex);
    }
}