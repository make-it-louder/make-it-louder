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
    public GameObject KeyInfo;
    private bool isTrue;
    // Start is called before the first frame update
    void Start()
    {
        loginForm.SetActive(true);
        signupForm.SetActive(false);
        isTrue = false;

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
    public void OnOffKeyInfo()
    {
        if (isTrue)
        {
            Debug.Log("열려");
            KeyInfo.SetActive(false);
            isTrue = false;
        }
        else
        {
            Debug.Log("닫혀");
            KeyInfo.SetActive(true);
            isTrue = true;
        }

    }
    //로그인
}
