using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AcheivementManager : MonoBehaviour
{
    // Profile, Record 테이블 지정
    FirebaseManager.Profile profile;
    Dictionary<string, FirebaseManager.Record> records;

    // player using
    public GameObject player;
    // 업적달성 계산을 위한 초기 데이터
    float initialPlaytime;
    int initialJumpCount;
    int initialClearCount;

    List<bool> initialAchevements;
    public List<bool> conditions;

    void Start()
    {
        GetUserData();
        initialPlaytime = records["map1"].playtime;
        initialJumpCount = records["map1"].count_jump;
        initialClearCount = records["map1"].count_clear;
        // 아바타인덱스 == 업적인덱스;
        initialAchevements = profile.avatars;
    }

    // Update is called once per frame
    void Update()
    {
    }
    //Profile, Record 테이블 할당
    private void GetUserData ()
    {
        profile = RecordManager.Instance.UserProfile;
        records = RecordManager.Instance.UserRecords;
    }

    public async Task UpdateAllAcheivement ()
    {
        Debug.Log("실행됨");
        AddConditions();

        for (int i = 1; i < initialAchevements.Count; i++)
        {
            Debug.Log(i.ToString());

            if (initialAchevements[i]) continue;
            Debug.Log(conditions[i - 1] + i.ToString());
            if (conditions[i-1])
            {
                await RecordManager.Instance.UpdateNewAvatar(i);
                Debug.Log(i + "번째 업적 업데이트함");
            }
        }
    }

    public void AddConditions ()
    {
        PlayerMove2D playerMove2D = player.GetComponent<PlayerMove2D>();
        Debug.Log("시작점프" + initialJumpCount);
        Debug.Log("뛴점프" + playerMove2D.jumpCount);
        conditions.Add(initialJumpCount + playerMove2D.jumpCount >= 100); // 1번 점프 100번
        conditions.Add(initialJumpCount + playerMove2D.jumpCount >= 1000); // 2번 점프 1000번
        conditions.Add(initialJumpCount + playerMove2D.jumpCount >= 3000); // 3번 점프 3000번
        conditions.Add(initialJumpCount + playerMove2D.jumpCount >= 10000); // 4번 점프 10000번
        conditions.Add(initialPlaytime + playerMove2D.playTime >= 300); // 5번 플레이타임 5분
        conditions.Add(initialPlaytime + playerMove2D.playTime >= 1800); // 6번 플레이타임 30분
        conditions.Add(initialPlaytime + playerMove2D.playTime >= 3600);  // 7번 플레이타임 1시간
        conditions.Add(initialClearCount + playerMove2D.isClear >= 1);  // 8번 최초클리어
        conditions.Add(initialClearCount + playerMove2D.isClear >= 3);  // 9번 3번 클리어
        conditions.Add(initialClearCount + playerMove2D.isClear >= 10);  // 10번 10번 클리어
        conditions.Add(initialClearCount + playerMove2D.isClear >= 102);  // 11번 102번 클리어
        conditions.Add(initialPlaytime + records["map1"].min_cleartime < 300 && playerMove2D.isClear == 1); // 12번 5분안에 클리어
        conditions.Add(initialJumpCount + records["map1"].count_minjump < 300 && playerMove2D.isClear == 1); // 13번 300번 점프 안에 클리어
        int cnt_avatars = 0; 
        for (int i = 0; i < profile.avatars.Count; i++)
        {
            if (profile.avatars[i]) { cnt_avatars++; }
        }
        conditions.Add(cnt_avatars == 14);  // 14번 모든 캐릭터 해금
    }
}
