using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
/*    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;*/
    public GameObject loginForm;
    public GameObject signupForm;
    // Start is called before the first frame update
    void Start()
    {
        loginForm.SetActive(true);
        signupForm.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //회원가입폼으로 변경
    public void GoToSignup()
    {
        signupForm.SetActive(true);
        loginForm.SetActive(false);
    }
    public void GoToLogin()
    {
        signupForm.SetActive(false);
        loginForm.SetActive(true);
    }

    //로그인
    public void Login()
    {
        FindObjectOfType<FadeManager>().ChangeScene("Lobby");
    }
}
