using UnityEngine;
using TMPro; // TextMeshPro 네임스페이스 추가
using UnityEngine.UI; // UI 컴포넌트를 사용하기 위해 필요

public class RoomCreation : MonoBehaviour
{
    public TMP_InputField roomNameInputField; // 방 이름 TMP_InputField
    public Button confirmButton; // 확인 버튼
    public TMP_Text warningMessage; // 경고 메시지를 표시할 TMP_Text

    void Start()
    {
        // 초기 설정: 경고 메시지를 숨깁니다.
        warningMessage.gameObject.SetActive(false);

        // 확인 버튼에 이벤트 리스너 추가
        confirmButton.onClick.AddListener(OnConfirmButtonClicked);
    }

    // 확인 버튼이 클릭될 때 호출되는 메서드
    void OnConfirmButtonClicked()
    {
        // 방 이름이 비어있는지 확인
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            // 경고 메시지를 표시합니다.
            warningMessage.gameObject.SetActive(true);
        }
        else
        {
            // 경고 메시지를 숨깁니다.
            warningMessage.gameObject.SetActive(false);
            // 방 생성 로직 수행
            CreateRoom(roomNameInputField.text);
        }
    }

    // 방을 생성하는 메서드 (예시)
    void CreateRoom(string roomName)
    {
        // 방 생성 로직
        Debug.Log(roomName + " 방이 생성되었습니다.");
    }
}
