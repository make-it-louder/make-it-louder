using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Scene 관리를 위한 네임스페이스 추가

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 버튼 클릭 시 호출될 메서드
    public void GoToLobby()
    {
        SceneManager.LoadScene("Lobby"); // Lobby라는 이름의 씬으로 이동
    }
}
