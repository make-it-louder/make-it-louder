using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Temp : MonoBehaviour
{
    // Profile 테이블 지정
    FirebaseManager.Profile profile;
    // Record 테이블 지정
    Dictionary<string, FirebaseManager.Record> record;

    public TMP_Text name;
    public TMP_Text cnt;
    public TMP_InputField cn;
    private void Start()
    {
        GetUserData();

        name.text = profile.username;
        cnt.text = record["map1"].count_jump.ToString();
    }


    private void GetUserData ()
    {
        profile = RecordManager.Instance.UserProfile;
        record = RecordManager.Instance.UserRecords;
    }

    public async void ChangeName ()
    {
        await RecordManager.Instance.UpdateUsername(cn.text);
        name.text = profile.username;
    }
}
