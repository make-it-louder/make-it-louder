using Unity;
using UnityEngine;
using Photon.Realtime;

public class RoomListManager
{
    public RoomListManager(LobbyManager lobbyManager, GameObject container)
    {
        this.lobbyManager = lobbyManager;
        this.container = container;
    }
    public GameObject Container
    {
        get
        {
            return container;
        }
        private set
        {
            container = value;
        }
    }
    private GameObject container;
    private LobbyManager lobbyManager;

    public void Add(RoomInfo roomInfo)
    {
        RoomBuilder newRoom = new RoomBuilder(lobbyManager, roomInfo);
        newRoom.Build(container.transform);
    }
    public void Remove(RoomInfo roomInfo)
    {
        GameObject room = container.transform.Find(roomInfo.Name)?.gameObject;
        if (room != null)
        {
            GameObject.Destroy(room);
        }
    }
    public bool Contains(RoomInfo roomInfo)
    {
        return container.transform.Find(roomInfo.Name) != null;
    }
}