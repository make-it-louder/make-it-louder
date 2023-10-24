using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    public RectTransform cursorImage; // 이미지 컴포넌트를 할당할 변수
    public Button[] buttons; // 설정 버튼들을 할당할 배열

    private int selectedIndex = 0; // 현재 선택된 버튼 인덱스

    void Start()
    {
        // 초기 설정으로 커서를 첫 번째 버튼 위치로 이동
        MoveCursorToButton(0);
    }

    void Update()
    {
        // 위쪽 화살표 키를 누르면 이전 버튼을 선택
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex = (selectedIndex - 1 + buttons.Length) % buttons.Length;
            MoveCursorToButton(selectedIndex);
        }
        // 아래쪽 화살표 키를 누르면 다음 버튼을 선택
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedIndex = (selectedIndex + 1) % buttons.Length;
            MoveCursorToButton(selectedIndex);
        }
        // 엔터 키를 누르면 현재 선택한 버튼의 기능을 실행
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            buttons[selectedIndex].onClick.Invoke();
        }
    }

    // 커서를 특정 버튼으로 이동시키는 함수
    void MoveCursorToButton(int index)
    {
        cursorImage.anchoredPosition = new Vector2(cursorImage.anchoredPosition.x, buttons[index].GetComponent<RectTransform>().anchoredPosition.y);
    }
}