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
    public int e_avatar;
    // Start is called before the first frame update
    void Start()
    {
        characterChangeForm.SetActive(true);
        otherChangeForm.SetActive(false);
        profile = RecordManager.Instance.UserProfile;
        avatars = profile.avatars;
        e_avatar = profile.e_avatar;
        SearchingMyCharacter();
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
            buttons[i].onClick.AddListener(() => OnClickChangeCharacter(i));
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
            await RecordManager.Instance.UpdateEquipmentAvatar(e_avatar);
            SearchingMyCharacter();
        }
    }
    public void SearchingMyCharacter()
    {
        
        for (int i = 0; i < avatars.Count; i++)
        {
            if(i == e_avatar)
            {
                Image buttonColor = buttons[i].GetComponent<Image>();
                buttonColor.color = Color.red;
            }
            buttons[i].interactable = avatars[i];
            Transform lockImageTransform = buttons[i].transform.Find("Lock");
            GameObject lockImage = lockImageTransform.gameObject;
            lockImage.SetActive(!avatars[i]);
        }
    }
}
