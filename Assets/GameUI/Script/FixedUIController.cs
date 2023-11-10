using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Voice.Unity.Demos;

public class FixedUIController : MonoBehaviour
{
    public GameObject settingsForm; // Panel GameObject를 참조할 변수
    public GameObject audioSettingsForm;
    public GameObject displaySettingsForm;

    // 로딩스피너
    public GameObject loadingSpinner;
    //팝업
    public GameObject exitPopup;
    public GameObject quitPopup;

    //데이타
    public GameObject player;
    public TMP_Text mapName;

    public AcheivementManager acheivementManager;
    bool isIgnored;
    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();
        if (settingsForm == null)
        {
            return;
        }
        settingsForm.transform.localScale = Vector3.one * 0.1f;
        settingsForm.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("일단들어옴");
            if (settingsForm == null)
            {
                Debug.Log("세팅창세팅중");
                settingsForm = GameObject.Find("SettingsForm");
            }

            if (settingsForm != null && !settingsForm.activeSelf)
            {
                Debug.Log("켜는중");
                OpenSettingsForm();
            }
            else if (settingsForm != null)
            {
                Debug.Log("끄는중");
                CloseSettingsForm();
            }
        }
    }

    //열기 
    void OpenSettingsForm()
    {
        if (settingsForm == null)
        {
            settingsForm = GameObject.Find("SettingsForm");
        }

        if (settingsForm != null)
        {
            settingsForm.SetActive(true);
            var seq = DOTween.Sequence();
            seq.Append(settingsForm.transform.DOScale(1.1f, 0.2f));
            seq.Append(settingsForm.transform.DOScale(1f, 0.1f));
            seq.Play();
        }
    }

    void CloseSettingsForm()
    {
        if (settingsForm == null)
        {
            settingsForm = GameObject.Find("SettingsForm");
        }

        if (settingsForm != null)
        {
            var seq = DOTween.Sequence();
            transform.localScale = Vector3.one * 0.2f;
            seq.Append(settingsForm.transform.DOScale(1.1f, 0.1f));
            seq.Append(settingsForm.transform.DOScale(0.2f, 0.2f));
            seq.Play().OnComplete(() =>
            {
                settingsForm.SetActive(false);
            });
        }
    }

    
    //팝업띄우기
    public void openExitPopupWindow ()
    {
        exitPopup.SetActive(true);
    }
    
    public void closeExitPopupWindow ()
    {
        exitPopup.SetActive(false);
    }    
    
    public void openQuitPopupWindow ()
    {
        quitPopup.SetActive(true);
    }
    
    public void closeQuitPopupWindow ()
    {
        quitPopup.SetActive(false);
    }
    public async void LeaveThisRoom()
    {
        PlayerMove2D playerMove2D = player.GetComponent<PlayerMove2D>();
        float playTime = playerMove2D.playTime;
        int countJump = playerMove2D.jumpCount;
        int countFall = 0;
        loadingSpinner.SetActive(true);
        BackgroundMusicController backgroundMusicController = GameObject.FindObjectOfType<BackgroundMusicController>();
        backgroundMusicController.playLobby();
        await RecordManager.Instance.UpdateEndGameData("map1", playTime, countJump, countFall);
        // 포톤 로비연결 끊는 로직 추가해야함.

        if (acheivementManager != null)
        {
            Debug.Log("acvm 있어요");
            await acheivementManager.UpdateAllAcheivement();
        } else
        {
            Debug.Log("acvm이 없어요");
        }
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        SceneManager.LoadSceneAsync("LobbyTest");
    }

    public async void QuitGame ()
    {
        PlayerMove2D playerMove2D = player.GetComponent<PlayerMove2D>();
        float playTime = playerMove2D.playTime;
        int countJump = playerMove2D.jumpCount;
        int countFall = 0;
        loadingSpinner.SetActive(true);
        BackgroundMusicController backgroundMusicController = GameObject.FindObjectOfType<BackgroundMusicController>();
        backgroundMusicController.playLobby();
        await RecordManager.Instance.UpdateEndGameData("map1", playTime, countJump, countFall);
        // 포톤 로비연결 끊는 로직 추가해야함.

        if (acheivementManager != null)
        {
            Debug.Log("acvm 있어요");
            await acheivementManager.UpdateAllAcheivement();
        }
        else
        {
            Debug.Log("acvm이 없어요");
        }
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        Application.Quit();
    }
    public void Logout ()
    {
        FirebaseManager.Instance.SignOut();
        SceneManager.LoadScene("Login");
    }
}
