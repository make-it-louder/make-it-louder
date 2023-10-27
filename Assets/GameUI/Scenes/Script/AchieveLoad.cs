using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Scene 관리를 위한 네임스페이스 추가

public class Achiev : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Achievement 씬으로 이동하는 메서드
    public void GoToAchievementScene()
    {
        SceneManager.LoadScene("Achievement"); // Achievement라는 이름의 씬으로 이동
    }
}
