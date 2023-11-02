using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class SignSystem : MonoBehaviour
{
    public TMP_InputField email;
    public TMP_InputField password;
    public TMP_InputField confirmpassword;
    public TMP_InputField username;

    // interface
    public GameObject signupForm;
    public GameObject loginForm;
    public GameObject popupWindow;
    public TMP_Text popupTitle;
    public TMP_Text popupContent;

    // Start is called before the first frame update
    void Start()
    {
        FirebaseManager.Instance.InitializeFirebase();
    }

    //
    private void OpenPopup(string title, string content)
    {
        popupTitle.text = title;
        popupContent.text = content;
        popupWindow.SetActive(true);
    }

    public void ClosePopup()
    {
        popupWindow.SetActive(false);
    }

    //
    public void SignUp()
    {
        if (string.IsNullOrEmpty(email.text))
        {
            OpenPopup("실패", "메일주소를 입력해주세요.");
            return;
        }

        string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        if (!Regex.IsMatch(email.text, emailPattern))
        {
            OpenPopup("실패", "메일형식을 입력해주세요.");
            return;
        }

        if (string.IsNullOrEmpty(password.text))
        {
            OpenPopup("실패", "비밀번호를 입력해주세요.");
            return;
        }

        if (password.text.Length < 6)
        {
            OpenPopup("실패", "비밀번호를 6자리 이상 입력해주세요.");
            return;
        }

        if (password.text != confirmpassword.text)
        {
            OpenPopup("실패", "비밀번호를 확인해주세요.");
            return;
        }

        if (string.IsNullOrEmpty(username.text))
        {
            OpenPopup("실패", "유저네임을 입력해주세요.");
            return;
        }

        FirebaseManager.Instance.SignUp(email.text, password.text, username.text, SignUpCallback);
    }

    private void SignUpCallback(bool flag)
    {
        if (flag)
        {
            OpenPopup("가입 성공", "회원가입에 성공했습니다.");

            loginForm.SetActive(true);
            signupForm.SetActive(false);
        }
        else
        {
            OpenPopup("가입 실패", "회원가입에 실패했습니다.");
        }
    }

    //
    public void SignIn()
    {
        FirebaseManager.Instance.SignIn(email.text, password.text, SignInCallback);
    }

    private void SignInCallback(bool flag)
    {
        if (flag)
        {
            SceneManager.LoadScene("LobbyTest");
        }
        else
        {
            OpenPopup("실패", "메일주소/비밀번호를 확인해주세요!");
        }
    }

    //
    public void SignOut()
    {
        FirebaseManager.Instance.SignOut();
    }
}
