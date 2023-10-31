using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    public TMP_InputField createName;
    public GameObject room;
    public string curNo { get; set; }
    public string curTitle { get; set; }
    private GameObject prevRoom;
    private NetworkManager networkManager;
    // Start is called before the first frame update
    void Start()
    {
        networkManager = GameObject.FindObjectOfType<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClickCreate()
    {

    }
    public void onClickJoin()
    {

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
