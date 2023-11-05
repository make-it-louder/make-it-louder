using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TabAction : MonoBehaviour
{
    //로그인 폼
    public TMP_InputField l_emailField;
    public TMP_InputField l_passwordField;

    //회원가입 폼
    public TMP_InputField s_emailField;
    public TMP_InputField s_passwordField;
    public TMP_InputField s_passwordConfirmField;
    public TMP_InputField s_nicknameField;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (l_emailField.isFocused)
            {
                l_passwordField.ActivateInputField();
            } else if (l_passwordField.isFocused)
            {
                l_emailField.ActivateInputField();
            } else if (s_emailField.isFocused)
            {
                s_passwordField.ActivateInputField();
            } else if (s_passwordField.isFocused) 
            {
                s_passwordConfirmField.ActivateInputField();
            } else if (s_passwordConfirmField.isFocused)
            {
                s_nicknameField.ActivateInputField();
            }
        }
    }
}
