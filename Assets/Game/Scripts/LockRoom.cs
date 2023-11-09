using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LockRoom : MonoBehaviour
{
    public Room Room;
    public TMP_InputField inputField;
    public LobbyManager lobbyManager;

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
            lobbyManager.JoinRoom(Room.RoomName);
        }
        else
        {
            Debug.Log("Wrong password");
        }
    }
}
