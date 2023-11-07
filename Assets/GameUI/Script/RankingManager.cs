using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    private static RankingManager instance;
    DatabaseReference databaseReference;
    public FirebaseManager.Ranking ranking;
    public List<string> cleartimeRank;
    public static RankingManager Instance
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

    private async void Start()
    {
        databaseReference = FirebaseManager.Instance.GetDatabaseReference();
        await GetRanking();
        GetClearTimeRank();
    }

    public async Task<FirebaseManager.Ranking> GetRanking ()
    {
        var task = databaseReference.Child("ranking").GetValueAsync();
        await task;

        if (task.IsCompleted)
        {
            DataSnapshot snapshot = task.Result;

            if (snapshot.Exists)
            {
                string json = snapshot.GetRawJsonValue();
                FirebaseManager.Ranking rankingDe = JsonConvert.DeserializeObject<FirebaseManager.Ranking>(json);
                ranking = rankingDe;
                return ranking;
            }
            else
            {
                Debug.LogError("the ranking data does not exist");
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

    public void GetClearTimeRank () // 클리어타임값을 키값(클리어타임) 순으로 정렬한 리스트 만들기
    {
        // 클리어타임 Key값을 정렬
        cleartimeRank = ranking.cleartime.OrderBy(kvp => kvp.Value).Select(kvp => kvp.Key).ToList();
        
        Debug.Log("랭킹정렬");
    }

    public async void UpdateClearTimeRank (float minClearTime, string userId)
    {
        string minCT = minClearTime.ToString();
        if (cleartimeRank.Count < 10) // 10개가 안될때 그냥 넣으면 됨.
        {
            Debug.Log("빈집 넣을게");
            ranking.cleartime.Add(userId, minCT);
            await UpdateClearTimeDB();

        }
        else  // 이미 10위 이상있을때 10위를 빼고 넣으면 됨.
        {
            Debug.Log("넣을게");
            float lastRankTime = float.Parse(ranking.cleartime[cleartimeRank[9]]);
            if (minClearTime < lastRankTime)  //랭킹안에 들때 넣기
            {
                ranking.cleartime.Remove(cleartimeRank[9]);
                ranking.cleartime.Add(userId, minCT);
                await UpdateClearTimeDB();
            }
        }
        GetClearTimeRank(); // 리스트 재정렬
    }

    public async Task UpdateClearTimeDB()
    {
        // `ranking` 객체를 JSON 문자열로 변환합니다.
        string jsonRanking = JsonConvert.SerializeObject(ranking.cleartime);
        Debug.Log(jsonRanking);
        // Firebase Realtime Database의 "ranking/cleartime" 경로에 업데이트합니다.
        try
        {
            await databaseReference.Child("ranking").Child("cleartime").SetRawJsonValueAsync(jsonRanking);
            await GetRanking();
            Debug.Log("DB업뎃완료");
        } catch (Exception ex)
        {
            Debug.Log("되겠냐?ㅋ" + ex);
        }

    }
}
