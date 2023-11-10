using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeNick : MonoBehaviourPunCallbacks
{
    public TMP_InputField cn;
    public TMP_Text nick;
    public GameObject inputFieldObject;
    public TMP_Text duplicationText;
    public GameObject Btn;

    // Start is called before the first frame update
    void Start()
    {
        HideInputField(); // 시작할 때 입력 필드를 숨깁니다.
        duplicationText.enabled = false;
        Btn.SetActive(true);
    }


    public void HideInputField()
    {
        inputFieldObject.SetActive(false);
        Btn.SetActive(true);
        duplicationText.enabled = false;
    }

    public void ShowInputField()
    {
        Btn.SetActive(false);
        inputFieldObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void UpdateNick()
    {   
        bool isDuplicated = await FirebaseManager.Instance.IsUsernameTaken(cn.text);
        if (cn.text == "" || cn.text.Contains(" ")) 
        {
            duplicationText.text = "공백은 허용되지 않습니다.";
            duplicationText.enabled = true;
            return;
        }
        else if (isDuplicated)
        {
            duplicationText.text = "중복된 닉네임 입니다.";
            duplicationText.enabled = true;
            return;;
        }
        else if (cn.text.Length > 10)
        {
            duplicationText.text = "닉네임은 10글자 제한입니다.";
            duplicationText.enabled = true;
            return; ;
        }
        else 
        {
            await RecordManager.Instance.UpdateUsername(cn.text);
            nick.text = cn.text;
            PhotonNetwork.NickName = nick.text;
            duplicationText.enabled = false;
            HideInputField();
        }

    }
}
