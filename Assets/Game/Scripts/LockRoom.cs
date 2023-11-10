using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LockRoom : MonoBehaviour
{
    [HideInInspector]
    public Room Room;
    public TMP_InputField inputField;
    public TMP_Text wrongPasswordText; // 비밀번호가 틀렸을 때 표시할 텍스트
    public LobbyManager lobbyManager;

    private void Start()
    {
        
        wrongPasswordText.gameObject.SetActive(false); // 초기에는 경고 텍스트를 숨깁니다.
    }

    public void OnClickButton()
    {
        string text = inputField.text;
        if (Room == null)
        {
            Debug.LogError("Room not set yet");
            return;
        }

        bool ok = Room.Check(text);
        if (ok)
        {
            // 비밀번호가 맞으면 경고 텍스트를 비활성화하고 방에 참여합니다.
            wrongPasswordText.gameObject.SetActive(false);
            lobbyManager.JoinRoom(Room.RoomName);
        }
        else
        {
            // 비밀번호가 틀리면 경고 텍스트를 활성화합니다.
            Debug.Log("Wrong password");
            wrongPasswordText.gameObject.SetActive(true);
        }
    }
}
