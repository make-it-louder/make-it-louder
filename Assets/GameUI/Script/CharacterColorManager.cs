using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharacterColorManager : MonoBehaviour
{
    public List<Button> characterButtons; // 캐릭터 선택 버튼들
    public Color selectedColor; // 선택된 캐릭터의 색상
    public Color defaultColor; // 선택되지 않은 캐릭터의 기본 색상

    private void Start()
    {
        InitializeButtons();
        SetInitialSelectedCharacterColor();
    }

    private void InitializeButtons()
    {
        foreach (var button in characterButtons)
        {
            button.onClick.AddListener(() => OnCharacterSelected(button));
        }
    }

    private void SetInitialSelectedCharacterColor()
    {
        FirebaseManager.Profile profile = RecordManager.Instance.UserProfile;
        if (profile != null && profile.e_avatar < characterButtons.Count)
        {
            int initChar = profile.e_avatar;
            // 초기 선택된 캐릭터의 버튼 색상을 설정
            var initialButton = characterButtons[initChar];
            initialButton.image.color = selectedColor;
            Debug.Log($"Initial button color set to: {initialButton.image.color}");
        }
        else
        {
            Debug.LogError("Profile is null or selected avatar index is out of range.");
        }
    }


    private void OnCharacterSelected(Button selectedButton)
    {
        ResetButtonColors(); // 모든 버튼의 색상을 초기화합니다.
        selectedButton.image.color = selectedColor;
    }

    private void ResetButtonColors()
    {
        foreach (var button in characterButtons)
        {
            button.image.color = defaultColor;
        }
    }
}
