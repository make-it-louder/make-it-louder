using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    public RectTransform cursorImage; // �̹��� ������Ʈ�� �Ҵ��� ����
    public Button[] buttons; // ���� ��ư���� �Ҵ��� �迭

    private int selectedIndex = 0; // ���� ���õ� ��ư �ε���

    void Start()
    {
        // �ʱ� �������� Ŀ���� ù ��° ��ư ��ġ�� �̵�
        MoveCursorToButton(0);
    }

    void Update()
    {
        // ���� ȭ��ǥ Ű�� ������ ���� ��ư�� ����
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex = (selectedIndex - 1 + buttons.Length) % buttons.Length;
            MoveCursorToButton(selectedIndex);
        }
        // �Ʒ��� ȭ��ǥ Ű�� ������ ���� ��ư�� ����
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedIndex = (selectedIndex + 1) % buttons.Length;
            MoveCursorToButton(selectedIndex);
        }
        // ���� Ű�� ������ ���� ������ ��ư�� ����� ����
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            buttons[selectedIndex].onClick.Invoke();
        }
    }

    // Ŀ���� Ư�� ��ư���� �̵���Ű�� �Լ�
    void MoveCursorToButton(int index)
    {
        cursorImage.anchoredPosition = new Vector2(cursorImage.anchoredPosition.x, buttons[index].GetComponent<RectTransform>().anchoredPosition.y);
    }
}