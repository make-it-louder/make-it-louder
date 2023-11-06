using System.Collections;
using UnityEngine;

public class Tp : MonoBehaviour
{
    public GameObject popup;
    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        if (popup != null)
        {
            animator = popup.GetComponent<Animator>();
        }
        popup.SetActive(false);
    }

        // Update is called once per frame
    void Update()
    {
        // ESC 키가 눌렸을 때
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePopup();
        }
    }

    // OpenPopup 메서드를 호출하여 팝업을 엽니다.
    public void OpenPopup()
    {
        popup.SetActive(true);
        
        if (animator != null)
        {
            animator.Play("Open");
        }
    }

    // ClosePopup 메서드를 호출하여 팝업을 닫습니다.
    public void ClosePopup()
    {
        if (animator != null && animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
        {
            animator.Play("Close");
        }

        // 닫기 애니메이션이 끝나는 것을 기다리는 대신 바로 숨깁니다.
        popup.SetActive(false);

        
    }

    public void OnBackgroundClicked()
    {
        // 배경이 클릭되었을 때 호출될 메서드
        ClosePopup();
    }

}
