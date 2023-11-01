using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class RecordManager : MonoBehaviour
{
    // singleton
    private static RecordManager instance;

    // firebase
    private FirebaseManager.Profile userProfile;
    private Dictionary<string, FirebaseManager.Record> userRecords;

    public static RecordManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        FirebaseManager.Instance.InitializeFirebase();
    }

    //
    public void GetProfile(FirebaseManager.Profile profile)
    {
        userProfile = profile;
    }
    public FirebaseManager.Profile UserProfile
    {
        get { return userProfile; }
    }

    //
    public void GetRecords(Dictionary<string, FirebaseManager.Record> records)
    {
        userRecords = records;
    }

    public Dictionary<string, FirebaseManager.Record> UserRecords
    {
        get { return userRecords; }
    }
}
