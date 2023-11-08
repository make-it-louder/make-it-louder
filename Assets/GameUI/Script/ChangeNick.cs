using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeNick : MonoBehaviourPunCallbacks
{
    public TMP_InputField cn;
    public TMP_Text nick;
    public GameObject inputFieldObject;


    // Start is called before the first frame update
    void Start()
    {
        HideInputField(); // 시작할 때 입력 필드를 숨깁니다.
    }


    public void HideInputField()
    {
        inputFieldObject.SetActive(false);

    }

    public void ShowInputField()
    {
        inputFieldObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void UpdateNick()
    {
        await RecordManager.Instance.UpdateUsername(cn.text);
        nick.text = cn.text;
        PhotonNetwork.NickName = nick.text;

    }
}
