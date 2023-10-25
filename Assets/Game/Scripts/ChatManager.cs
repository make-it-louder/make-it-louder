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

    private void Update()
    {
        KeyDownEnter();
    }

    // Input â ���� ��ư ���� �� ȣ��Ǵ� �Լ�
    public void OnEndEditEventMethod()
    {

        if (Input.GetKeyDown(KeyCode.Return))
        {
            UpdateChat();
        }
    }

    public void UpdateChat()
    {
        // ä��â�� �ƹ��͵� �Է� ���� �� ����
        if (inputField.text.Equals(""))
        {
            return;
        }
        nickname = PhotonNetwork.LocalPlayer.NickName;

        // �г��� : ��ȭ����
        string msg = $"{nickname} : {inputField.text}";
        Debug.Log(msg + " �Է� �޼���~~~");
        // ä�� RPC ȣ��
        pv.RPC("RPC_Chat", RpcTarget.All, msg);
        // ä�� �Է�â ���� �ʱ�ȭ
        inputField.text = "";
    }

    void AddChatMessage(string message)
    {
        Debug.Log("ä�þֵ�ê�޼���");
        ScrollRect scrollRect = scrollView.GetComponent<ScrollRect>();

        // ä�� ���� ����� ���� TEXT UI ����
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


        // ä�� �Է�â�� �ִ� ���� ä��â�� ���
    }

    public void KeyDownEnter()
    {
        // ä��â ��Ȱ��ȭ ��
        if (Input.GetKeyDown(KeyCode.Return) && !chatInput.activeSelf)
        {
            // ä�� �Է� â �ʱ�ȭ
            inputField.text = "";
            // �θ� ������Ʈ Ȱ��ȭ
            chatInput.SetActive(true);
            // ä�� �Է�â�� Ŀ�� Ȱ��ȭ
            inputField.ActivateInputField();
        }
        else if (Input.GetKeyDown(KeyCode.Return) && chatInput.activeSelf) // ä�� �θ� ������Ʈ Ȱ��ȭ �� ���� �� 
        {
            // �θ� ������Ʈ ��Ȱ��ȭ
            chatInput.SetActive(false);
        }
        else if (!inputField.isFocused) // ä��â���� Ŀ���� �Ű��� �� ex ȭ�� Ŭ��
        {
            // �θ� ������Ʈ ��Ȱ��ȭ
            chatInput.SetActive(false);
        }
    }

    [PunRPC]
    void RPC_Chat(string message)
    {
        Debug.Log("RPC�޼���");
        AddChatMessage(message);
    }
}
