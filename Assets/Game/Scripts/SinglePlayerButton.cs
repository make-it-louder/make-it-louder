using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerButton : MonoBehaviour
{
    public LobbyManager lobbyManager;
    public void OnClick()
    {
        lobbyManager.JoinSingleRoom();
    }
}
