using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FIxedUIController : MonoBehaviour
{
    public GameObject settingsForm; // Panel GameObject�� ������ ����

    private static FIxedUIController instance = null;
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
            if (!settingsForm.activeSelf)
            {
                OpenSettingsForm();
            }
            else
            {
                CloseSettingsForm();
            }
        }
    }
    void OpenSettingsForm()
    {
        settingsForm = GameObject.Find("SettingsForm");
        settingsForm.SetActive(true);
        var seq = DOTween.Sequence();
        seq.Append(settingsForm.transform.DOScale(1.1f, 0.2f));
        seq.Append(settingsForm.transform.DOScale(1f, 0.1f));

        seq.Play();
    }
    void CloseSettingsForm()
    {
        settingsForm = GameObject.Find("SettingsForm");

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
