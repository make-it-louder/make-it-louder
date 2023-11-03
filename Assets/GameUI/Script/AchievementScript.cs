using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementScript : MonoBehaviour
{
    FirebaseManager.Profile profile;
    Dictionary<string, FirebaseManager.Record> record;

    public TMP_Text name;
    public TMP_Text cnt;

    // Start is called before the first frame update
    private void Start()
    {
        GetUserData();

        // 사용자 이름 업데이트
        name.text = profile.username;

        // 달성도 텍스트 형식에 맞게 업데이트
        cnt.text = string.Format("현재달성도 [{0}/100]", record["map1"].count_jump);
    }

    private void GetUserData()
    {
        // 여기서는 RecordManager 인스턴스로부터 사용자 데이터를 가져오는 것을 가정합니다.
        profile = RecordManager.Instance.UserProfile;
        record = RecordManager.Instance.UserRecords;
    }
}
