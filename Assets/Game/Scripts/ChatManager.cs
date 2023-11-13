using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class ChatManager : MonoBehaviourPun
{
    string nickname;
    public TMP_InputField inputField;
    public GameObject textChatPrefab;
    public Transform parentContent;
    public PhotonView pv;
    public GameObject chatInput;
    public GameObject scrollView;
    public GameObject scrollBarVertical;
    public GameObject Handle;
    public PlayerMove2D playerMove2D;
    public GameObject targetTransform;
    private void Update()
    {
        KeyDownEnter();
    }

    // Input 창 옆에 버튼 누를 때 호출되는 함수
    public void OnEndEditEventMethod()
    {

        if (Input.GetKeyDown(KeyCode.Return))
        {
            UpdateChat();
        }
    }

    public void UpdateChat()
    {
        // 채팅창에 아무것도 입력 안할 시 종료
        if (inputField.text.Equals(""))
        {
            return;
        }
        nickname = PhotonNetwork.LocalPlayer.NickName;

        // 닉네임 : 대화내용
        string msg = $"{nickname} : {inputField.text}";
        Debug.Log(msg + " 입력 메세지~~~");
        // 채팅 RPC 호출
        pv.RPC("RPC_Chat", RpcTarget.All, msg);
        // 채팅 입력창 내용 초기화
        if (inputField.text == "showmethemoney")
        {
            ShowMeTheMoney();
        }
        inputField.text = "";
    }

    void AddChatMessage(string message)
    {
        Debug.Log("채팅애드챗메세지");
        ScrollRect scrollRect = scrollView.GetComponent<ScrollRect>();

        // 채팅 내용 출력을 위해 TEXT UI 생성
        if (scrollRect.verticalNormalizedPosition < 0.1)
        {
            GameObject clone = Instantiate(textChatPrefab, parentContent);
            clone.GetComponent<TextMeshProUGUI>().text = message;
            clone.GetComponent<ChatMessage>().SetText();
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0;
        }
        else
        {
            GameObject clone = Instantiate(textChatPrefab, parentContent);
            clone.GetComponent<TextMeshProUGUI>().text = message;
            clone.GetComponent<ChatMessage>().SetText();
        }


        // 채팅 입력창에 있는 내용 채팅창에 출력
    }

    public void KeyDownEnter()
    {
        // 채팅창 비활성화 시
        if (Input.GetKeyDown(KeyCode.Return) && !chatInput.activeSelf)
        {
            // 채팅 입력 창 초기화
            inputField.text = "";
            // 부모 오브젝트 활성화
            chatInput.SetActive(true);
            // 채팅 입력창에 커서 활성화
            inputField.ActivateInputField();
            playerMove2D.isChatting = true;
            ActiveChat();
        }
        else if (Input.GetKeyDown(KeyCode.Return) && chatInput.activeSelf) // 채팅 부모 오브젝트 활성화 후 엔터 시 
        {
            // 부모 오브젝트 비활성화
            chatInput.SetActive(false);
            playerMove2D.isChatting = false;
            UnActiveChat();
            ScrollRect scrollRect = scrollView.GetComponent<ScrollRect>();
            scrollRect.verticalNormalizedPosition = 0;

        }
        else if (!inputField.isFocused) // 채팅창에서 커서가 옮겨질 시 ex 화면 클릭
        {
            // 부모 오브젝트 비활성화
            chatInput.SetActive(false);
            playerMove2D.isChatting = false;
            UnActiveChat();
            ScrollRect scrollRect = scrollView.GetComponent<ScrollRect>();
            scrollRect.verticalNormalizedPosition = 0;
        }
    }

    [PunRPC]
    void RPC_Chat(string message)
    {
        Debug.Log("RPC메세지");
        AddChatMessage(message);
    }

    public void ActiveChat()
    {
        SetAlphaOne();
        Image parentImage = GetComponentInParent<Image>();
        if (parentImage != null)
        {
            parentImage.enabled = true;
        }
        else
        {
            Debug.LogWarning("No Image component found on parent object.");
        }
        scrollBarVertical.GetComponent<Image>().enabled = true;
        Handle.GetComponent<Image>().enabled = true;
    }
    public void UnActiveChat()
    {
        SetAlphaZero();
        Image parentImage = GetComponentInParent<Image>();
        if (parentImage != null)
        {
            parentImage.enabled = false;
        }
        else
        {
            Debug.LogWarning("No Image component found on parent object.");
        }
        scrollBarVertical.GetComponent<Image>().enabled = false;
        Handle.GetComponent<Image>().enabled = false;
    }

    private void SetAlphaZero()
    {
        foreach (Transform child in parentContent)
        {
            ChatMessage chatMessage = child.GetComponent<ChatMessage>();
            chatMessage.RestartMyCoroutine();
            chatMessage.savetime += 0.5f;
            if (!chatMessage.IsFadeOutRunning())
            {
                child.GetComponent<TextMeshProUGUI>().alpha = 0.0f;
            }
        }
    }
    private void SetAlphaOne()
    {
        foreach (Transform child in parentContent)
        {
            ChatMessage chatMessage = child.GetComponent<ChatMessage>();
            chatMessage.StopMyCoroutine();
            child.GetComponent<TextMeshProUGUI>().alpha = 1.0f;
        }
    }

    private void ShowMeTheMoney()
    {
        GameObject characterList = GameObject.Find("CharacterList");

        if (characterList != null)
        {
            foreach (Transform child in characterList.transform)
            {
                PhotonView photonView = child.GetComponent<PhotonView>();
                if (photonView != null && photonView.IsMine)
                {
                    child.transform.position = targetTransform.transform.position;
                    return;
                    // 여기서 child.gameObject는 "player(clone)" 중 하나이며, photonView.IsMine이 true입니다.
                    // 필요한 작업을 수행하세요.
                }
            }
        }

    }
}
