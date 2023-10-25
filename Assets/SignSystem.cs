using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SignSystem : MonoBehaviour
{
    public TMP_InputField email;
    public TMP_InputField username;
    public TMP_InputField password;

    // Start is called before the first frame update
    void Start()
    {
        FirebaseManager.Instance.InitializeFirebase();
    }

    public void SignUp()
    {
        string e = email.text;
        string u = username.text;
        string p = password.text;

        FirebaseManager.Instance.SignUp(e, u, p);
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