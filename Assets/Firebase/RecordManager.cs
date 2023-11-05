using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using Newtonsoft.Json;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Linq;
using static UnityEngine.Rendering.DebugUI;

public class RecordManager : MonoBehaviour
{
    // singleton
    private static RecordManager instance;

    // firebase
    private string currentId;
    private DatabaseReference databaseReference;

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
        databaseReference = FirebaseManager.Instance.GetDatabaseReference();
    }

    public async Task GetUser(string userId)
    {
        currentId = userId;

        try
        {
            userProfile = await GetProfile(currentId);

            if (userProfile != null)
            {
                Debug.Log("Profile loaded successfully.");
            }
            else
            {
                Debug.LogError("Failed to load user profile.");
            }

            userRecords = await GetRecords(currentId);

            if (userRecords != null)
            {
                Debug.Log("Records loaded successfully.");
            }
            else
            {
                Debug.LogError("Failed to load user records.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"An error occurred: {e.Message}");
        }
    }


    public async Task<FirebaseManager.Profile> GetProfile(string userId)
    {
        var task = databaseReference.Child("users").Child(userId).GetValueAsync();
        await task;

        if (task.IsCompleted)
        {
            DataSnapshot snapshot = task.Result;

            if (snapshot.Exists)
            {
                string json = snapshot.GetRawJsonValue();
                FirebaseManager.Profile user = JsonConvert.DeserializeObject<FirebaseManager.Profile>(json);
                return user;
            }
            else
            {
                Debug.LogError("the user data does not exist");
                return null;
            }
        }
        else
        {
            if (task.IsFaulted)
            {
                Debug.LogError("failed to retrieve user data from the database due to a faulted state");
            }
            else if (task.IsCanceled)
            {
                Debug.LogError("failed to retrieve user data from the database due to a canceled state");
            }
            return null;
        }
    }

    //
    public async Task<Dictionary<string, FirebaseManager.Record>> GetRecords(string userId)
    {
        var task = databaseReference.Child("records").Child(userId).GetValueAsync();
        await task;

        if (task.IsCompleted)
        {
            DataSnapshot snapshot = task.Result;

            if (snapshot.Exists)
            {
                string json = snapshot.GetRawJsonValue();
                Dictionary<string, FirebaseManager.Record> records = JsonConvert.DeserializeObject<Dictionary<string, FirebaseManager.Record>>(json);
                Debug.Log(records["map1"].count_jump);
                return records;
            }
            else
            {
                Debug.LogError("the record data does not exist");
                return null;
            }
        }
        else
        {
            if (task.IsFaulted)
            {
                Debug.LogError("failed to retrieve from the database due to a faulted state");
            }
            else if (task.IsCanceled)
            {
                Debug.LogError("failed to retrieve from the database due to a canceled state");
            }
            return null;
        }
    }

    //
    public FirebaseManager.Profile UserProfile
    {
        get { return userProfile; }
    }

    public Dictionary<string, FirebaseManager.Record> UserRecords
    {
        get { return userRecords; }
    }

    // 사용함수
    // 유저닉네임 업데이트 함수
    public async Task UpdateUsername(string newName)
    {
        if (userProfile != null)
        {
            userProfile.username = newName;

            try
            {
                await databaseReference.Child("users").Child(currentId).Child("username").SetValueAsync(newName);
                Debug.Log("username updated successfully");
            }
            catch (Exception e)
            {
                Debug.LogError("failed to update username: " + e.Message);
            }
        }
        else
        {
            Debug.LogError("userprofile is null");
        }
    }

    // 게임종료시 게임기록 업데이트 함수
    public async Task UpdateEndGameData(string mapName, float playTime, int countJump, int countFall)
    {
        if (userRecords == null)
        {
            Debug.LogError("userRecords is null");
            return;
        }
        
        playTime = (float)Math.Round(playTime, 2);

        userRecords[mapName].playtime += playTime;
        userRecords[mapName].count_jump += countJump;
        userRecords[mapName].count_fall += countFall;
        string updatedRecordJsonData = JsonConvert.SerializeObject(userRecords[mapName], Formatting.Indented);
        try
        {
            // 데이터베이스에 업데이트된 레코드 객체를 비동기적으로 설정합니다.
            await databaseReference.Child("records").Child(currentId).Child(mapName).SetRawJsonValueAsync(updatedRecordJsonData);
            Debug.Log("게임기록 업데이트 완료.");
        }
        catch (Exception e)
        {
            Debug.LogError("게임기록 업데이트 실패: " + e.Message);
        }
    }

    // 캐릭터 변경시 e_avatar 업데이트 함수
    public async Task UpdateEquipmentAvatar(int avataIndex)
    {
        if (userProfile == null)
        {
            return;
        }
        try
        {
            userProfile.e_avatar = avataIndex;
            await databaseReference.Child("users").Child(currentId).Child("e_avatar").SetValueAsync(avataIndex);
        }
        catch (Exception e)
        {
            Debug.LogError("캐릭터 변경 실패" + e.Message);
        }
    }
}