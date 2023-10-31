using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    public TMP_InputField createName;
    public GameObject room;
    public GameObject content;
    public string curNo { get; set; }
    public string curTitle { get; set; }
    private GameObject prevRoom;
    private NetworkManager networkManager;
    // Start is called before the first frame update
    void Start()
    {
        networkManager = GameObject.FindObjectOfType<NetworkManager>();
        UpdateRoomList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DestroyAllChildren(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public void UpdateRoomList()
    {
        DestroyAllChildren(content);
        int i = 1;
        foreach (RoomInfo roomInfo in networkManager.myList)
        {
            // Instantiate a new object
            GameObject spawnedObject = Instantiate(room);
            spawnedObject.transform.Find("No").GetComponent<TMP_Text>().text = i++.ToString("D3");
            spawnedObject.transform.Find("Title").GetComponent<TMP_Text>().text = roomInfo.Name;

            // Set the parent of the spawned object to the specified parent object
            spawnedObject.transform.SetParent(content.transform);
        }
    }
    public void onClickCreate()
    {
        networkManager.CreateRoom(createName.text);
    }
    public void onClickJoin()
    {
        networkManager.JoinRoom(curTitle);
    }
    public void ChangeRoom(GameObject room)
    {
        if(prevRoom != null)
        {
            prevRoom.GetComponent<Image>().color = Color.white;
        }
        prevRoom = room;
        prevRoom.GetComponent<Image>().color = Color.green;
    }
}
