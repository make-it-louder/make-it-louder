using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SignSystem : MonoBehaviour
{
    public TMP_InputField email;
    public TMP_InputField username;
    public TMP_InputField password;
    public TMP_InputField passwordConfirm;
    public GameObject popupWindow;
    public TMP_Text popupTitle;
    public TMP_Text popupContent;

    // Start is called before the first frame update
    void Start()
    {
        FirebaseManager.Instance.InitializeFirebase();
    }

    public void SignUp()
    {
        if (password.text == passwordConfirm.text)
        {
            FirebaseManager.Instance.SignUp(email.text, username.text, password.text);
        } else
        {
            popupWindow.SetActive(true);
            popupTitle.text = "실패";
            popupContent.text = "비밀번호가 일치하지 않습니다.";
        }

    }

    public void SignIn()
    {
        FirebaseManager.Instance.SignIn(email.text, password.text);
    }

    public void SignOut()
    {
        FirebaseManager.Instance.SignOut();
    }
}
