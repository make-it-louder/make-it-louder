using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static FirebaseManager;

public class MaxJumpRanking : MonoBehaviour
{
    public GameObject rank1; // UI 프리팹
    public GameObject rank2; // UI 프리팹
    public GameObject rank3; // UI 프리팹
    public GameObject rank4; // UI 프리팹
    public Transform ranksParent; // 랭킹을 표시할 부모 객체


    public List<string> allUsers;
    public List<string> rankerNick;
    public List<string> rankerId;
    DatabaseReference databaseReference;

    public ScrollRect scrollRect;
    // Start is called before the first frame update
    void Start()
    {
        ScrollToTop();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public async Task<List<string>> ConvertUserIdToNickName()
    {
        var task = databaseReference.Child("users").GetValueAsync();
        await task;
        List<string> rankerNicks = new List<string>();
        DataSnapshot snapshot = task.Result;
        if (snapshot != null)
        {
            string json = snapshot.GetRawJsonValue();
            Dictionary<string, FirebaseManager.Profile> allUsersDict = JsonConvert.DeserializeObject<Dictionary<string, FirebaseManager.Profile>>(json);
            allUsers = allUsersDict.Keys.ToList();

            int cnt = 0;
            rankerId = RankingManager.Instance.maxJumpRank;
            for (int i = 0; i < rankerId.Count; i++)
            {
                foreach (string user in allUsers)
                {
                    if (rankerId[i] == user)
                    {
                        cnt++;
                        rankerNicks.Add(allUsersDict[user].username);
                        break;
                    }
                }
            }
            return rankerNicks;
        }
        else
        {
            Debug.Log("되겠냐 ㅋ");
            return null;
        }
    }

    public async void ShowUIRanking()
    {
        databaseReference = Instance.GetDatabaseReference();
        rankerNick = await ConvertUserIdToNickName();
        Dictionary<string, int> ranking = RankingManager.Instance.ranking.max_jump;
        UnityMainThreadDispatcher.Instance.ExecuteInUpdate(() =>
        {
            foreach (Transform child in ranksParent)
            {
                Destroy(child.gameObject); // 기존의 랭킹 UI 제거
            }
            Debug.Log(rankerNick.Count + "맥점카운트");
            // 데이터베이스에서 받은 데이터로 랭킹 UI 업데이트
            for (int i = 0; i < rankerNick.Count; i++)
            {
                if (i == 0) // 1등일때 1등 프리팹 사용
                {
                    GameObject rankItem = Instantiate(rank1, ranksParent);
                    rankItem.transform.Find("Content/NameText").GetComponent<TMP_Text>().text = rankerNick[i];
                    rankItem.transform.Find("Content/PointsText").GetComponent<TMP_Text>().text = ranking[rankerId[i]].ToString() + "회";

                }
                else if (i == 1) // 2등일때 2등 프리팹 사용
                {
                    GameObject rankItem = Instantiate(rank2, ranksParent);
                    rankItem.transform.Find("Content/NameText").GetComponent<TMP_Text>().text = rankerNick[i];
                    rankItem.transform.Find("Content/PointsText").GetComponent<TMP_Text>().text = ranking[rankerId[i]].ToString() + "회";
                }
                else if (i == 2) // 3등일때 3등 프리팹 사용
                {
                    GameObject rankItem = Instantiate(rank3, ranksParent);
                    rankItem.transform.Find("Content/NameText").GetComponent<TMP_Text>().text = rankerNick[i];
                    rankItem.transform.Find("Content/PointsText").GetComponent<TMP_Text>().text = ranking[rankerId[i]].ToString() + "회";
                }
                else // 4등 이하일때는 4등이하의 공동 프리팹 사용
                {
                    GameObject rankItem = Instantiate(rank4, ranksParent);
                    rankItem.transform.Find("Content/NameText").GetComponent<TMP_Text>().text = rankerNick[i];
                    rankItem.transform.Find("Content/PointsText").GetComponent<TMP_Text>().text = ranking[rankerId[i]].ToString() + "회";
                    rankItem.transform.Find("Content/RankText").GetComponent<TMP_Text>().text = (i + 1).ToString();
                }
            }
        });
    }


    public void ScrollToTop()
    {
        // verticalNormalizedPosition을 1로 설정하면 스크롤이 맨 위로 이동합니다.
        scrollRect.verticalNormalizedPosition = 1f;
    }
}


