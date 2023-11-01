using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Room : MonoBehaviour
{
    // Start is called before the first frame update
    private RoomManager roomManager;
    private string roomNo;
    private string roomName;
    void Start()
    {
        roomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
        roomNo = transform.Find("No").GetComponent<TMP_Text>().text;
        roomName = transform.Find("Title").GetComponent<TMP_Text>().text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void onClick()
    {
        roomManager.curNo = roomNo;
        roomManager.curTitle = roomName;
        roomManager.ChangeRoom(gameObject);
    }

    public void setText(string no, string title)
    {
        roomNo = no;
        roomName = title;
    }
}
