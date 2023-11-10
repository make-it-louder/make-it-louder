using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System; // TimeSpan을 사용하기 위해 추가

public class AchievementScript : MonoBehaviour
{
    FirebaseManager.Profile profile;
    Dictionary<string, FirebaseManager.Record> record;

    public TMP_Text name;
    public TMP_Text AchieveCount;
    public TMP_Text AchieveCount2;
    public TMP_Text AchieveCount3;
    public TMP_Text AchieveCount4;

    public TMP_Text PlayTimeAchieve1MinText;
    public TMP_Text PlayTimeAchieve10MinText;
    public TMP_Text PlayTimeAchieve60MinText;

    public TMP_Text ClearCount1;
    public TMP_Text ClearCount3;
    public TMP_Text ClearCount10;
    public TMP_Text ClearCount102;

    public TMP_Text ChallangeTime15;

    public TMP_Text ChallangeJump100;

    public TMP_Text AchieveCharacterNumText;


    // profile info
    public TMP_Text playTimeText;

    public TMP_Text ProfileCount;

    public TMP_Text ClearCount;

    public TMP_Text MinCleartime;

    public TMP_Text MinClearJump;

    public TMP_Text CharacterNum;

    // 아이콘들에 대한 참조
    public GameObject icon1;
    public GameObject icon2;
    public GameObject icon3;
    public GameObject icon4;
    public GameObject PlayTimeIcon1Min;
    public GameObject PlayTimeIcon10Min;
    public GameObject PlayTimeIcon60Min;
    public GameObject ClearCounticon1;
    public GameObject ClearCounticon3;
    public GameObject ClearCounticon10;
    public GameObject ClearCounticon102;


    public GameObject ChallangeTime15icon;
    public GameObject ChallangeJump100icon;

    public GameObject CharacterNumicon;
    

    // Start is called before the first frame update
    public void OnButtonClick()
    {
        GetUserData();

        // 사용자 이름 업데이트
        name.text = profile.username;
        
        // 각 달성도에 대한 최댓값 설정
        int maxForAchieveCount = 100;
        int maxForAchieveCount2 = 1000;
        int maxForAchieveCount3 = 3000;
        int maxForAchieveCount4 = 10000;

        int maxForClearCount1 = 1;
        int maxForClearCount3 = 3;
        int maxForClearCount10 = 10;
        int maxForClearCount102 = 102;

        int minForChallangeJump100 = 100;

        int maxCharacterAchievement = 14;



        // 현재 점프 횟수 가져오기
        int currentJumpCount = record["map1"].count_jump;

        // 달성도 텍스트 업데이트 및 아이콘 활성화/비활성화
        UpdateAchievementJumpCount(AchieveCount, icon1, currentJumpCount, maxForAchieveCount);
        UpdateAchievementJumpCount(AchieveCount2, icon2, currentJumpCount, maxForAchieveCount2);
        UpdateAchievementJumpCount(AchieveCount3, icon3, currentJumpCount, maxForAchieveCount3);
        UpdateAchievementJumpCount(AchieveCount4, icon4, currentJumpCount, maxForAchieveCount4);

        // 현재 클리어 횟수
        int currentClearCount = record["map1"].count_clear;

        UpdateAchievementClearCount(ClearCount1, ClearCounticon1, currentClearCount, maxForClearCount1);
        UpdateAchievementClearCount(ClearCount3, ClearCounticon3, currentClearCount, maxForClearCount3);
        UpdateAchievementClearCount(ClearCount10, ClearCounticon10, currentClearCount, maxForClearCount10);
        UpdateAchievementClearCount(ClearCount102, ClearCounticon102, currentClearCount, maxForClearCount102);

        // CharacterNum
        int cnt = 0;

        for (int i = 0; i < profile.avatars.Count; i++)
        {
            if (profile.avatars[i])
            {
                cnt++;
            }
        }

        CharacterNum.text = cnt.ToString() + "개";

        UpdateAchievementCharacterNum(AchieveCharacterNumText, CharacterNumicon, cnt, maxCharacterAchievement);


        // 업적을 위한 최소점프

        int MinCurrentClaerJump = record["map1"].count_minjump;


        UpdateAchievementChallangeJump(ChallangeJump100, ChallangeJump100icon, MinCurrentClaerJump, minForChallangeJump100);





        // 프로필 카운트는 최댓값에 제한 없이 현재 점프 횟수를 그대로 표시
        ProfileCount.text = currentJumpCount.ToString("N0") + "회";

        // playtime을 double 형태로 가져온 후, 정수 부분만 TimeSpan으로 변환
        double playTimeSeconds = record["map1"].playtime; // Firebase에서 가져온 시간 데이터
        int totalPlayTime = (int)Math.Floor(playTimeSeconds); // 소수점 이하를 버림

        TimeSpan time = TimeSpan.FromSeconds(totalPlayTime);

        // TimeSpan에서 시, 분, 초를 가져와서 형식화합니다.
        playTimeText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);

        // playtime을 double 형태로 가져온 후, 정수 부분만 TimeSpan으로 변환
       
        UpdatePlayTimeAchievements(playTimeSeconds); // 플레이 타임 달성도를 업데이트하는 함수를 호출합니다.


        // ClearCount
        int CountClear = record["map1"].count_clear;

        ClearCount.text = CountClear.ToString() + "회";

        // MinCleartime
        double Min_clearsecond = record["map1"].min_cleartime; // Firebase에서 가져온 시간 데이터
        int Minclaertime = (int)Math.Floor(Min_clearsecond); // 소수점 이하를 버림
        TimeSpan time2 = TimeSpan.FromSeconds(Minclaertime);
        MinCleartime.text = string.Format("{0:D2}:{1:D2}:{2:D2}", time2.Hours, time2.Minutes, time2.Seconds);

        UpdateMinTimeAchievment(Min_clearsecond);

        // MinClearJump
        int Count_MinJump = record["map1"].count_minjump;

        MinClearJump.text = Count_MinJump.ToString() + "회";




    }





    private void GetUserData()
    {
        // RecordManager 인스턴스로부터 사용자 데이터 가져오기
        profile = RecordManager.Instance.UserProfile;
        record = RecordManager.Instance.UserRecords;
    }
    // 총 점프 업적
    private void UpdateAchievementJumpCount(TMP_Text achieveText, GameObject icon, int currentCount, int maxCount)
    {
        // 달성도 텍스트 형식에 맞게 업데이트
        achieveText.text = string.Format("현재달성도 [{0:N0}/{1:N0}]", Mathf.Min(currentCount, maxCount), maxCount);

        // 아이콘 활성화/비활성화
        icon.SetActive(currentCount >= maxCount);
    }
    // 클리어횟수 업적
    private void UpdateAchievementClearCount(TMP_Text achieveText, GameObject icon, int currentClearCount, int maxClearCount)
    {
        achieveText.text = string.Format("현재달성도 [{0}/{1}]", Mathf.Min(currentClearCount, maxClearCount), maxClearCount);

        icon.SetActive(currentClearCount >= maxClearCount);
    }

    private void UpdatePlayTimeAchievements(double playTimeSeconds)
    {
        // 플레이 타임 달성도 업데이트
        int minutes = (int)(playTimeSeconds / 60); // 플레이 타임을 분으로 변환

        UpdateAchievementPlayTime(PlayTimeAchieve1MinText, PlayTimeIcon1Min, minutes, 1); // 1분 달성도
        UpdateAchievementPlayTime(PlayTimeAchieve10MinText, PlayTimeIcon10Min, minutes, 10); // 10분 달성도
        UpdateAchievementPlayTime(PlayTimeAchieve60MinText, PlayTimeIcon60Min, minutes, 60); // 60분 달성도
    }

    private void UpdateAchievementPlayTime(TMP_Text achieveText, GameObject icon, int currentMinutes, int maxMinutes)
    {
        // 달성도 텍스트 형식에 맞게 업데이트
        achieveText.text = string.Format("현재달성도 [{0}/{1}]", Mathf.Min(currentMinutes,maxMinutes), maxMinutes);

        // 아이콘 활성화/비활성화
        icon.SetActive(currentMinutes >= maxMinutes);
    }
    ////////////////////////
    
    private void UpdateMinTimeAchievment(double Min_clearsecond)
    {
        int minutes = (int)(Min_clearsecond / 60); // 플레이 타임을 분으로 변환
        UpdateAchievementChallengeMinTime(ChallangeTime15, ChallangeTime15icon, minutes, 15);
    }

    private void UpdateAchievementChallengeMinTime(TMP_Text achieveText, GameObject icon, int currentMinutes, int minMinutes)
    {
        // 달성도 텍스트 형식에 맞게 업데이트
        achieveText.text = string.Format("현재달성도 [{0}/{1}]", Mathf.Max(currentMinutes, minMinutes), minMinutes);

        // 아이콘 활성화/비활성화
        icon.SetActive(minMinutes >= currentMinutes);
    }

    ////////////////////////
    private void UpdateAchievementCharacterNum(TMP_Text achieveText, GameObject icon, int currentNum, int maxNum)
    {
        achieveText.text = string.Format("현재달성도 [{0}/{1}]", Mathf.Min(currentNum, maxNum), maxNum);

        icon.SetActive(currentNum >= maxNum);
    }

    private void UpdateAchievementChallangeJump(TMP_Text achieveText, GameObject icon, int MinCurrentClaerJump, int minForChallangeJump100)
    {
        achieveText.text = string.Format("현재달성도 [{0:N0}/{1:N0}]", Mathf.Max(MinCurrentClaerJump, minForChallangeJump100), minForChallangeJump100);

        icon.SetActive(minForChallangeJump100 >= MinCurrentClaerJump);
    }


}
