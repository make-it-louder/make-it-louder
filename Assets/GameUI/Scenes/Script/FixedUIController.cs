using DG.Tweening;
using TMPro;
using UnityEngine;
public class FixedUIController : MonoBehaviour
{
    public GameObject settingsForm; // Panel GameObject를 참조할 변수
    public GameObject settingsDetail;
    public GameObject audioSettingsForm;
    public GameObject displaySettingsForm;
    private static FixedUIController instance = null;
    public TMP_Text title;

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
                audioSettingsForm.SetActive(false);
                displaySettingsForm.SetActive(false);
                settingsDetail.SetActive(true);
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

    public void ChangeForm(GameObject formName)
    {
        if (formName != null)
        {
            Debug.Log("있대");
            settingsDetail.SetActive(false);
            formName.SetActive(true);
        }
        else
        {
            Debug.LogError("The Form is not found.");
        }
    }

/*    public void GoToSettingsForm()
    {
        settingsForm.SetActive(true);
        settingsDetail.SetActive(false);
        audioSettingsForm.SetActive(false);
        displaySettingsForm.SetActive(false);
    }*/
}
