using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Temp : MonoBehaviour
{
    FirebaseManager.Profile profile;
    Dictionary<string, FirebaseManager.Record> record;

    public TMP_Text name;
    public TMP_Text cnt;

    private void Start()
    {
        GetUserData();

        name.text = profile.username;
        cnt.text = record["map1"].count_jump.ToString();

        Debug.Log(cnt.text);
        Debug.Log(record["map1"].count_jump);
    }


    private void GetUserData ()
    {
        profile = RecordManager.Instance.UserProfile;
        record = RecordManager.Instance.UserRecords;
    }
}
