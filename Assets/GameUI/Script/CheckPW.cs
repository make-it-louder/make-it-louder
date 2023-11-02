using UnityEngine;
using TMPro; // TextMeshPro 네임스페이스 추가
using UnityEngine.UI; // UI 컴포넌트를 사용하기 위해 필요

public class RoomSettings : MonoBehaviour
{
    public Toggle passwordToggle; // 체크박스 Toggle
    public TMP_InputField passwordInputField; // 비밀번호 TMP_InputField

    void Start()
    {
        // 초기 설정: TMP_InputField를 비활성화
        passwordInputField.interactable = false;

        // Toggle의 상태가 변경될 때 호출될 메서드 설정
        passwordToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    // Toggle 상태가 변경될 때 호출되는 메서드
    void OnToggleChanged(bool isOn)
    {
        // Toggle이 켜져있다면 TMP_InputField를 활성화, 아니면 비활성화
        passwordInputField.interactable = isOn;
    }
}
