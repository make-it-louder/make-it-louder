using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    // 여러 개의 팝업을 저장할 리스트
    public List<GameObject> popups;

    // Start is called before the first frame update
    void Start()
    {
        // 모든 팝업을 비활성화합니다.
        foreach (var popup in popups)
        {
            if (popup != null)
            {
                popup.SetActive(false);
            }
        }
    }

    // ESC 키가 눌렸을 때 모든 팝업을 닫습니다.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseAllPopups();
        }
    }

    // 특정 팝업을 여는 메서드
    public void OpenPopup(GameObject popup)
    {
        // 다른 모든 팝업을 닫습니다.
        CloseAllPopups();

        // 선택된 팝업을 활성화합니다.
        Animator animator = popup.GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play("Open");
        }
        popup.SetActive(true);
    }
    public void OpenPopup(int idx)
    {
        if (idx < 0 || idx >= popups.Count)
        {
            Debug.LogWarning("invalid idx");
            return;
        }
        OpenPopup(popups[idx]);
    }

    // 모든 팝업을 닫는 메서드
    public void CloseAllPopups()
    {
        foreach (var popup in popups)
        {
            Animator animator = popup.GetComponent<Animator>();
            if (animator != null && animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
            {
                animator.Play("Close");
            }
            popup.SetActive(false);
        }
    }
}

