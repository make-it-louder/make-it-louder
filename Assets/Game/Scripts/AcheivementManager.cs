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
    PlayerMove2D playerMove2D;
    // 업적달성 계산을 위한 초기 데이터
    float initialPlaytime;
    int initialJumpCount;
    int initialClearCount;

    List<bool> initialAchevements;
    List<bool> conditions;

    void Start()
    {
        GetUserData();
        playerMove2D = player.GetComponent<PlayerMove2D>();
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
        AddConditions();

        for (int i = 1; i < initialAchevements.Count; i++)
        {
            if (initialAchevements[i]) return;
            if (conditions[i-1])
            {
                profile.avatars[i] = true;
                await RecordManager.Instance.UpdateNewAvatar(i);
            }
        }
    }

    public void AddConditions ()
    {
        conditions.Add(initialJumpCount + playerMove2D.jumpCount >= 100);
        conditions.Add(initialJumpCount + playerMove2D.jumpCount >= 1000);
        conditions.Add(initialJumpCount + playerMove2D.jumpCount >= 3000);
        conditions.Add(initialJumpCount + playerMove2D.jumpCount >= 10000);
        conditions.Add(initialPlaytime + playerMove2D.playTime >= 300);
        conditions.Add(initialPlaytime + playerMove2D.playTime >= 1800);
        conditions.Add(initialPlaytime + playerMove2D.playTime >= 3600);
        conditions.Add(initialClearCount + playerMove2D.isClear == 1);
        conditions.Add(initialClearCount + playerMove2D.isClear == 3);
        conditions.Add(initialClearCount + playerMove2D.isClear == 10);
        conditions.Add(initialClearCount + playerMove2D.isClear == 102);
        conditions.Add(initialPlaytime + records["map1"].min_cleartime < 300);
        conditions.Add(initialJumpCount + records["map1"].count_minjump < 300);
        int cnt_avatars = 0;
        for (int i = 0; i < profile.avatars.Count; i++)
        {
            if (profile.avatars[i]) { cnt_avatars++; }
        }
        conditions.Add(cnt_avatars == 14);
    }
   /* // 1~4 점프카운트 업적
    public async void UnlockAcheivement1 () // 점프 100번업적
    {
        if (initialAchevements[1]) return;
        if (initialJumpCount + playerMove2D.jumpCount >= 100)
        {
            profile.avatars[1] = true;
            await RecordManager.Instance.UpdateNewAvatar(1);
        }
    }    
    public async void UnlockAcheivement2 () // 점프 1000번업적
    {
        if (initialAchevements[2]) return;
        if (initialJumpCount + playerMove2D.jumpCount >= 1000)
        {
            profile.avatars[2] = true;
            await RecordManager.Instance.UpdateNewAvatar(2);
        }
    }
    public async void UnlockAcheivement3 ()// 점프 3000번업적
    {
        if (initialAchevements[3]) return;
        if (initialJumpCount + playerMove2D.jumpCount >= 3000)
        {
            profile.avatars[3] = true;
            await RecordManager.Instance.UpdateNewAvatar(3);
        }
    }
    public async void UnlockAcheivement4() // 점프 10000번업적
    {
        if (initialAchevements[4]) return;
        if (initialJumpCount + playerMove2D.jumpCount >= 10000)
        {
            profile.avatars[4] = true;
            await RecordManager.Instance.UpdateNewAvatar(4);
        }
    }
    // 5~8 클리어 업적
    public async void UnlockAcheivement5() // 플레이타임 5분 업적
    {
        if (initialAchevements[5]) return;
        if (initialPlaytime + playerMove2D.playTime >= 300)
        {
            profile.avatars[5] = true;
            await RecordManager.Instance.UpdateNewAvatar(5);
        }
    }
    public async void UnlockAcheivement6() // 플레이타임 30분 업적
    {
        if (initialAchevements[6]) return;
        if (initialPlaytime + playerMove2D.playTime >= 1800)
        {
            profile.avatars[6] = true;
            await RecordManager.Instance.UpdateNewAvatar(6);
        }
    }
    public async void UnlockAcheivement7() // 플레이타임 60분 업적
    {
        if (initialAchevements[7]) return;
        if (initialPlaytime + playerMove2D.playTime >= 3600)
        {
            profile.avatars[7] = true;
            await RecordManager.Instance.UpdateNewAvatar(7);
        }
    }
    public async void UnlockAcheivement8() // 클리어 횟수 1회 업적
    {
        if (initialAchevements[8]) return;
        if (initialClearCount + playerMove2D.isClear == 1)
        {
            profile.avatars[8] = true;
            await RecordManager.Instance.UpdateNewAvatar(8);
        }
    }
    //9 ~시간안에 클리어
    public async void UnlockAcheivement9() // 클리어 횟수 3회 업적
    {
        if (initialAchevements[9]) return;
        if (initialClearCount + playerMove2D.isClear == 3)
        {
            profile.avatars[9] = true;
            await RecordManager.Instance.UpdateNewAvatar(9);
        }
    }

    //10 점프 ~만에 클리어
    public async void UnlockAcheivement10() // 클리어 횟수 10회 업적
    {
        if (initialAchevements[10]) return;
        if (initialClearCount + playerMove2D.isClear == 10)
        {
            profile.avatars[10] = true;
            await RecordManager.Instance.UpdateNewAvatar(10);
        }
    }
    //11~13 플레이타임 업적
    public async void UnlockAcheivement11() // 클리어 횟수 102회 업적
    {
        if (initialAchevements[11]) return;
        if (initialClearCount + playerMove2D.isClear == 102)
        {
            profile.avatars[11] = true;
            await RecordManager.Instance.UpdateNewAvatar(11);
        }
    }
    public async void UnlockAcheivement12() // ~분 안에 클리어
    {
        return;
        if (initialAchevements[12]) return;
        if (initialClearCount == 1)
        {
            profile.avatars[12] = true;
            await RecordManager.Instance.UpdateNewAvatar(12);
        }
    }
    public async void UnlockAcheivement13() // ~번 점프 안에 클리어
    {
        return;
        if (initialAchevements[13]) return;
        if (initialClearCount == 1)
        {
            profile.avatars[13] = true;
            await RecordManager.Instance.UpdateNewAvatar(13);
        }
    }
    public async void UnlockAcheivement14() //캐릭터 보유업적(보너스)
    {
        if (initialAchevements[14]) return;
        int ea = 0;
        for (int i = 0; i < profile.avatars.Count; i++)
        {
            if (profile.avatars[i]) ea++;
        }
        if (ea == 14)
        {
            profile.avatars[14] = true;
            await RecordManager.Instance.UpdateNewAvatar(14);
        }
    }*/
}
