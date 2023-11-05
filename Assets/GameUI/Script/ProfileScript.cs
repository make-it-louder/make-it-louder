using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // Scene 관리를 위해 추가

public class ProfileScript : MonoBehaviour
{
    FirebaseManager.Profile profile;
    Dictionary<string, FirebaseManager.Record> record;

    public TMP_Text name;
    public TMP_Text cnt;
    // public Button updateButton; // 이제 Update 버튼은 사용하지 않으므로 주석 처리

    // OnEnable is called when the object becomes active
    public void OnEnable()
    {
        // 씬이 로드될 때 호출될 메소드를 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // OnDisable is called when the object becomes inactive
    public void OnDisable()
    {
        // 씬이 로드될 때 호출될 메소드의 등록을 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 씬이 로드될 때 호출될 메소드
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "LobbyTest") // "LobbyTest" 씬에 들어왔을 때만 업데이트
        {
            UpdateUserData();
        }
    }

    public void UpdateUserData()
    {
        GetUserData();
        UpdateUI();
    }

    public void GetUserData()
    {
        profile = RecordManager.Instance.UserProfile;
        record = RecordManager.Instance.UserRecords;
    }

    public void UpdateUI()
    {
        name.text = profile.username;
        cnt.text = record["map1"].count_jump.ToString() + "회";
    }
}
