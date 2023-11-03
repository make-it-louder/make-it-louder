using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    FirebaseManager.Profile profile;
    Dictionary<string, FirebaseManager.Record> records;
    public TMP_Text userName;
    public TMP_Text curAvatar;
    public List<string> avatars;
    public List<string> achievements;

    public TMP_Text Map1PT;
    public TMP_Text Map1CJ;
    public TMP_Text Map1CF;

    public TMP_Text Map2PT;
    public TMP_Text Map2CJ;
    public TMP_Text Map2CF;


    public TMP_Text Map3PT;
    public TMP_Text Map3CJ;
    public TMP_Text Map3CF;

    // Start is called before the first frame update
    void Start()
    {
        profile = RecordManager.Instance.UserProfile;
        records = RecordManager.Instance.UserRecords;

        userName.text = profile.username;
        curAvatar.text = profile.avatar;
        avatars = profile.avatars;
        achievements = profile.achievements;
        // records 저장
        Map1PT.text = records["Map1"].playtime.ToString();
        Map1CJ.text = records["Map1"].count_jump.ToString();
        Map1CF.text = records["Map1"].count_fall.ToString();

        Map2PT.text = records["Map2"].playtime.ToString();
        Map2CJ.text = records["Map2"].count_jump.ToString();
        Map2CF.text = records["Map2"].count_fall.ToString();

        Map3PT.text = records["Map3"].playtime.ToString();
        Map3CJ.text = records["Map3"].count_jump.ToString();
        Map3CF.text = records["Map3"].count_fall.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
