using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using Photon.Pun;

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

    public Button loginButton; // 에디터에서 할당
    public Button signupButton; // 에디터에서 할당

    public GameObject loadingSpinner;
    // Start is called before the first frame update
    void Start()
    {
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (email.isFocused)
            {
                password.ActivateInputField();
            }
            else if (confirmpassword != null && password.isFocused)
            {
                confirmpassword.ActivateInputField();
            }
            else if (username != null && confirmpassword.isFocused )
            {
                username.ActivateInputField();
            }
        } else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (loginForm.activeSelf == true)
            {
                loginButton.onClick.Invoke();
            }
            else if (signupForm.activeSelf == true)
            {
                signupButton.onClick.Invoke();
            }
        }
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
    public async void SignUp()
    {

        if (string.IsNullOrEmpty(email.text))
        {
            OpenPopup("실패", "메일주소를 입력해주세요.");
            return;
        }
        if (username.text.Length > 10)
        {
            OpenPopup("실패", "닉네임을 10글자 이내로 입력해주세요.");
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
        bool isDuplicated = await FirebaseManager.Instance.IsUsernameTaken(username.text);
        if (isDuplicated)
        {
            OpenPopup("실패", "이미 존재하는 닉네입입니다.");
            return;
        }
        loadingSpinner.SetActive(true);
        FirebaseManager.Instance.SignUp(email.text, password.text, username.text, SignUpCallback);
    }

    private void SignUpCallback(bool flag)
    {
        loadingSpinner.SetActive(false);

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
        loadingSpinner.SetActive(true);
        FirebaseManager.Instance.SignIn(email.text, password.text, SignInCallback);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    private void SignInCallback(bool flag)
    {
        loadingSpinner.SetActive(false);
        if (flag)
        {
            StartCoroutine(JoinLobbyAfterPhotonConnection());
        }
        else
        {
            OpenPopup("실패", "메일주소/비밀번호를 확인해주세요!");
        }
    }
    private IEnumerator JoinLobbyAfterPhotonConnection()
    {
        yield return new WaitUntil(() => PhotonNetwork.IsConnectedAndReady);
        SceneManager.LoadScene("LobbyTest");
    }
    //
    public void SignOut()
    {
        FirebaseManager.Instance.SignOut();
    }
}
