using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ProfileScript : MonoBehaviour

{
    FirebaseManager.Profile profile;

    Dictionary<string, FirebaseManager.Record> record;

    public TMP_Text name;
    public TMP_Text cnt;

    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
