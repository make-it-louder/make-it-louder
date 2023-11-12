using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;

public class InputFieldFilter : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_Text text;

    private void Start()
    {
            text.enabled = false;
    }
    public void OnInputFieldChanged(string input)
    {

        if (!Regex.IsMatch(input, @"[^\uAC00-\uD7A3]+"))
        {
            // 텍스트 필드를 공백으로 설정
            inputField.text = "";
            text.enabled = true;
        } else
        {
            text.enabled = false;
        }
    }
}