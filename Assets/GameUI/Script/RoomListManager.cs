using Unity;
using UnityEngine;
using Photon.Realtime;

public class RoomListManager : MonoBehaviour
{
    public RoomListManager(GameObject container)
    {
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

    public void Add(RoomInfo roomInfo)
    {
        RoomBuilder newRoom = new RoomBuilder(roomInfo);
        newRoom.Build(container.transform);
    }
    public void Remove(RoomInfo roomInfo)
    {
        GameObject room = container.transform.Find(roomInfo.Name)?.gameObject;
        if (room != null)
        {
            Destroy(room);
        }
    }
}