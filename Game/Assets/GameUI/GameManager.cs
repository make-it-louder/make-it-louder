using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    private GameObject loginForm;
    private GameObject signupForm;
    // Start is called before the first frame update
    void Start()
    {
        loginForm = GameObject.Find("LoginForm");
        signupForm = GameObject.Find("SignUpForm");
        loginForm.SetActive(true);
        signupForm.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //ȸ������������ ����
    public void GoToSignup()
    {
        signupForm.SetActive(true);
        loginForm.SetActive(false);
    }


    //�α���
    public void Login()
    {
        SceneManager.LoadScene("Lobby");
    }
}
