using UnityEngine;
using TMPro; // TextMeshPro 네임스페이스 추가
using UnityEngine.UI; // UI 컴포넌트를 사용하기 위해 필요
using UltimateClean; // SceneTransition 클래스가 있는 네임스페이스
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class RoomCreation : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomNameInputField; // 방 이름 TMP_InputField
    public Button confirmButton; // 확인 버튼
    public TMP_Text warningMessage; // 경고 메시지를 표시할 TMP_Text
    private RoomSettings roomSettings;
    public LobbyManager lobbyManager;
    void Start()
    {
        // 초기 설정: 경고 메시지를 숨깁니다.
        warningMessage.gameObject.SetActive(false);

        // 확인 버튼에 이벤트 리스너 추가
        confirmButton.onClick.AddListener(OnConfirmButtonClicked);

        // 비밀번호 창을 가져옵니다
        roomSettings = GetComponent<RoomSettings>();
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
        string passwordText = roomSettings.passwordInputField.text;
        bool passwordEnabled = roomSettings.passwordToggle.isOn;

        string password = null;
        if (string.IsNullOrEmpty(passwordText) || !passwordEnabled)
        {
            password = null;
        }
        else
        {
            password = passwordText;
        }
        lobbyManager.CreateRoom(roomName, password);
    }
}
