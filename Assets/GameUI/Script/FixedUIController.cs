using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FixedUIController : MonoBehaviour
{
    public GameObject settingsForm; // Panel GameObject를 참조할 변수
    public GameObject audioSettingsForm;
    public GameObject displaySettingsForm;
    private static FixedUIController instance = null;

    //팝업
    public GameObject exitPopup;

    public GameObject quitPopup;
    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();
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
    public void LeaveThisRoom()
    {
        SceneManager.LoadScene("LobbyTest");
        Destroy(this.gameObject);
    }

    public void QuitGame ()
    {
        Application.Quit();
    }
}
