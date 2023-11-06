using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSettings : MonoBehaviour
{
    public GameObject characterChangeForm;
    public GameObject otherChangeForm;

    FirebaseManager.Profile profile;

    public Button[] buttons;
    private List<bool> avatars;
    int e_avatar;
    // Start is called before the first frame update
    private void Awake()
    {
        characterChangeForm.SetActive(true);
        otherChangeForm.SetActive(false);
        profile = RecordManager.Instance.UserProfile;
        avatars = profile.avatars;
        e_avatar = profile.e_avatar;
        Debug.Log(e_avatar);
    }


    void Start()
    {
        SearchingMyCharacter(e_avatar);
        AddEventListener();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 클릭 이벤트 리스너 추가
    public void AddEventListener()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // 클로저 문제를 방지하기 위한 지역 변수
            buttons[i].onClick.AddListener(() => OnClickChangeCharacter(index));
        }
    }

    // 캐릭터창과 꾸미기창 바꾸는 토글
    public void toggleValueChange(bool value)
    {
        if (!value)
        {
            characterChangeForm.SetActive(false);
            otherChangeForm.SetActive(true);
        } else
        {
            characterChangeForm.SetActive(true);
            otherChangeForm.SetActive(false);
        }
    }

    public async void OnClickChangeCharacter (int btnIndex)
    {
        if (e_avatar == btnIndex)
        {
            return;
        } else
        {
            e_avatar = btnIndex;
            await RecordManager.Instance.UpdateEquipmentAvatar(btnIndex);
            SearchingMyCharacter(btnIndex);
        }
    }
    public void SearchingMyCharacter(int e_avatar)
    {
        Color whiteColor = Color.white; // 버튼의 기본 색상을 흰색으로 설정합니다.
        Color selectedColor = Color.red;
        for (int i = 0; i < avatars.Count; i++)
        {
            Image buttonImage = buttons[i].GetComponent<Image>();
            GameObject lockImage = buttons[i].transform.Find("Lock").gameObject;
            buttonImage.color = (i == e_avatar) ? selectedColor : whiteColor;
            buttons[i].interactable = avatars[i];
            lockImage.SetActive(!avatars[i]);
        }

    }
}
