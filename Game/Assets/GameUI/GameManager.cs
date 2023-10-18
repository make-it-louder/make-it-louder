using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GoToScene(string sceneName) => SceneManager.LoadScene(sceneName);


    //·Î±×ÀÎ
    public void Login()
    {


        SceneManager.LoadScene("Lobby");
    }
}
